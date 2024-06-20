using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LJ
{
    [Serializable]
    public class Team
    {
        [SerializeField] private List<Player> _players = new();
        [SerializeField] private int _teamScore;
        [SerializeField] private string _teamName;
        [SerializeField] private Material _hatColor;
        [SerializeField] private Color _color;

        private int _index;
        public int Index => _index;
        public Material HatColor => _hatColor;
        public Color Color => _color;
        public int TeamScore { get { return _teamScore; } set { _teamScore = value; } }
        public string TeamName { get { return _teamName; } set { _teamName = value; } }

        public List<Player> Players { get { return _players; } }
        public int PlayerCount => _players.Count;

        public delegate void ScoreChangedEvent(Team team, int score);
        public event ScoreChangedEvent ScoreChanged;

        public void SetIndex(int index)
        {
            _index = index;
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
            player.SetTeam(this);
        }

        public void RemovePlayer(Player player)
        {
            _players.Remove(player);
        }

        public Player FindPlayer(Player player)
        {
            return _players.FirstOrDefault(p => p == player);
        }

        public void AddPoint()
        {
            _teamScore++;
            ScoreChanged?.Invoke(this, _teamScore);
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

        // --- Properties -------------------------------------------------------------------------------------------------
        public List<Team> Teams { get { return _teams; } }
        public int CurrentPlayerCount => _teams.Sum(t => t.PlayerCount);

        public static TeamManager Instance { get; private set; }

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
            for(int i = 0; i < _teams.Count; i++)
            {
                _teams[i].SetIndex(i);
            }
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

        private void OnDestroy()
        {
            DeletePlayers();
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            Player player = playerInput.GetComponent<Player>();

            player.IsPlayerOne = CurrentPlayerCount == 0;

            //player.ReadyStatusChanged += OnPlayerReadyChanged;

            playerInput.SwitchCurrentActionMap("CharacterSelect");
            playerInput.DeactivateInput();
            StartCoroutine(SwitchRoutine());

            IEnumerator SwitchRoutine()
            {
                yield return null;
                playerInput.ActivateInput();
            }

            Team teamToJoinTo = _teams.OrderBy(t => t.PlayerCount).FirstOrDefault();
            _teams[teamToJoinTo.Index].AddPlayer(player);

            player.transform.position = _spawnPositions[CurrentPlayerCount - 1].position;
            Physics.SyncTransforms();
            DontDestroyOnLoad(player);

            Debug.Log("New Player");
            Debug.Log($"Currently {CurrentPlayerCount} players");
            Debug.Log($"Currently {_teams[0].PlayerCount} players in Team 1");

            PlayerJoined?.Invoke(player);
        }

        public void SwitchTeam(Player player, int changeDirection)
        {
            int oldTeam = player.Team.Index;
            int newTeam = player.Team.Index + changeDirection;
            if(newTeam < 0)
            {
                newTeam = _teams.Count - 1;
            }
            else if(newTeam == _teams.Count)
            {
                newTeam = 0;
            }

            _teams[oldTeam].RemovePlayer(player);
            _teams[newTeam].AddPlayer(player);

            Debug.Log($"Moved Player '{player.Index}' from team '{_teams[oldTeam].TeamName}' to team '{_teams[newTeam].TeamName}'");
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            //Player player = playerInput.GetComponent<Player>();
            //player.ReadyStatusChanged -= OnPlayerReadyChanged;
            //player.TeamSwitched -= OnTeamSwitched;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        //private void OnRoundFinished()
        //{
        //    //move Players back to spawn
        //    foreach(var team in _teams)
        //    {
        //        foreach(var player in team.Players)
        //        {
        //            player.transform.position = _gameSpawnPositions[CurrentPlayerCount - 1].position;
        //        }
        //    }
        //}

        private void DeletePlayers()
        {
            foreach(var team in _teams)
            {
                foreach(var player in team.Players)
                {
                    if(player != null)
                    {
                        Destroy(player.gameObject);
                    }
                }
            }
            Debug.Log("Destroyed all players");
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        public Team CalculateWinner()
        {
            Team leader = _teams.OrderByDescending(t => t.TeamScore).FirstOrDefault();
            int leaders = _teams.Count(t => t.TeamScore == leader.TeamScore);

            //Reset Scores
            foreach(Team team in _teams)
            {
                team.TeamScore = 0;
            }

            return leaders == 1 ? leader : null;
        }

        public void Approve()
        {
            bool evenTeams = _teams.All(t => t.PlayerCount == CurrentPlayerCount / _teams.Count);
            if(evenTeams)
            {
                GameManager.Instance.StartRound();
            }
            else
            {
                if(CurrentPlayerCount % _teams.Count != 0)
                {
                    // not enough players
                }
                else if(!evenTeams)
                {
                    // uneven teams
                }

                Debug.Log("Invalid player amount. Please play with 2 or 4 players");
            }
        }

        public void SetPlayerGamePositions()
        {
            int index = 0;
            foreach(var team in _teams)
            {
                foreach(Player player in team.Players)
                {
                    player.transform.position = _gameSpawnPositions[index].position;
                    Physics.SyncTransforms();
                    index++;
                }
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}