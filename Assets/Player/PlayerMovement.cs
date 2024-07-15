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
        [SerializeField] private float _rotationSmoothTime = .2f;
        private float _rotationVelocity;

        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private InputActionReference _movementReference;

        private Camera _camera;
        private Player _player;
        private CharacterController _cc;
        private Vector2 _movementInput;
        private Vector3 _lastInputDirection;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _player = GetComponent<Player>();
            _cc = GetComponent<CharacterController>();
            _camera = Camera.main;
        }

        private void OnDisable()
        {
            _lastInputDirection = Vector3.zero;
        }

        private void Update()
        {
            if(_camera == null)
            {
                _camera = Camera.main;
            }

            Vector3 inputDirection = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

            Quaternion camRotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
            inputDirection = camRotation * inputDirection;

            // Move
            if(inputDirection != Vector3.zero)
            {
                _lastInputDirection = inputDirection;
                Vector3 movement = _maxSpeed * Time.deltaTime * inputDirection;
                // Apply gravity
                movement += Physics.gravity * Time.deltaTime;
                _cc.Move(movement);
            }

            // Rotate
            if(_lastInputDirection != Vector3.zero)
            {
                float currentAngle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
                float targetAngle = Vector3.SignedAngle(Vector3.forward, _lastInputDirection, Vector3.up);
                float newAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref _rotationVelocity, _rotationSmoothTime);
                transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.up);
            }
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        public void OnMovement(InputValue inputValue)
        {
            _movementInput = inputValue.Get<Vector2>();

            if(_player.Animator != null)
            {
                if(_movementInput.x != 0 || _movementInput.y != 0)
                {
                    Debug.Log("isWalking");
                    _player.Animator.SetBool("isWalking", true);
                }
                else
                {
                    _player.Animator.SetBool("isWalking", false);
                }
            }

        }
        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}