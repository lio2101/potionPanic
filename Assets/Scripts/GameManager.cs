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

        public const int ORDERS_PER_ROUND = 5;
        public const int ORDER_TIME = 30;

        public static bool IS_CHARACTERSELECT = false;
        public static bool IS_GAME_ACTIVE = false;
        public static bool IS_GAME_OVER = false;
        public static bool IS_GAME_PAUSED = false;

        public delegate void CharacterSelectionActiveEvent();
        public event CharacterSelectionActiveEvent CharacterSelectionActive;

        public delegate void RoundActiveEvent();
        public event RoundActiveEvent RoundActive;

        public delegate void RoundPausedEvent();
        public event RoundPausedEvent RoundPaused;

        public delegate void RoundFinishedEvent();
        public event RoundFinishedEvent RoundFinished
            ;

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
            //SceneManager.LoadScene(0);
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
            SceneManager.LoadScene(1);
            Debug.Log("Start Round");
            IS_CHARACTERSELECT = false;
            IS_GAME_PAUSED = false;
            IS_GAME_ACTIVE = true;
            RoundActive?.Invoke();
            //transform player objects
        }

        public void PauseRound()
        {
            Debug.Log("Paused Round");
            IS_GAME_PAUSED = true;
            IS_GAME_ACTIVE = false;
            RoundPaused?.Invoke();
        }
        public void EndRound()
        {
            SceneManager.LoadScene(0);
            Debug.Log("Game Over");
            IS_GAME_OVER = true;
            RoundFinished?.Invoke();

            Destroy(this.gameObject);


        }

        public void QuitGame()
        {
            Application.Quit();
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}