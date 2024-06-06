using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LJ.SO_HerbData;

namespace LJ
{
    [CreateAssetMenu(fileName = "RecipeCollection", menuName = "LJ/Recipe Collection", order = 100)]
    public class SO_RecipeCollection : ScriptableObject
    {
        [System.Serializable]
        public class PotionRecipe
        {
            [SerializeField] private SO_HerbData[] _ingredients;
            [SerializeField] private SO_PotionData _potion;
            public SO_HerbData[] Ingredients { get { return _ingredients; } }
            public SO_PotionData PotionData { get { return _potion; } }
        }

        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private PotionRecipe[] _recipes;
        [SerializeField] private SO_PotionData _failedPotion;

        const int INGREDIENT_AMOUNT = 3;


        // --- Properties -------------------------------------------------------------------------------------------------
        public PotionRecipe[] Recipes { get {  return _recipes; } }

        // --- Unity Functions --------------------------------------------------------------------------------------------        

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public SO_PotionData SearchRecipe(IEnumerable<SO_HerbData> data)
        {
            if(data == null || data.Count() != INGREDIENT_AMOUNT)
            {
                Debug.LogWarning($"A recipe always requires {INGREDIENT_AMOUNT} ingredients!");
                return _failedPotion;
            }

            if(data.Distinct().Count() < INGREDIENT_AMOUNT)
                return _failedPotion;

            PotionRecipe matchingRecipe = _recipes.FirstOrDefault(GetMatch);
            bool GetMatch(PotionRecipe recipe)
            {
                return recipe.Ingredients.Union(data).Count() == INGREDIENT_AMOUNT;
            }

            return matchingRecipe != null ? matchingRecipe.PotionData : _failedPotion;
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}