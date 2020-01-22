// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
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
    	
    	// ======================= PUBLIC METHODS - USER =================================
    	
        // -------------------------------------------------------------------------------
        // RequestUserLogin
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestUserLogin(NetworkConnection conn, string name, string password)
		{
			if (!base.RequestUserLogin(conn, name, password)) return false;
			
			ClientMessageRequestUserLogin message = new ClientMessageRequestUserLogin
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserRegister
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestUserRegister(NetworkConnection conn, string name, string password, string usermail)
		{
			if (!base.RequestUserRegister(conn, name, password, usermail)) return false;
			
			ClientMessageRequestUserRegister message = new ClientMessageRequestUserRegister
			{
				username 	= name,
				password 	= GenerateHash(name, password),
				email 		= usermail,
				deviceid	= Tools.GetDeviceId
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserDelete
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestUserDelete(NetworkConnection conn, string name, string password, int action=1)
		{
			if (!base.RequestUserDelete(conn, name, password)) return false;
			
			ClientMessageRequestUserDelete message = new ClientMessageRequestUserDelete
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserChangePassword
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestUserChangePassword(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			if (!base.RequestUserChangePassword(conn, name, oldpassword, newpassword)) return false;
			
			ClientMessageRequestUserChangePassword message = new ClientMessageRequestUserChangePassword
			{
				username = name,
				oldPassword = GenerateHash(name, oldpassword),
				newPassword = GenerateHash(name, newpassword)
			};
			
			// reset player prefs on password change
			PlayerPrefs.SetString(Constants.PlayerPrefsPassword, "");
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserConfirm
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestUserConfirm(NetworkConnection conn, string name, string password, int action=1)
		{
			if (!base.RequestUserConfirm(conn, name, password)) return false;
			
			ClientMessageRequestUserConfirm message = new ClientMessageRequestUserConfirm
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// ======================= PUBLIC METHODS - PLAYER ================================
		
        // -------------------------------------------------------------------------------
        // RequestPlayerLogin
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestPlayerLogin(NetworkConnection conn, string name, string username)
		{
			if (!base.RequestPlayerLogin(conn, name, username)) return false;
			
			ClientMessageRequestPlayerLogin message = new ClientMessageRequestPlayerLogin
			{
				playername = name,
				username = userName
			};
			
			ClientScene.Ready(conn);
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestPlayerRegister
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestPlayerRegister(NetworkConnection conn, string name, string username)
		{
			if (!base.RequestPlayerRegister(conn, name, username)) return false;
			
			ClientMessageRequestPlayerRegister message = new ClientMessageRequestPlayerRegister
			{
				playername = name,
				username = userName
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestPlayerDelete
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestPlayerDelete(NetworkConnection conn, string name, string username, int action=1)
		{
			if (!base.RequestPlayerDelete(conn, name, username)) return false;
			
			ClientMessageRequestPlayerDelete message = new ClientMessageRequestPlayerDelete
			{
				playername = name,
				username = userName
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestPlayerSwitchServer
        // @Client
		// -------------------------------------------------------------------------------
		protected override bool RequestPlayerSwitchServer(NetworkConnection conn, string name, int _token=0)
		{
			
			_token = Tools.GenerateToken();
			
			if (!base.RequestPlayerSwitchServer(conn, name, _token)) return false;
			
			ClientMessageRequestPlayerSwitchServer message = new ClientMessageRequestPlayerSwitchServer
			{
				username = name,
				token = _token
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================