using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //[SerializeField] private Sprite[] _inventoryIcons = new Sprite[3];

        private Stack<SO_HerbData> _inventory = new Stack<SO_HerbData>();
        //private Stack<Sprite> _inventoryIcons = new Stack<Sprite>();

        private SO_PotionData _potion;

        // --- Properties -------------------------------------------------------------------------------------------------
        public Stack<SO_HerbData> Inventory {  get { return _inventory; } }
        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void AddHerb(SO_HerbData data)
        {
            if(_inventory.Count < 3)
            {
                _inventory.Push(data);
                //_inventoryIcons.Push(data.Icon);

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
                //_inventoryIcons.Pop();
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
            }
        }

        public void RemovePotion()
        {
            if(_potion != null)
            {
                _potion = null;
                UpdateInventoryUI();
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void UpdateInventoryUI()
        {
            //if(_inventory.Count > 0 && _potion == null)
            //{
            //    for(int i = 0; i < _inventory.Count; i++)
            //    {
            //        _inventorySlots[i].GetComponent<Image>().sprite = _inventoryIcons[i];
            //    }

            //}
            //else if (_inventory.Count == 0 && _potion != null)
            //{
            //    _inventorySlots[0].GetComponent<Image>().sprite = _potion.Icon;
            //}

            int index = 0;
            foreach(SO_HerbData item in _inventory)
            {
                _inventorySlots[index].sprite = item.Icon;
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