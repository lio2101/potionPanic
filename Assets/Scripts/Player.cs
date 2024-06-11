using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    public class Player : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------		

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private GameObject[] _characterModels;
        [SerializeField] private InputActionReference _switchTeamReference;
        [SerializeField] private InputActionReference _changeAppearanceReference;
        [SerializeField] private InputActionReference _readyReference;

        private bool _isReady = false;
        private GameManager _gm;


        [SerializeField] private PlayerInput _playerInput;
        private int _modelIndex = 0;
        private int _teamIndex = 0;

        //Delegates
        public delegate void ReadyStatusChangedEvent(Player player, bool isReady);
        public event ReadyStatusChangedEvent ReadyStatusChanged;

        public delegate void TeamSwitchedEvent(Player player, int teamIndex);
        public event TeamSwitchedEvent TeamSwitched;

        private InputAction _switchTeamAction;
        private InputAction _changeAppearanceAction;
        private InputAction _readyAction;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool IsReady => _isReady;

        // --- Unity Functions -----------------------------------------------------------------------------------------------

        private void OnEnable()
        {

            _switchTeamAction = _playerInput.actions.FindAction(_switchTeamReference.action.id);

            _changeAppearanceAction = _playerInput.actions.FindAction(_changeAppearanceReference.action.id);

            _readyAction = _playerInput.actions.FindAction(_readyReference.action.id);

            _switchTeamAction.performed += OnSwitchTeam;
            _changeAppearanceAction.performed += OnChangeAppearance;
            _readyAction.performed += OnReady;
        }

        private void Start()
        {
            _gm = GameManager.Instance;
            _gm.CharacterSelectionActive += ChangeActionMap;
            _gm.GameActive += ChangeActionMap;

            //TeamSwitched.Invoke(this, _teamIndex);
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void OnSwitchTeam(InputAction.CallbackContext context)
        {
            float value = context.ReadValue<float>();
            Debug.Log(value);

            TeamSwitched.Invoke(this, _teamIndex);

            if(_teamIndex == 0) { _teamIndex = 1; }
            else if(_teamIndex == 1) { _teamIndex = 0; }
        }

        public void OnChangeAppearance(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();

            input = input > 0 ? Mathf.CeilToInt(input) : Mathf.FloorToInt(input);
            Debug.Log(input);
            _modelIndex += (int)input;

            if(_modelIndex >= _characterModels.Length)
            {
                _modelIndex = 0;
            }
            if(_modelIndex < 0)
            {
                _modelIndex = _characterModels.Length - 1;
            }

            foreach(GameObject go in _characterModels)
            {
                go.SetActive(false);
            }
            _characterModels[_modelIndex].SetActive(true);
        }

        public void OnReady(InputAction.CallbackContext context)
        {
            Debug.Log("OnReady");
            if(context.ReadValueAsButton())
            {
                Debug.Log("Ready or Not");
                _isReady = !_isReady;
                //InputAction switchTeamAction = _playerInput.actions.FindAction("SwitchTeam");

                if(_isReady == true)
                {
                    _switchTeamAction.Disable();
                    _changeAppearanceAction.Disable();
                }
                else
                {
                    _switchTeamAction.Enable();
                    _changeAppearanceAction.Enable();
                }

                ReadyStatusChanged?.Invoke(this, _isReady);
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void ChangeActionMap(string newActionMap)
        {
            Debug.Log("Changed action map to " + newActionMap);
            _playerInput.SwitchCurrentActionMap(newActionMap);
        }

        // --------------------------------------------------------------------------------------------
    }
}