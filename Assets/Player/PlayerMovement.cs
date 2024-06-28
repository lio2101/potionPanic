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
        [SerializeField] private float _rotationSpeed = 20f;

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

            Vector3 playerDirection = transform.forward;
            Vector3 inputDirection = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;
            Quaternion camRotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
            inputDirection = camRotation * inputDirection;

            if(inputDirection != Vector3.zero)
            {

                Debug.Log($"player: {playerDirection} input: {inputDirection}");

                float step = _rotationSpeed * Time.deltaTime;

                //transform.forward = Vector3.RotateTowards(playerDirection, inputDirection, step, 0.0f);

                Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

                inputDirection = _maxSpeed * Time.deltaTime * inputDirection;

                // Apply gravity
                inputDirection += Physics.gravity * Time.deltaTime;

                _player.Move(inputDirection);

            }

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        public void OnMovement(InputValue inputValue)
        {
            //StartCoroutine(RotatePlayerRoutine(inputValue));

            _movementInput = inputValue.Get<Vector2>();

            Debug.Log(_movementInput.ToString());
            if(_movementInput.x != 0 || _movementInput.y != 0)
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
        //private IEnumerator RotatePlayerRoutine(InputValue inputValue)
        //{
        //    Vector2 playerDirection = transform.forward;
        //    Vector2 inputDirection = inputValue.Get<Vector2>();
        //    float step = _rotationSpeed * Time.deltaTime;

        //    while(playerDirection != inputDirection)
        //    {
        //        Vector3 newDirection = Vector3.RotateTowards(playerDirection, inputDirection, step, 0.0f);
        //        yield return new WaitForEndOfFrame();

        //        playerDirection = newDirection;
        //    }
        //}
        // --------------------------------------------------------------------------------------------
    }
}