using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LJ
{
	public class HerbItem
	{
		// --- Enums ------------------------------------------------------------------------------------------------------		
		

		// --- Fields -----------------------------------------------------------------------------------------------------

		// --- Properties -------------------------------------------------------------------------------------------------
		private string _herbType;
		private Sprite _sprite;

		// --- Constructors -----------------------------------------------------------------------------------------------
		public HerbItem(string type, Sprite sprite)
		{
			_herbType = type;
			_sprite = sprite;
		}

		// --- Event callbacks --------------------------------------------------------------------------------------------

		// --- Public/Internal Methods ------------------------------------------------------------------------------------

		// --- Protected/Private Methods ----------------------------------------------------------------------------------
		
		// --------------------------------------------------------------------------------------------
	}
}