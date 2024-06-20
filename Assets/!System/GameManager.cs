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

        private Coroutine _startGameRoutine;

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

        public bool GameStarting => _startGameRoutine != null;
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
            _currentGameState = GameState.MainMenu;

            if(TeamManager.Instance != null)
                Destroy(TeamManager.Instance.gameObject);

            ToggleMusic();
            SceneManager.LoadScene(0);
        }

        public void StartCharacterSelection()
        {
            Time.timeScale = 1f;
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
            _startGameRoutine = StartCoroutine(StartGameRoutine());
        }

        private IEnumerator StartGameRoutine()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(2);
            yield return new WaitForSeconds(0.1f);
            TeamManager.Instance.SetPlayerGamePositions();

            yield return new WaitForSeconds(3f);

            _currentGameState = GameState.GameRunning;
            ToggleMusic();
            Debug.Log("Start a new game");
            
            _startGameRoutine = null;
            RoundActive?.Invoke();
        }

        public void ResumeRound()
        {
            _currentGameState = GameState.GameRunning;
            ToggleMusic();
            Time.timeScale = 1f;
            RoundActive?.Invoke();
        }

        public void PauseRound()
        {
            _currentGameState = GameState.GamePaused;
            Time.timeScale = 0f;

            ToggleMusic();
            RoundPaused?.Invoke();
            Debug.Log("Paused Round");
        }
        public void EndRound()
        {
            Time.timeScale = 0;
            ToggleMusic();

            Debug.Log("Game Over");
            _currentGameState = GameState.GameIsEnding;
            RoundFinished?.Invoke();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        private void ToggleMusic()
        {
            switch(_currentGameState)
            {
                case GameState.MainMenu:
                case GameState.CharacterSelection:
                    if(_audioSource.clip != _menuMusic)
                        _audioSource.clip = _menuMusic;
                    _audioSource.Play();
                    break;
                case GameState.GameRunning:
                    if(_audioSource.clip != _gameMusic)
                        _audioSource.clip = _gameMusic;
                    _audioSource.Play();
                    break;
                case GameState.GamePaused:
                    _audioSource.Pause();
                    break;
                case GameState.GameIsEnding:
                case GameState.GameOver:
                    if(_audioSource.isPlaying)
                        _audioSource.Stop();
                    break;
            }
        }

        // --------------------------------------------------------------------------------------------
    }
}