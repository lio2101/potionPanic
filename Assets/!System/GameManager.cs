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
            GameIsEnding,
            GameOver
        }

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private GameState _currentGameState;

        [SerializeField] private AudioClip _menuMusic;
        [SerializeField] private AudioClip _gameMusic;
        private AudioSource _audioSource;

        public const int ORDERS_PER_ROUND = 1;
        public const int ORDER_TIME = 30;


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

            _audioSource = GetComponent<AudioSource>();
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
            _audioSource.clip = _menuMusic;
            _audioSource.Play();
            _currentGameState = GameState.MainMenu;
            SceneManager.LoadScene(0);
        }

        public void StartCharacterSelection()
        {
            Debug.Log("Start Character Select");
            _currentGameState = GameState.CharacterSelection;

            if(SceneManager.GetActiveScene().buildIndex != 1)
            {
                SceneManager.LoadScene(1);
            }
            CharacterSelectionActive?.Invoke();
        }

        public void StartRound()
        {
            _audioSource.clip = _gameMusic;
            _audioSource.Play();

            Debug.Log("Start Round");
            Time.timeScale = 1f;
            _currentGameState = GameState.GameRunning;

            if(SceneManager.GetActiveScene().buildIndex != 2 || _currentGameState == GameState.GameIsEnding)
            {
                SceneManager.LoadScene(2);
                //How to Reload Scene?
            }
            RoundActive?.Invoke();
        }

        public void PauseRound()
        {
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                _audioSource.Pause();
            }

            Debug.Log("Paused Round");
            Time.timeScale = 0f;
            _currentGameState = GameState.GamePaused;
            RoundPaused?.Invoke();
        }
        public void EndRound()
        {
            _audioSource.clip = _menuMusic;
            _audioSource.Play();

            Debug.Log("Game Over");
            _currentGameState = GameState.GameIsEnding;
            RoundFinished?.Invoke();
        }

        public void Reset()
        {
            
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}