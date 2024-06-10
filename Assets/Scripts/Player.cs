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

        private bool _isReady = false;
        private GameManager _gm;


        private PlayerInput _playerInput;
        private int _modelIndex = 0;
        private int _teamIndex;
        private int _playerIndex;

        //Delegates
        public delegate void ReadyStatusChangedEvent(Player player, bool isReady);
        public event ReadyStatusChangedEvent ReadyStatusChanged;

        public delegate void TeamSwitchedEvent(Player player, int teamIndex);
        public event TeamSwitchedEvent TeamSwitched;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool IsReady => _isReady;

        // --- Unity Functions -----------------------------------------------------------------------------------------------
        private void Start()
        {
            _gm = GameManager.Instance;
            _gm.CharacterSelectionActive += ChangeActionMap;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void OnSwitchTeam(InputValue inputValue)
        {
            Debug.Log("SwitchTeam");

            if(_teamIndex == 0) { _teamIndex = 1; }
            else if (_teamIndex == 1) {  _teamIndex = 0; }

            TeamSwitched.Invoke(this, _teamIndex);
        }

        public void OnChangeAppearance(InputValue inputValue)
        {
            Debug.Log("Change Prefab");
            float _input = inputValue.Get<float>();


            Debug.Log(_input);
            _modelIndex += (int)_input;

            if(_modelIndex >= _characterModels.Length)
            {
                _modelIndex = 0;
            }
            if(_modelIndex < 0)
            {
                _modelIndex = _characterModels.Length - 1;
            }

            foreach(GameObject go in  _characterModels)
            {
                go.SetActive(false);
            }
            _characterModels[_modelIndex].SetActive(true);
        }

        public void OnReady(InputValue inputValue)
        {
            if(inputValue.isPressed)
            {
                Debug.Log("Ready or Not");
                _isReady = !_isReady;
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