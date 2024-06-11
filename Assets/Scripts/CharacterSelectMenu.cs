using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LJ.UI
{
    public class CharacterSelectMenu : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private TeamManager _teamManager;
        [SerializeField] private CharacterConfigurationUI[] _configurationUI;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _teamManager.PlayerJoined += OnPlayerJoined;
        }

        private void OnDestroy()
        {
            _teamManager.PlayerJoined -= OnPlayerJoined;
        }

        private void OnPlayerJoined(Player player)
        {
            player.ReadyStatusChanged += OnReadyStatusChanged;
            _configurationUI.FirstOrDefault(c => c.LinkedPlayer == null).LinkToPlayer(player);
        }


        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnReadyStatusChanged(Player player, bool isReady)
        {
            if(_configurationUI.Count(c => c.LinkedPlayer != null && c.LinkedPlayer.IsReady) == _teamManager.CurrentPlayerCount)
            {
                Debug.Log("All Players Ready!");
                _teamManager.Approve();
            }
            else { Debug.Log("Not all players ready");  }
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------


        // --------------------------------------------------------------------------------------------
    }
}