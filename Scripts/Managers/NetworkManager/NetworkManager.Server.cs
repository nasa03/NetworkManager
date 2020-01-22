// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Wovencode.Database;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace Wovencode.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {
    	
    	// -------------------------------------------------------------------------------
		// OnStartServer
		// @Server
		// -------------------------------------------------------------------------------
        public override void OnStartServer()
        {
        
#if wDB
			DatabaseManager.singleton.Init();
#endif

            // ---- User
            NetworkServer.RegisterHandler<ClientMessageRequestUserLogin>(OnClientMessageRequestUserLogin);
            NetworkServer.RegisterHandler<ClientMessageRequestUserRegister>(OnClientMessageRequestUserRegister);
            NetworkServer.RegisterHandler<ClientMessageRequestUserDelete>(OnClientMessageRequestUserDelete);
            NetworkServer.RegisterHandler<ClientMessageRequestUserChangePassword>(OnClientMessageRequestUserChangePassword);
            NetworkServer.RegisterHandler<ClientMessageRequestUserConfirm>(OnClientMessageRequestUserConfirm);
            
            // ---- Player
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerLogin>(OnClientMessageRequestPlayerLogin);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerRegister>(OnClientMessageRequestPlayerRegister);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerDelete>(OnClientMessageRequestPlayerDelete);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerSwitchServer>(OnClientMessageRequestPlayerSwitchServer);
        	
			eventListener.onStartServer.Invoke();
			this.InvokeInstanceDevExtMethods(nameof(OnStartServer));
        	
        }
           
        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================
        
        // -------------------------------------------------------------------------------
		// OnClientMessageRequest
		// @Client -> @Server
		// --------------------------------------------------------------------------------
        void OnClientMessageRequest(NetworkConnection conn, ClientMessageRequest msg)
        {
    		// do nothing (this message is never called directly)
        }
        
        // ========================== MESSAGE HANDLERS - USER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserLogin
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
        void OnClientMessageRequestUserLogin(NetworkConnection conn, ClientMessageRequestUserLogin msg)
		{
			
			ServerMessageResponseUserLogin message = new ServerMessageResponseUserLogin
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryUserLogin(msg.username, msg.password))
			{
				LoginUser(conn, msg.username);
				
				// TODO: Add increased maxPlayers from user data later
				message.maxPlayers = GameRulesTemplate.singleton.maxPlayersPerUser;

				message.LoadPlayerPreviews(DatabaseManager.singleton.GetPlayers(msg.username));
								
				eventListener.onUserLogin.Invoke(conn);
				message.text = systemText.userLoginSuccess;
			}
			else
			{
				message.text = systemText.userLoginFailure;
				message.success = false;
			}
					
			conn.Send(message);
			
		}
		
       	// -------------------------------------------------------------------------------
        // OnClientMessageRequestUserRegister
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnClientMessageRequestUserRegister(NetworkConnection conn, ClientMessageRequestUserRegister msg)
        {
        	
        	ServerMessageResponseUserRegister message = new ServerMessageResponseUserRegister
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};

        	if (DatabaseManager.singleton.TryUserRegister(msg.username, msg.password, msg.email, msg.deviceid))
			{
				DatabaseManager.singleton.SaveDataUser(msg.username, false);
				eventListener.onUserRegister.Invoke(msg.username);
				message.text = systemText.userRegisterSuccess;
			}
			else
			{
				message.text = systemText.userRegisterFailure;
				message.success = false;
			}

        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserDelete
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnClientMessageRequestUserDelete(NetworkConnection conn, ClientMessageRequestUserDelete msg)
        {
        	
        	ServerMessageResponseUserDelete message = new ServerMessageResponseUserDelete
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryUserDelete(msg.username, msg.password))
			{
				message.text = systemText.userDeleteSuccess;
				eventListener.onUserDelete.Invoke(msg.username);
			}
			else
			{
				message.text = systemText.userDeleteFailure;
				message.success = false;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserChangePassword
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnClientMessageRequestUserChangePassword(NetworkConnection conn, ClientMessageRequestUserChangePassword msg)
        {
        	
        	ServerMessageResponseUserChangePassword message = new ServerMessageResponseUserChangePassword
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryUserChangePassword(msg.username, msg.oldPassword, msg.newPassword))
			{
				message.text = systemText.userChangePasswordSuccess;
			}
			else
			{
				message.text = systemText.userChangePasswordFailure;
				message.success = false;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestUserConfirm
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnClientMessageRequestUserConfirm(NetworkConnection conn, ClientMessageRequestUserConfirm msg)
        {
        	
        	ServerMessageResponseUserConfirm message = new ServerMessageResponseUserConfirm
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryUserConfirm(msg.username, msg.password))
			{
				message.text = systemText.userConfirmSuccess;
				eventListener.onUserConfirm.Invoke(msg.username);
			}
			else
			{
				message.text = systemText.userConfirmFailure;
				message.success = false;
			}
					
        	conn.Send(message);
        	
        }
        
        // ======================== MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerLogin
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
        void OnClientMessageRequestPlayerLogin(NetworkConnection conn, ClientMessageRequestPlayerLogin msg)
		{
			
			ServerMessageResponsePlayerLogin message = new ServerMessageResponsePlayerLogin
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryPlayerLogin(msg.playername, msg.username))
			{
				LoginPlayer(conn, msg.playername);
				
				message.text = systemText.playerLoginSuccess;
				eventListener.onPlayerLogin.Invoke(conn.identity.gameObject);
			}
			else
			{
				message.text = systemText.playerLoginFailure;
				message.success = false;
			}
					
			conn.Send(message);
			
		}
		
       	// -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerRegister
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnClientMessageRequestPlayerRegister(NetworkConnection conn, ClientMessageRequestPlayerRegister msg)
        {
        	
        	ServerMessageResponsePlayerRegister message = new ServerMessageResponsePlayerRegister
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerRegister(msg.playername, msg.username))
			{
				RegisterPlayer(msg.playername);
				message.text = systemText.playerRegisterSuccess;
				eventListener.onPlayerRegister.Invoke(msg.playername);
			}
			else
			{
				message.text = systemText.playerRegisterFailure;
				message.success = false;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerDelete
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnClientMessageRequestPlayerDelete(NetworkConnection conn, ClientMessageRequestPlayerDelete msg)
        {
        	
        	ServerMessageResponsePlayerDelete message = new ServerMessageResponsePlayerDelete
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerDeleteSoft(msg.playername, msg.username))
			{
				message.text = systemText.playerDeleteSuccess;
				eventListener.onPlayerDelete.Invoke(msg.playername);
			}
			else
			{
				message.text = systemText.playerDeleteFailure;
				message.success = false;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerSwitchServer
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
       	void OnClientMessageRequestPlayerSwitchServer(NetworkConnection conn, ClientMessageRequestPlayerSwitchServer msg)
        {
        	
        	ServerMessageResponsePlayerSwitchServer message = new ServerMessageResponsePlayerSwitchServer
			{
				success = true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerSwitchServer(msg.username, msg.token))
			{
				message.text = systemText.playerSwitchServerSuccess;
				eventListener.onPlayerSwitchServer.Invoke(msg.username);
			}
			else
			{
				message.text = systemText.playerSwitchServerFailure;
				message.success = false;
			}
			
        	conn.Send(message);
        	
        }
        
        // ============================= COMMON ACTIONS ==================================
        
        // -------------------------------------------------------------------------------
		// LoginUser
		// @Server
		// -------------------------------------------------------------------------------
		protected void LoginUser(NetworkConnection conn, string name)
		{
			onlineUsers[conn] = name;
			state = NetworkState.Lobby;
			this.InvokeInstanceDevExtMethods(nameof(LoginUser));
		}
		
		// -------------------------------------------------------------------------------
		// LoginPlayer
		// @Server
		// -------------------------------------------------------------------------------
		protected void LoginPlayer(NetworkConnection conn, string _name)
		{
			if (!AccountLoggedIn(_name))
			{
				GameObject player = DatabaseManager.singleton.LoadDataPlayer(playerPrefab, _name);
				NetworkServer.AddPlayerForConnection(conn, player);
				onlinePlayers[player.name] = player;
				state = NetworkState.Game;
				
				this.InvokeInstanceDevExtMethods(nameof(LoginPlayer));
			}
			else
				ServerSendError(conn, systemText.userAlreadyOnline, true);
		}
		
		// -------------------------------------------------------------------------------
		// RegisterPlayer
		// @Server
		// -------------------------------------------------------------------------------
		protected void RegisterPlayer(string playername)
		{
			GameObject player = Instantiate(playerPrefab);
			player.name = playername;
			DatabaseManager.singleton.CreateDefaultDataPlayer(player);
			DatabaseManager.singleton.SaveDataPlayer(player, false);
			Destroy(player);
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================