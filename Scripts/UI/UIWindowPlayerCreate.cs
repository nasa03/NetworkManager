﻿// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Wovencode.UI
{

	// ===================================================================================
	// UIWindowPlayerCreate
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowPlayerCreate : UIRoot
	{
		
		[Header("Windows")]
		public UIWindowPlayerSelect selectWindow;
		
		[Header("Prefab")]
		public UISelectPlayerSlot slotPrefab;
		public UIButtonGroup buttonGroup;
		
		[Header("Content")]
		public Transform contentViewport;
		
		[Header("Input Fields")]
		public InputField playernameInput;
		
		[Header("Buttons")]
		public Button createButton;
		public Button backButton;
		
		protected int index = -1;
		
		// -------------------------------------------------------------------------------
		// Show
		// -------------------------------------------------------------------------------
		public override void Show()
		{
			playernameInput.text = "";
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
		
			this.InvokeInstanceDevExtMethods(nameof(ThrottledUpdate));
			
			// -- Available Players
			UpdatePlayerIndex();
			
			// -- Buttons
			createButton.interactable = (index != -1 && !String.IsNullOrWhiteSpace(playernameInput.text));
			createButton.onClick.SetListener(() => { OnClickCreate(); });
			
			backButton.onClick.SetListener(() => { OnClickBack(); });
		
		}
		
		// -------------------------------------------------------------------------------
		// UpdatePlayerIndex
		// -------------------------------------------------------------------------------
		protected void UpdatePlayerIndex()
		{
			
			foreach (UIButton button in buttonGroup.buttons)
			{
				int _index = button.GetComponent<UISelectPlayerSlot>().Index;
				if (_index != -1)
				{
					index = _index;
					return;
				}
			}
			
			index = -1;
			
		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickCreate
		// -------------------------------------------------------------------------------
		public void OnClickCreate()
		{	
			string prefabName = networkManager.playerPrefabs[index].name;
			networkManager.TryRegisterPlayer(playernameInput.text, prefabName);
			//Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickBack
		// -------------------------------------------------------------------------------
		public void OnClickBack()
		{	
			selectWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================