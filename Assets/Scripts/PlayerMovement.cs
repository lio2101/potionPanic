using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    public class PlayerMovement : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [Tooltip("The maximum movement speed of the player.")]
        [SerializeField, Min(0f)] private float _maxSpeed;

        private CharacterController _player;
        private Vector3 _playerVelocity;
        private Vector2 _movementInput;
        private Camera _camera;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _player = GetComponent<CharacterController>();
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector3 movement = new Vector3(_movementInput.x, 0f, _movementInput.y);
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

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void OnMovement(InputValue inputValue)
        {
            //Debug.Log($"On movement: {inputValue.Get<Vector2>()}");
            _movementInput = inputValue.Get<Vector2>();
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}