// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using System;
using UnityEngine;
using Wovencode;

namespace Wovencode
{
	
	// ===================================================================================
	// GameRulesTemplate
	// ===================================================================================
	public partial class GameRulesTemplate
	{
		[Tooltip("How many characters can one user create on one account?")]
		public int maxPlayersPerUser = 4;
		[Tooltip("How many accounts can be created on one device (helps fight twinks)?")]
		public int maxUsersPerDevice = 4;
		[Tooltip("How many accounts can be created on one email (helps fight twinks)?")]
		public int maxUsersPerEmail = 4;
		
	}

}

// =======================================================================================
