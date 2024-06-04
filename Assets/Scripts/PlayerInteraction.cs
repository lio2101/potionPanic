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
        //private CauldronInteractable _cauldron;
        //private TrashInteractable _trash;
        //private HerbInteractable _objectInRange;

        //private bool canInteract;

        // --- Properties -------------------------------------------------------------------------------------------------
        //private bool CanInteract => _nearestInteractable != null || _cauldron != null || _trash != null;

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
                        cauldron.AddHerb(_inventory.Inventory.Peek());
                        break;

                    case TrashInteractable:
                        _inventory.RemoveHerb();
                        _inventory.RemovePotion();
                        break;
                }


                //if(CanInteract)
                //{
                    //switch(_objectInRange.Item)
                    //{
                    //    case SO_HerbData herb:
                    //        _inventory.AddHerb(herb);
                    //        break;

                    //    case SO_PotionData potion:
                    //        _inventory.AddPotion(potion);
                    //        break;
                    //}

                    //    if(_objectInRange != null && _objectInRange.Item is SO_HerbData herb)
                    //    {
                    //        _inventory.AddHerb(herb);
                    //    }
                    //    else if(_objectInRange != null && _objectInRange.Item is SO_PotionData potion)
                    //    {
                    //        _inventory.AddPotion(potion);
                    //    }
                    //    else if(_cauldron != null)
                    //    {
                    //        _cauldron.MakePotion();
                    //    }
                    //    else if (_trash != null)
                    //    {
                    //        _inventory.RemoveHerb();
                    //        _inventory.RemovePotion();
                    //    }

                //}

            }
        }


        public void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.name}");
            //canInteract = true;
            if(other.TryGetComponent(out IInteractable interactable))
            {
                _nearestInteractable = interactable;
            }
            //else if(other.TryGetComponent(out CauldronInteractable cauldron))
            //{
            //    _cauldron = cauldron;
            //}
            //else if(other.TryGetComponent(out TrashInteractable trsh))
            //{
            //    _trash = trsh;
            //}
        }

        public void OnTriggerExit(Collider other)
        {
            if(_nearestInteractable != null && other.TryGetComponent(out IInteractable interactable) && interactable == _nearestInteractable)
            {
                _nearestInteractable = null;
                //canInteract = false;
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------        

        // --------------------------------------------------------------------------------------------
    }
}