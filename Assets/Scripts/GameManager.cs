using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LJ
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------
        public enum GameState
        {
            MainMenu,
            CharacterSelection,
            GameRunning,
            GamePaused,
            GameOver
        }

        // --- Fields -----------------------------------------------------------------------------------------------------
        public const int ORDERS_PER_ROUND = 3;
        public const int ORDER_TIME = 30;

        [SerializeField] private GameState _currentGameState;


        //public static bool IS_CHARACTERSELECT = false;
        //public static bool IS_GAME_ACTIVE = false;
        //public static bool IS_GAME_OVER = false;
        //public static bool IS_GAME_PAUSED = false;

        public delegate void CharacterSelectionActiveEvent();
        public event CharacterSelectionActiveEvent CharacterSelectionActive;

        public delegate void RoundActiveEvent();
        public event RoundActiveEvent RoundActive;

        public delegate void RoundPausedEvent();
        public event RoundPausedEvent RoundPaused;

        public delegate void RoundFinishedEvent();
        public event RoundFinishedEvent RoundFinished;

        // --- Properties -------------------------------------------------------------------------------------------------
        public static GameManager Instance { get; private set; }
        public GameState CurrentGameState => _currentGameState;

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            switch(_currentGameState)
            {
                case GameState.MainMenu:
                    StartMainMenu();
                    break;
                case GameState.CharacterSelection:
                    StartCharacterSelection();
                    break;
                case GameState.GameRunning:
                    StartRound();
                    break;
            }
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void StartMainMenu()
        {
            _currentGameState = GameState.MainMenu;
            SceneManager.LoadScene(0);
        }

        public void StartCharacterSelection()
        {
            Debug.Log("Start Character Select");
            _currentGameState = GameState.CharacterSelection;
            SceneManager.LoadScene(1);
            CharacterSelectionActive?.Invoke();
        }

        public void StartRound()
        {
            Debug.Log("Start Round");
            Time.timeScale = 1f;
            _currentGameState = GameState.GameRunning;

            if(SceneManager.GetActiveScene().buildIndex != 2)
            {
                SceneManager.LoadScene(2);
            }
            RoundActive?.Invoke();
        }

        public void PauseRound()
        {
            Debug.Log("Paused Round");
            Time.timeScale = 0f;
            _currentGameState = GameState.GamePaused;
            RoundPaused?.Invoke();
        }
        public void EndRound()
        {
            Debug.Log("Game Over");
            _currentGameState = GameState.MainMenu;
            SceneManager.LoadScene(0);
            RoundFinished?.Invoke();

            //Destroy(this.gameObject);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}