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
        public enum HerbType
        {
            None = 0,
            Pilz,
            Alraune,
			Schnecken,
			Beeren,
			Blaetter
        }

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private HerbType _type;
		[SerializeField] private Sprite _icon;
        [SerializeField] private bool isProcessed;

        // --- Properties -------------------------------------------------------------------------------------------------
        public HerbType Type { get { return _type; } }
		public Sprite Icon { get { return _icon; } }
        public bool IsProcessed { get { return isProcessed; } set {  isProcessed = value; } }

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