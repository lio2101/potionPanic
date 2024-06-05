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

        private Stack<SO_HerbData> _inventory = new Stack<SO_HerbData>();

        private SO_PotionData _potion;

        // --- Properties -------------------------------------------------------------------------------------------------
        public Stack<SO_HerbData> Inventory { get { return _inventory; } }
        public SO_PotionData Potion { get { return _potion; } }
        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void AddHerb(SO_HerbData data)
        {
            if(_inventory.Count < 3 && _potion == null)
            {
                _inventory.Push(data);

                UpdateInventoryUI();
                Debug.Log($"Added {data.Type} to inventory");

            }
            else { Debug.Log("Inventory full"); }
        }

        public void RemoveHerb()
        {
            if(_inventory.Count > 0)
            {
                SO_HerbData lastItem = _inventory.Peek();
                _inventory.Pop();

                UpdateInventoryUI();
                Debug.Log($"Removed {lastItem.Type} from inventory");
            }
            else { Debug.Log("Inventory already empty"); }
        }

        public void AddPotion(SO_PotionData data)
        {
            if(_inventory.Count == 0 && _potion == null)
            {
                _potion = data;
                UpdateInventoryUI();
                Debug.Log($"Added {data.Type} Potion to inventory");
            }
        }

        public void RemovePotion()
        {
            if(_potion != null)
            {
                SO_PotionData lastPotion = _potion;
                _potion = null;
                UpdateInventoryUI();
                Debug.Log($"Removed {lastPotion.Type} Potion from inventory");
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void UpdateInventoryUI()
        {
            int index = 0;

            foreach(SO_HerbData item in _inventory)
            {
                _inventorySlots[index].sprite = item.Icon;
                index++;
            }
            if(_potion != null)
            {
                _inventorySlots[index].sprite = _potion.Icon;
                index++;
            }

            for(; index < _inventorySlots.Length; index++)
            {
                _inventorySlots[index].sprite = null;
            }

        }

        // --------------------------------------------------------------------------------------------
    }
}