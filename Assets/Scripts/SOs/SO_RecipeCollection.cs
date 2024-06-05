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

            // Data with duplicate ingredients can never produce a potion as recipes always use 3 unique ingredients
            if(data.Distinct().Count() < INGREDIENT_AMOUNT)
                return _failedPotion;

            //PotionRecipe matchingRecipe = _recipes.FirstOrDefault(r => r.Ingredients.Union(data).Count() == INGREDIENT_AMOUNT);            

            PotionRecipe matchingRecipe = _recipes.FirstOrDefault(GetMatch);
            bool GetMatch(PotionRecipe recipe)
            {
                return recipe.Ingredients.Union(data).Count() == INGREDIENT_AMOUNT;
            }

            return matchingRecipe != null ? matchingRecipe.PotionData : _failedPotion;

            //int match = 0;

            //foreach(PotionRecipe recipe in _recipes) //go through all recipes
            //{
            //    for(int i = 0; i < recipe.Ingredients.Length; i++) //check for every herb in recipe
            //    {
            //        for(int j = 0; j < data.Length; j++) //check for every herb in cauldron
            //        {
            //            if(recipe.Ingredients[i].Type == data[j].Type && recipe.Ingredients[i].IsProcessed == false)
            //            {
            //                recipe.Ingredients[i].IsProcessed = true;
            //                match++;
            //            }

            //        }
            //        recipe.Ingredients[i].IsProcessed = false;
            //    }

            //    if(match == 3)
            //    {
            //        Debug.Log("Found Potion Match");
            //        return recipe.PotionData;
            //    }
            //    else 
            //    { 
            //        match = 0;
            //    }
            //}

            //Debug.Log("Potion Failed");
            //return _failedPotion;
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}