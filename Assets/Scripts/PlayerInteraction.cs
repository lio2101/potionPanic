using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    public class PlayerInteraction : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        private PlayerInventory _inventory;
        private IInteractable _nearestInteractable;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        void Start()
        {
            _inventory = GetComponent<PlayerInventory>();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void OnInteract(InputValue inputValue)
        {

            if(inputValue.isPressed && _nearestInteractable != null)
            {
                switch(_nearestInteractable)
                {
                    case ItemInteractable item:
                        switch(item.Item)
                        {
                            case SO_HerbData herb:
                                _inventory.AddHerb(herb);
                                break;

                            case SO_PotionData potion:
                                _inventory.AddPotion(potion);
                                break;
                        }
                        break;

                    case CauldronInteractable cauldron:
                        Debug.Log($"Inventory empty: {_inventory.Inventory == null} Cauldron has Potion: {cauldron.HasPotionReady}");
                        if(_inventory.Inventory.Count>0 && !cauldron.HasPotionReady && !cauldron.IsCooking)
                        {
                            cauldron.AddHerb(_inventory.Inventory.Peek());
                            _inventory.RemoveHerb();
                        }
                        else if(_inventory.Inventory.Count==0 && _inventory.Potion == null && cauldron.HasPotionReady)
                        {
                            _inventory.AddPotion(cauldron.PassPotion());
                        }
                        break;


                    case TrashInteractable:
                        _inventory.RemoveHerb();
                        _inventory.RemovePotion();
                        break;
                }

            }
        }


        public void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.name}");
            if(other.TryGetComponent(out IInteractable interactable))
            {
                _nearestInteractable = interactable;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if(_nearestInteractable != null && other.TryGetComponent(out IInteractable interactable) && interactable == _nearestInteractable)
            {
                _nearestInteractable = null;
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------        

        // --------------------------------------------------------------------------------------------
    }
}