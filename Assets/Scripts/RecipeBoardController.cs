
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
{
    public class RecipeBoardController : MonoBehaviour, IInteractable
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private SO_RecipeCollection _recipeCollection;
        [SerializeField] private Renderer _recipeBoard;
        [Range(-1f, 1f)]
        [SerializeField] private int _increaseBy = 1;

        private int _index = 0;
        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _recipeBoard = _recipeBoard.GetComponent<Renderer>();
            _recipeBoard.material = _recipeCollection.Recipes[0].RecipeImage;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        public void ChangeRecipe() //only optimised for +1 or -1
        {
            _index += _increaseBy;
            if(_index >= _recipeCollection.Recipes.Length)
            {
                _index = 0;
            }
            if(_index < 0)
            {
                _index = _recipeCollection.Recipes.Length-1;
            }
            _recipeBoard.material = _recipeCollection.Recipes[_index].RecipeImage;

        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}