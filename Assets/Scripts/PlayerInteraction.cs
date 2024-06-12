using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    public class PlayerInteraction : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private InputActionReference _interactReference;

        private GameManager _gm;
        private PlayerInventory _inventory;

        private InputAction _interactAction;

        private IInteractable _nearestInteractable;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        void Start()
        {
            _gm = GameManager.Instance;
            _inventory = GetComponent<PlayerInventory>();
        }

        private void OnEnable()
        {
            _interactAction = _playerInput.actions.FindAction(_interactReference.action.id);
            //_interactAction.performed += OnInteract;
        }

        private void OnDisable()
        {
            //_interactAction.performed -= OnInteract;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        
        public void OnInteract(InputValue inputValue)
        {
            Debug.Log("OnInteract");
            if(_nearestInteractable != null)
            {
                switch(_nearestInteractable)
                {
                    case ItemInteractable item:
                        switch(item.Item)
                        {
                            case SO_HerbData herb:
                                if(_inventory.CanReceiveHerb)
                                {
                                    _inventory.AddHerb(herb);
                                }
                                break;

                            case SO_PotionData potion:
                                if(_inventory.CanReceivePotion)
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
                        else if(_inventory.CanReceivePotion && cauldron.HasPotionReady)
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

                    case RecipeBoardController recipeBoard:

                        Debug.Log("recipeboard in reach");
                        recipeBoard.ChangeRecipe();

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