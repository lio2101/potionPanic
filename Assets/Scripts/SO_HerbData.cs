using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LJ.ItemInteractable;

namespace LJ
{
	[CreateAssetMenu(fileName = "HerbData", menuName = "LJ/Herb Data", order = 100)]
	public class SO_HerbData : ScriptableObject
	{
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private HerbType _type;
		[SerializeField] private Sprite _icon;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void OnEnable()
		{
			
		}

		// --- Event callbacks --------------------------------------------------------------------------------------------

		// --- Public/Internal Methods ------------------------------------------------------------------------------------

		// --- Protected/Private Methods ----------------------------------------------------------------------------------
		
		// --------------------------------------------------------------------------------------------
	}	
}