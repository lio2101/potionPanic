using System;
using System.Collections;
using System.Collections.Generic;
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

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool HasPotionReady {  get { return hasPotionReady; } }
        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void AddHerb(SO_HerbData data)
        {
            if(_cauldronInventory.Count < 3 && !hasPotionReady)
            {
                _cauldronInventory.Push(data);
            }
            else if (_cauldronInventory.Count >= 3)
            {
                StartCoroutine(MakePotion());
                
                _cauldronInventory.Clear();
            }
        }

        IEnumerator MakePotion()
        {
            //calculate potion
            //foreach(PotionRecipe in  _recipes)
            //{

            //}

            yield return new WaitForSeconds(2);
            

        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}