using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
{
    public class CauldronInteractable : MonoBehaviour, IInteractable
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private SO_RecipeCollection _recipes;
        [SerializeField, Min(0f)] private float _cookingDuration = 2f;
        [SerializeField] private Image[] _statusUI = new Image[3];

        private Stack<SO_HerbData> _herbs = new Stack<SO_HerbData>();
        private bool _isCooking;
        private SO_PotionData _activeRecipe;
        private GameObject _currentPotion;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool HasPotionReady => _currentPotion != null;

        // --- Unity Functions --------------------------------------------------------------------------------------------
        void Start()
        {
            UpdateStatusUI();
        }
        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public bool CanReceiveHerb()
        {
            return _herbs.Count < 3 && !HasPotionReady && !_isCooking;
        }

        public void AddHerb(SO_HerbData data)
        {
            if(CanReceiveHerb())
            {
                _herbs.Push(data);
                Debug.Log($"Added {data.Type} to Cauldron");
                UpdateStatusUI();

                if(_herbs.Count >= 3)
                {
                    Debug.Log("Cauldron Inventory full");
                    StartCoroutine(MakePotionRoutine());

                }
            }
        }


        public SO_PotionData PassPotion()
        {
            Destroy(_currentPotion);
            return _activeRecipe;
        }

        IEnumerator MakePotionRoutine()
        {
            UpdateStatusUI();
            _isCooking = true;
            _activeRecipe = _recipes.SearchRecipe(_herbs);

            Debug.Log("Initiate Cooking Procedures");
            yield return new WaitForSeconds(_cookingDuration);

            _currentPotion = Instantiate(_activeRecipe.PotionPrefab, transform, false);
            //_currentPotion.transform.localPosition = new Vector3(0f, 1f, 0f);
            Debug.Log($"Potion Prefab for {_activeRecipe.Type} Potion created");
            _herbs.Clear();
            UpdateStatusUI();

            _isCooking = false;
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void UpdateStatusUI()
        {
            int index = 0;

            foreach(SO_HerbData item in _herbs)
            {
                _statusUI[index].enabled = true;
                index++;
            }

            for(; index < _statusUI.Length; index++)
            {
                _statusUI[index].enabled = false;
            }

        }
        // --------------------------------------------------------------------------------------------
    }
}