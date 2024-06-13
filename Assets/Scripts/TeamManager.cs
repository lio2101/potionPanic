using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    [Serializable]
    public class Team
    {
        [SerializeField] private List<Player> _players = new();

        public List<Player> Players { get { return _players; } }
        //public bool IsFull => _players.Count == 2;
        public int PlayerCount => _players.Count;

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            _players.Remove(player);
        }

        public Player FindPlayer(Player player)
        {
            return _players.FirstOrDefault(p => p == player);
        }
    }

    public class TeamManager : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _gameSpawnPositions;
        [SerializeField] private PlayerInputManager _playerInputManager;

        [SerializeField] private List<Team> _teams;

        private const int MAX_TEAM_AMOUNT = 2;

        public delegate void PlayerJoinedEvent(Player player);
        public event PlayerJoinedEvent PlayerJoined;

        private GameManager _gm;

        // --- Properties -------------------------------------------------------------------------------------------------
        public List<Team> Teams { get { return _teams; } }

        public int CurrentPlayerCount => _teams.Sum(t => t.PlayerCount);
        //private bool AllTeamsFull => _teams.All(t => t.IsFull);

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _gm = GameManager.Instance;
            _gm.RoundFinished += OnRoundFinished;
            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            //subscribe
            _playerInputManager.onPlayerJoined += OnPlayerJoined;
            _playerInputManager.onPlayerLeft += OnPlayerLeft;
        }


        private void OnDisable()
        {
            //unsubscribe
            _playerInputManager.onPlayerJoined -= OnPlayerJoined;
            _playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            Player player = playerInput.GetComponent<Player>();

            if(CurrentPlayerCount == 0)
            {
                player.IsPlayerOne = true;
            }

            //player.ReadyStatusChanged += OnPlayerReadyChanged;
            player.TeamSwitched += OnTeamSwitched;

            playerInput.SwitchCurrentActionMap("CharacterSelect");
            playerInput.DeactivateInput();
            StartCoroutine(SwitchRoutine());

            IEnumerator SwitchRoutine()
            {
                yield return null;
                playerInput.ActivateInput();
            }

            if(_teams.Count < MAX_TEAM_AMOUNT)
            {
                Team team = new();
                _teams.Add(team);
            }
            _teams[0].AddPlayer(player);

            player.transform.position = _spawnPositions[CurrentPlayerCount - 1].position;
            Physics.SyncTransforms();
            DontDestroyOnLoad(player);

            Debug.Log("New Player");
            Debug.Log($"Currently {CurrentPlayerCount} players");
            Debug.Log($"Currently {_teams[0].PlayerCount} players in Team 1");

            PlayerJoined?.Invoke(player);
        }

        private void OnTeamSwitched(Player player, int teamIndex)
        {

            if(CurrentPlayerCount > 1)
            {
                if(_teams.Count < MAX_TEAM_AMOUNT)
                {
                    Team team = new();
                    _teams.Add(team);
                }
                if(teamIndex == 0)
                {
                    _teams[0].Players.Remove(player);
                    _teams[1].Players.Add(player);
                    Debug.Log($"Moved Player from team 1 to team 2");
                }
                else if(teamIndex == 1)
                {
                    _teams[1].Players.Remove(player);
                    _teams[0].Players.Add(player);
                    Debug.Log($"Moved Player from team 2 to team 1");
                }
                Debug.Log($"Currently {_teams[0].PlayerCount} players in Team 1");
                Debug.Log($"Currently {_teams[1].PlayerCount} players in Team 2");
            }
            else { Debug.Log($"need more than {CurrentPlayerCount} players to change team"); }
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            //Player player = playerInput.GetComponent<Player>();
            //player.ReadyStatusChanged -= OnPlayerReadyChanged;
            //player.TeamSwitched -= OnTeamSwitched;
        }

        private void OnPause(InputValue inputValue)
        {
            
        }



        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnRoundFinished()
        {
            foreach(var team in _teams)
            {
                foreach (var player in team.Players)
                {
                    //team.Players.Remove(player);
                    Destroy(player.gameObject);
                }
            }
            Debug.Log("Destroyed all players");
            Destroy(this.gameObject);
        }

        //private void OnPlayerReadyChanged(Player player, bool isReady)
        //{

        //}

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void Approve()
        {
            if(CurrentPlayerCount == 2 || CurrentPlayerCount == 4)
            {
                Debug.Log("Valid player amount");
                if(_teams.Count(t => t.PlayerCount == 1) == 2)
                {
                    Debug.Log("1 v 1 Starting");

                    StartCoroutine(CountDownRoutine());

                }
                else if(_teams.Count(t => t.PlayerCount == 2) == 2)
                {
                    Debug.Log("2 v 2 Starting");

                    StartCoroutine(CountDownRoutine());

                }
                else { Debug.Log("Invalid team sizes"); }
            }
            else
            {
                Debug.Log("Invalid player amount. Please play with 2 or 4 players");
            }

            IEnumerator CountDownRoutine()
            {
                yield return new WaitForSeconds(3);
                _gm.StartRound();
                int index = 0;
                foreach(var team in _teams)
                {
                    foreach(var player in team.Players)
                    {
                        player.transform.position = _gameSpawnPositions[index].position;
                        
                        Physics.SyncTransforms();
                        index++;
                    }
                }
                Debug.Log("Moved all players to new spawn positions");
                
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}