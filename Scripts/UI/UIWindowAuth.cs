﻿
using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Wovencode.UI
{

	// ===================================================================================
	// UIWindowAuth
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowAuth : UIRoot
	{
		
		[Header("Settings")]
		public bool rememberServer;
		
		[Header("Dropdown")]
		public Dropdown serverDropdown;
		
		[Header("Buttons")]
		public Button connectButton;
		public Text connectButtonText;
		
		[Header("System Texts")]
		public UIWindowAuth_Lang systemTexts;
		
		public static UIWindowAuth singleton;
		
		protected int connectTimer = -1;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// Show
		// -------------------------------------------------------------------------------
		public override void Show()
		{
			base.Show();
			
			connectTimer = -1;
			
			serverDropdown.options.Clear();
			
			foreach (ServerInfoTemplate template in ProjectConfigTemplate.singleton.serverList)
			{
				if (template.visible)
					serverDropdown.options.Add(new Dropdown.OptionData(template.title));
			}
			
			if (rememberServer && PlayerPrefs.HasKey(Constants.PlayerPrefsLastServer))
			{
				string lastServer = PlayerPrefs.GetString(Constants.PlayerPrefsLastServer, "");
				
				for (int i = 0; i < ProjectConfigTemplate.singleton.serverList.Length; i++)
					if (ProjectConfigTemplate.singleton.serverList[i].visible && ProjectConfigTemplate.singleton.serverList[i].title == lastServer)
						serverDropdown.value = i;
			}
			
			networkManager.networkAddress = ProjectConfigTemplate.singleton.serverList[serverDropdown.value].ip;
			
			if (networkAuthenticator.connectTimeout > 0)
				Invoke(nameof(Timeout), networkAuthenticator.connectTimeout);
			
		}
		
		// -------------------------------------------------------------------------------
		// Hide
		// -------------------------------------------------------------------------------
		public override void Hide()
		{
			
			CancelInvoke();
			
			if (rememberServer)
				PlayerPrefs.SetString(Constants.PlayerPrefsLastServer, serverDropdown.captionText.text);
		
			base.Hide();
			
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (connectTimer == -1)
				connectTimer = networkAuthenticator.connectDelay;
				
			if (networkManager.IsConnecting())
			{
			
				if (connectButtonText)
				{
					connectTimer--;
					connectButtonText.text = systemTexts.clientConnect + " (in " + connectTimer.ToString() + "s)";
				}
			
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// Timeout
		// -------------------------------------------------------------------------------
		protected void Timeout()
		{
			UIPopupConfirm.singleton.Init(systemTexts.serverOffline, "", OnClickQuit);
		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// onClickQuit
		// -------------------------------------------------------------------------------
		protected void OnClickQuit()
		{
			networkManager.Quit();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConnect
		// -------------------------------------------------------------------------------
		public void OnClickConnect()
		{
			CancelInvoke();
			networkAuthenticator.ClientAuthenticate();
		}
		
		// -------------------------------------------------------------------------------
		// OnDropdownChange
		// -------------------------------------------------------------------------------
		public void OnDropdownChange()
		{
			if (rememberServer)
				PlayerPrefs.SetString(Constants.PlayerPrefsLastServer, serverDropdown.captionText.text);
			
            networkManager.networkAddress = ProjectConfigTemplate.singleton.serverList[serverDropdown.value].ip;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================