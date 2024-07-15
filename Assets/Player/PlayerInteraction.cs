using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace LJ
{
    public class PlayerInteraction : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private InputActionReference _interactReference;

        private PlayerInventory _inventory;
        private Player _player;

        private IInteractable _nearestInteractable;
        private GameObject _interactObject;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        void Start()
        {
            _player = GetComponent<Player>();
            _inventory = GetComponent<PlayerInventory>();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        public void OnInteract(InputValue inputValue)
        {
            Debug.Log("OnInteract");

            //is looking in direction

            //if(Vector3.Angle(_interactObject.transform.position, transform.forward) <= 45)

            Vector3 direction = (_interactObject.transform.position - transform.position).normalized;
            direction.y = 0f;
            float lookAmount = Vector3.Dot(direction, transform.forward);
            //Debug.Log(lookAmount);

            if(lookAmount > 0)
            {
                if(_nearestInteractable != null)
                {
                    _player.Animator.SetBool("isWishing", true);

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

                        case TrashInteractable trash:
                            if(_inventory.HasHerbs)
                            {
                                trash.PlayTrashSound();
                                _inventory.RemoveHerb();
                            }
                            else if(_inventory.HasPotion)
                            {
                                trash.PlayTrashSound();
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
        }


        public void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.name}");
            if(other.TryGetComponent(out IInteractable interactable))
            {
                _nearestInteractable = interactable;
            }
            _interactObject = other.gameObject;
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