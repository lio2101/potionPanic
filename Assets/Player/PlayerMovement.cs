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

        private Camera _camera;

        private CharacterController _player;
        private Vector2 _movementInput;
        private Animator _animator;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _player = GetComponent<CharacterController>();
            _camera = Camera.main;
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if(_camera == null)
            {
                _camera = Camera.main;
            }

            Vector3 movement = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;
            Quaternion camRotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
            movement = camRotation * movement;

            if(movement != Vector3.zero)
            {
                gameObject.transform.forward = movement;
            }

            movement = _maxSpeed * Time.deltaTime * movement;

            // Apply gravity
            movement += Physics.gravity * Time.deltaTime;

            _player.Move(movement);

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        public void OnMovement(InputValue inputValue)
        {
            _movementInput = inputValue.Get<Vector2>();
            if(_movementInput != null)
            {
                Debug.Log("isWalking");
                _animator.SetBool("isWalking", true);

            }
            else
            {
                _animator.SetBool("isWalking", false);
            }
        }
        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}