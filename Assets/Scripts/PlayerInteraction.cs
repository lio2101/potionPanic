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
                                if(_inventory.CanRecieveHerb)
                                {
                                    _inventory.AddHerb(herb);
                                }
                                break;

                            case SO_PotionData potion:
                                if(_inventory.CanRecievePotion)
                                {
                                    _inventory.AddPotion(potion);
                                }
                                break;
                        }
                        break;

                    case CauldronInteractable cauldron:
                        if(_inventory.HasHerbs && cauldron.CanReceiveHerb())
                        {
                            cauldron.AddHerb(_inventory.Herbs.Peek());
                            _inventory.RemoveHerb();
                        }
                        else if(_inventory.CanRecievePotion && cauldron.HasPotionReady)
                        {
                            _inventory.AddPotion(cauldron.PassPotion());
                        }
                        break;

                    case TrashInteractable:
                        if(_inventory.HasHerbs)
                        {
                            _inventory.RemoveHerb();
                        }
                        else if(_inventory.HasPotion)
                        {
                            _inventory.RemovePotion();
                        }
                        break;

                    case Customer customer:
                        if(_inventory.HasPotion)
                        {
                            if(customer.TryGivePotion(_inventory.Potion))
                            {
                                _inventory.RemovePotion();
                            }
                        }
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