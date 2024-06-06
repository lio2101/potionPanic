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
        public enum Teams
        {
            none = 0,
            Team1,
            Team2
        }

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Image[] _inventorySlots = new Image[3];
        [SerializeField] private Teams _team;

        private Stack<SO_HerbData> _herbs = new Stack<SO_HerbData>();

        private SO_PotionData _potion;

        // --- Properties -------------------------------------------------------------------------------------------------
        public Stack<SO_HerbData> Herbs { get { return _herbs; } }
        public SO_PotionData Potion { get { return _potion; } }
        public Teams Team { get { return _team; } set { _team = value; } }

        public bool HasHerbs => _herbs.Count > 0;
        public bool CanRecieveHerb => _herbs.Count < 3 && _potion == null;
        public bool CanRecievePotion => _herbs.Count == 0 && _potion == null;

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void AddHerb(SO_HerbData data)
        {
            if(CanRecieveHerb)
            {
                _herbs.Push(data);

                UpdateInventoryUI();
                Debug.Log($"Added {data.Type} to inventory");

            }
            else { Debug.Log("Inventory full"); }
        }

        public void RemoveHerb()
        {
            if(HasHerbs)
            {
                SO_HerbData lastItem = _herbs.Pop();

                UpdateInventoryUI();
                Debug.Log($"Removed {lastItem.Type} from inventory");
            }
            else { Debug.Log("Inventory already empty"); }
        }

        public void AddPotion(SO_PotionData data)
        {
            if(CanRecievePotion)
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

            foreach(SO_HerbData item in _herbs)
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