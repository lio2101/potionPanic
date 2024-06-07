using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace LJ
{

    public class PlayerInventory : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Image[] _inventorySlots = new Image[3];
        [SerializeField] private Image _potionSlot;

        private Stack<SO_HerbData> _herbs = new Stack<SO_HerbData>();
        private Canvas _canvas;

        private SO_PotionData _potion;

        // --- Properties -------------------------------------------------------------------------------------------------
        public Stack<SO_HerbData> Herbs { get { return _herbs; } }
        public SO_PotionData Potion { get { return _potion; } }

        public bool HasHerbs => _herbs.Count > 0;
        public bool HasPotion => _potion != null;
        public bool CanRecieveHerb => _herbs.Count < 3 && _potion == null;
        public bool CanRecievePotion => _herbs.Count == 0 && _potion == null;
        public bool IsInventoryEmpty => _herbs.Count == 0 && _potion == null;

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _canvas = GetComponentInChildren<Canvas>();
            _canvas.enabled = false;
        }
        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void AddHerb(SO_HerbData data)
        {
            _herbs.Push(data);
            UpdateInventoryUI();
            Debug.Log($"Added {data.Type} to inventory");
        }

        public void RemoveHerb()
        {
            SO_HerbData lastItem = _herbs.Pop();
            UpdateInventoryUI();
            Debug.Log($"Removed {lastItem.Type} from inventory");
        }

        public void AddPotion(SO_PotionData data)
        {
            _potion = data;
            UpdateInventoryUI();
            Debug.Log($"Added {data.Type} Potion to inventory");
        }

        public void RemovePotion()
        {
            SO_PotionData lastPotion = _potion;
            _potion = null;
            UpdateInventoryUI();
            Debug.Log($"Removed {lastPotion.Type} Potion from inventory");
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void UpdateInventoryUI()
        {
            int index = 0;
            _canvas.enabled = true;
            foreach(SO_HerbData item in _herbs)
            {
                _inventorySlots[index].enabled = true;
                _inventorySlots[index].sprite = item.Icon;
                index++;
            }
            if(_potion != null)
            {
                _potionSlot.enabled = true;
                _potionSlot.sprite = _potion.Icon;
                index++;
            }
            else { _potionSlot.enabled = false; }

            for(; index < _inventorySlots.Length; index++)
            {
                _inventorySlots[index].enabled = false;
            }

        }

        // --------------------------------------------------------------------------------------------
    }
}