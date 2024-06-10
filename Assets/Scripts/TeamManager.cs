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

        public List<Player> Players { get { return _players; } }
        public bool IsFull => _players.Count == 2;
        public int PlayerCount => _players.Count;

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }
    }

    public class TeamManager : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private PlayerInputManager _playerInputManager;

        [SerializeField] private List<Team> _teams;

        private const int MAX_TEAM_AMOUNT = 2;

        public delegate void PlayerJoinedEvent(Player player);
        public event PlayerJoinedEvent PlayerJoined;

        // --- Properties -------------------------------------------------------------------------------------------------
        public List<Team> Teams { get { return _teams; } }

        private int CurrentPlayerCount => _teams.Sum(t => t.PlayerCount);
        private bool AllTeamsFull => _teams.All(t => t.IsFull);

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {

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

            player.ReadyStatusChanged += OnPlayerReadyChanged;


            playerInput.SwitchCurrentActionMap("CharacterSelect");

            Team nextAvailableTeam = _teams.FirstOrDefault(t => t.IsFull == false);

            if(nextAvailableTeam == null)
            {
                if(_teams.Count < MAX_TEAM_AMOUNT)
                {
                    Team team = new();
                    _teams.Add(team);
                    nextAvailableTeam = team;
                }
                else
                {
                    Debug.LogWarning("All teams are full!");
                    Destroy(playerInput.gameObject);
                    return;
                }
            }

            Debug.Log(nextAvailableTeam != null);
            Debug.Log(player != null);
            nextAvailableTeam.AddPlayer(player);

            //CharacterController controller = player.GetComponent<CharacterController>();
            //controller.enabled = false;
            player.transform.position = _spawnPositions[CurrentPlayerCount - 1].position;
            Physics.SyncTransforms();

            Debug.Log("SpawnPosition: " + _spawnPositions[CurrentPlayerCount - 1].position);
            Debug.Log("PlayerPosition:" + player.gameObject.transform.position);
            Debug.Log("New Player");

            PlayerJoined?.Invoke(player);
        }        

        private void OnPlayerLeft(PlayerInput playerInput)
        {

            Debug.Log("New Player");
        }



        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnPlayerReadyChanged(Player player, bool isReady)
        {

        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void EnableMovement()
        {
            //foreach
        }

        public void DisableMovement()
        {

        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}