using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LJ.ItemInteractable;
using static LJ.SO_HerbData;

namespace LJ
{
    [CreateAssetMenu(fileName = "PotionData", menuName = "LJ/PotionData", order = 100)]
    public class SO_PotionData : ScriptableObject
    {
        // --- Enums ------------------------------------------------------------------------------------------------------
        public enum PotionType
        {
            None = 0,
            Health,
            Poison,
            Acid,
            Failed
        }
        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private PotionType _type;
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _potionPrefab;
        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        public PotionType Type { get { return _type; } }
        public Sprite Icon { get { return _icon; } }

        public GameObject PotionPrefab { get { return _potionPrefab; } }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}