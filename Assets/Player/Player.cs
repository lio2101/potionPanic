using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    public class Player : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------		

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private MeshRenderer[] _characterModels;
        [SerializeField] private MeshRenderer[] _hats;
        [SerializeField] private InputActionReference _switchTeamReference;
        [SerializeField] private InputActionReference _changeAppearanceReference;
        [SerializeField] private InputActionReference _readyReference;
        [SerializeField] private PlayerInput _playerInput;

        private Team _team;

        private bool _isReady = false;

        private int _modelIndex = 0;

        private InputAction _switchTeamAction;
        private InputAction _changeAppearanceAction;
        private InputAction _readyAction;

        [SerializeField] private bool _isPlayerOne = false;

        //Delegates
        public delegate void ReadyStatusChangedEvent(Player player, bool isReady);
        public event ReadyStatusChangedEvent ReadyStatusChanged;

        public delegate void TeamSwitchedEvent(Player player, Team team);
        public event TeamSwitchedEvent TeamSwitched;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool IsReady => _isReady;
        public bool IsPlayerOne { get { return _isPlayerOne; } set { _isPlayerOne = value; } }
        public Team Team => _team;
        public int Index => _playerInput.playerIndex;

        // --- Unity Functions -----------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            _switchTeamAction = _playerInput.actions.FindAction(_switchTeamReference.action.id);

            _changeAppearanceAction = _playerInput.actions.FindAction(_changeAppearanceReference.action.id);

            _readyAction = _playerInput.actions.FindAction(_readyReference.action.id);

        }

        private void Start()
        {
            ChangeActionMap();
            GameManager.Instance.CharacterSelectionActive += ChangeActionMap;
            GameManager.Instance.RoundActive += ChangeActionMap;
            GameManager.Instance.RoundFinished += ChangeActionMap;
            //GameManager.Instance.ReloadGame += ChangeActionMap;
        }
        private void OnDestroy()
        {
            GameManager.Instance.CharacterSelectionActive -= ChangeActionMap;
            GameManager.Instance.RoundActive -= ChangeActionMap;
            GameManager.Instance.RoundFinished -= ChangeActionMap;
            //GameManager.Instance.ReloadGame -= ChangeActionMap;

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        public void SetTeam(Team team)
        {
            _team = team;

            foreach(MeshRenderer hat in _hats)
            {
                hat.material = _team.HatColor;
            }

            TeamSwitched?.Invoke(this, _team);
        }

        public void OnSwitchTeam(InputValue inputValue)
        {
            int direction = Mathf.RoundToInt(inputValue.Get<float>());
            TeamManager.Instance.SwitchTeam(this, direction);
        }

        public void OnChangeAppearance(InputValue inputValue)
        {
            float input = inputValue.Get<float>();

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

            for(int i = 0; i < _characterModels.Length; i++)
            {
                _characterModels[i].gameObject.SetActive(i == _modelIndex);
            }
        }

        public void OnReady(InputValue inputValue)
        {
            Debug.Log("OnReady");
            if(inputValue.isPressed)
            {
                if(GameManager.Instance.GameStarting)
                {
                    Debug.LogWarning("Can't unready because game is already starting!");
                    return;
                }

                Debug.Log("Ready or Not");
                _isReady = !_isReady;

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

        public void OnPause(InputValue inputValue)
        {
            if(inputValue.isPressed)
            {
                if(this.IsPlayerOne)
                {
                    GameManager.Instance.PauseRound();
                    ChangeActionMap();
                }
            }
        }

        public void OnReturn(InputValue inputValue)
        {
            if(inputValue.isPressed)
            {
                if(this.IsPlayerOne)
                {
                    GameManager.Instance.PauseRound();
                    ChangeActionMap();
                }
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void ChangeActionMap()
        {
            if(_playerInput != null)
            {

                switch(GameManager.Instance.CurrentGameState)
                {
                    //case GameManager.GameState.MainMenu:
                    //    _playerInput.SwitchCurrentActionMap("UI");
                    //    break;
                    case GameManager.GameState.CharacterSelection:
                        _playerInput.SwitchCurrentActionMap("CharacterSelect");
                        break;
                    case GameManager.GameState.GameRunning:
                        Debug.Log("ChangeActionMap to GameControls");
                        _playerInput.SwitchCurrentActionMap("GameControls");
                        break;
                    case GameManager.GameState.GamePaused:
                        if(this.IsPlayerOne)
                        {
                            Debug.Log("ChangeActionMap to UI");
                            _playerInput.SwitchCurrentActionMap("UI");
                        }
                            break;
                    case GameManager.GameState.GameIsEnding:
                        Debug.Log("ChangeActionMap to UI");
                        _playerInput.SwitchCurrentActionMap("UI");
                        break;
                    //case GameManager.GameState.GameOver:
                    //    Debug.Log("ChangeActionMap to UI");
                    //    _playerInput.SwitchCurrentActionMap("UI");
                    //    break;
                    //case GameManager.GameState.MainMenu:
                    //    Debug.Log("ChangeActionMap to UI");
                    //    _playerInput.SwitchCurrentActionMap("UI");
                    //    break;
                }
            }
        }

        // --------------------------------------------------------------------------------------------
    }
}