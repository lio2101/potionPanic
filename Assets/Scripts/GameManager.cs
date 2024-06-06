using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    public class GameManager : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        private static GameManager _instance;
        [SerializeField] private PlayerInputManager _playerManager;

        [SerializeField] private List<Player> team1;
        [SerializeField] private List<Player> team2;

        public static bool isGameActive;



        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    Debug.Log("Game Manager is null");
                }
                return _instance;
            }
        }
        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _instance = this;

        }

        private void OnEnable()
        {
            //subscribe
            _playerManager.onPlayerJoined += OnPlayerJoined;
            _playerManager.onPlayerLeft += OnPlayerJoined;
        }
        private void OnDisable()
        {
            //unsubscribe    
            _playerManager.onPlayerJoined -= OnPlayerJoined;
            _playerManager.onPlayerLeft -= OnPlayerLeft;
        }

        private void OnPlayerJoined(PlayerInput obj)
        {
            Debug.Log("New Player");
        }

        private void OnPlayerLeft(PlayerInput obj)
        {

            Debug.Log("New Player");
        }




        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void StartRound()
        {

        }

        public void PauseRound()
        {

        }

        public void EndRound()
        {

        }

        public void QuitGame()
        {

        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}