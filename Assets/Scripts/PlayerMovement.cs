using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace LJ
{
    public class PlayerMovement : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [Tooltip("The maximum movement speed of the player.")]
        [SerializeField, Min(0f)] private float _maxSpeed;

        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private InputActionReference _movementReference;
        //private InputAction _movementAction;

        private GameManager _gameManager;
        private Camera _camera;

        private CharacterController _player;
        private Vector2 _movementInput;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _player = GetComponent<CharacterController>();
            _camera = Camera.main;
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            //_gameManager.GameActive += EnableMovement;
            //_playerInput.onActionTriggered += OnPlayerActionTrriggered;

            //_movementAction = _playerInput.actions.FindAction(_movementReference.action.id);
            //_movementAction.started += OnMovement;
            //_movementAction.performed += OnMovement;
            //_movementAction.canceled += OnMovement;
        }

        private void OnDisable()
        {
            // _gameManager.GameActive -= EnableMovement;
            //_movementAction.Disable();
            //_movementAction.started -= OnMovement;
            //_movementAction.performed -= OnMovement;
            //_movementAction.canceled -= OnMovement;

        }

        private void Update()
        {
            if(_camera == null)
            {
                _camera = Camera.main;
            }

            Vector3 movement = new Vector3(_movementInput.x, 0f, _movementInput.y);
            Quaternion camRotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
            movement = camRotation * movement;

            if(movement != Vector3.zero)
            {
                gameObject.transform.forward = -movement;
            }

            movement = _maxSpeed * Time.deltaTime * movement;

            // Apply gravity
            movement += Physics.gravity * Time.deltaTime;

            _player.Move(movement);

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        //private void EnableMovement(string actionMapName)
        //{
        //    Debug.Log("changed notification behavior");
        //    _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;

        //}

        public void OnMovement(InputValue inputValue)
        {
            _movementInput = inputValue.Get<Vector2>();
            //Debug.Log($"On movement: {_movementInput}");
        }
        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}