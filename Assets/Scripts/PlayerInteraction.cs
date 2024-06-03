using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    public class PlayerInteraction : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private ItemInteractable _itemInRange;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------


        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void OnInteract(InputValue inputValue)
        {
            
            if(inputValue.isPressed)
            {
                if(_itemInRange != null)
                {
                    // Do herb stuff
                    Debug.Log($"Interacting with {_itemInRange.name}");
                }
            }
        }


        public void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.name}");
            if(other.TryGetComponent(out ItemInteractable herb))
            {
                _itemInRange = herb;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if(_itemInRange != null && other.TryGetComponent(out ItemInteractable item) && item == _itemInRange)
            {
                _itemInRange = null;
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------        

        // --------------------------------------------------------------------------------------------
    }
}