using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LJ
{
    public class CauldronInteractable : MonoBehaviour, IInteractable
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private SO_RecipeCollection _recipes;

        private Stack<SO_HerbData> _cauldronInventory = new Stack<SO_HerbData>();
        private bool hasPotionReady;
        private bool isCooking;
        private SO_PotionData _newPotion;
        private GameObject _newPotionPrefab;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool HasPotionReady { get { return hasPotionReady; } }
        public bool IsCooking { get {  return isCooking; } }
        // --- Unity Functions --------------------------------------------------------------------------------------------

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void AddHerb(SO_HerbData data)
        {
            if(_cauldronInventory.Count < 3 && !hasPotionReady && !isCooking)
            {
                _cauldronInventory.Push(data);
                Debug.Log($"Added {data.Type} to Cauldron");

                if(_cauldronInventory.Count >= 3)
                {
                    Debug.Log("Cauldron Inventory full");
                    isCooking = true;
                    StartCoroutine(MakePotion());

                    _cauldronInventory.Clear();
                }
            }
        }


        public SO_PotionData PassPotion()
        {
            Destroy(_newPotionPrefab);
            hasPotionReady = false;
            return _newPotion;
        }

        IEnumerator MakePotion()
        {
            _newPotion = _recipes.SearchRecipe(_cauldronInventory);

            Debug.Log("Initiate Cooking Procedures");
            yield return new WaitForSeconds(2);

            _newPotionPrefab = Instantiate(_newPotion.PotionPrefab);
            _newPotionPrefab.transform.parent = this.transform;
            _newPotionPrefab.transform.localPosition = new Vector3(0f, 1f, 0f);
            Debug.Log($"Potion Prefab for {_newPotion.Type} Potion created");

            isCooking = false;
            hasPotionReady = true;

        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}