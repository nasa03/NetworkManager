// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Wovencode;
using Wovencode.DebugManager;

namespace Wovencode
{

	// ===================================================================================
	// ServerInfoTemplateDictionary
	// ===================================================================================
	public partial class ServerInfoTemplateDictionary
	{
		
		public readonly ReadOnlyDictionary<int, ServerInfoTemplate> data;
		
		// -------------------------------------------------------------------------------
		public ServerInfoTemplateDictionary(string folderName="")
		{
			List<ServerInfoTemplate> templates = Resources.LoadAll<ServerInfoTemplate>(folderName).ToList();
			
			if (templates.HasDuplicates())
				debug.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
			else
				data = new ReadOnlyDictionary<int, ServerInfoTemplate>(templates.ToDictionary(x => x.hash, x => x));
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================
