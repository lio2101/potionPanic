using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
{

    public class PlayerInventory : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Image[] _inventorySlots = new Image[3];
        [SerializeField] private Image _inventoryBubble;
        [SerializeField] private Image _potionSlot;

        [SerializeField] private AudioClip _herbPickup;
        [SerializeField] private AudioClip _potionPickup;
        private AudioSource _audioSource;

        private Stack<SO_HerbData> _herbs = new Stack<SO_HerbData>();

        private SO_PotionData _potion;

        // --- Properties -------------------------------------------------------------------------------------------------
        public Stack<SO_HerbData> Herbs { get { return _herbs; } }
        public SO_PotionData Potion { get { return _potion; } }

        public bool HasHerbs => _herbs.Count > 0;
        public bool HasPotion => _potion != null;
        public bool CanReceiveHerb => _herbs.Count < 3 && _potion == null;
        public bool IsInventoryEmpty => _herbs.Count == 0 && _potion == null;
        public bool CanReceivePotion => IsInventoryEmpty;

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            ClearInventory();
            UpdateInventoryUI();
            _audioSource = GetComponent<AudioSource>();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void AddHerb(SO_HerbData data)
        {
            _audioSource.clip = _herbPickup;
            _audioSource.Play();

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
            _audioSource.clip = _potionPickup;
            _audioSource.Play();

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
            _inventoryBubble.enabled = !IsInventoryEmpty;
            if(_potion != null)
            {
                _potionSlot.enabled = true;
                _potionSlot.sprite = _potion.Icon;
            }
            else
            {
                _potionSlot.enabled = false;

                foreach(SO_HerbData item in _herbs.Reverse())
                {
                    _inventorySlots[index].enabled = true;
                    _inventorySlots[index].sprite = item.Icon;
                    index++;
                }
            }

            // Disable free potion slots
            for(; index < _inventorySlots.Length; index++)
            {
                _inventorySlots[index].enabled = false;
            }
            
        }

        private void ClearInventory()
        {
            _herbs.Clear();
            _potion = null;
            UpdateInventoryUI() ;
        }

        // --------------------------------------------------------------------------------------------
    }
}