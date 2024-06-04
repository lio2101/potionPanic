using System;
using System.Collections;
using System.Collections.Generic;
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

        // --- Properties -------------------------------------------------------------------------------------------------


        // --- Unity Functions --------------------------------------------------------------------------------------------        

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public PotionRecipe SearchRecipe(SO_HerbData[] data)
        {
            int match = 0;
            foreach(PotionRecipe recipe in _recipes)
            {
                for(int i = 0; i < data.Length; i++)
                {
                    for(int j = 0; j < recipe.Ingredients.Length; j++)
                    {
                        if(data[i].Type == recipe.Ingredients[j].Type)
                        {
                            match++;
                        }

                    }
                }

                if(match == 3)
                {
                    return recipe;
                }
                else { match = 0; }
            }
            return _recipes[_recipes.Length-1];
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}