using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LJ
{
    public class GameManager : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        private static GameManager _instance;

        public const int ORDERS_PER_ROUND = 10;

        public static bool IS_CHARACTERSELECT = false;
        public static bool IS_GAME_ACTIVE = false;
        public static bool IS_GAME_OVER = false;

        public delegate void CharacterSelectionActiveEvent();
        public event CharacterSelectionActiveEvent CharacterSelectionActive;

        public delegate void GameActiveEvent();
        public event GameActiveEvent GameActive;

        public delegate void GamePausedEvent();
        public event GamePausedEvent GamePaused;




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
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            StartCharacterSelection();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void StartCharacterSelection()
        {
            Debug.Log("Start Character Select");
            IS_CHARACTERSELECT = true;
            CharacterSelectionActive?.Invoke();
        }

        public void StartRound()
        {
            Debug.Log("Start Game");
            SceneManager.LoadScene(1);
            IS_CHARACTERSELECT = false;
            IS_GAME_ACTIVE = true;
            GameActive?.Invoke();
            //transform player objects
        }

        public void PauseRound()
        {
            Debug.Log("Paused Game");
            IS_GAME_ACTIVE = true;
            GamePaused?.Invoke();
        }

        public void EndRound()
        {
            IS_GAME_OVER = true;
            Debug.Log("Game Over");

        }

        public void QuitGame()
        {
            Application.Quit();
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}