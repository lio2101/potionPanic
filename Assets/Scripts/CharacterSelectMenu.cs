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
        

        [SerializeField] private Sprite _colorTeam1;
        [SerializeField] private Sprite _colorTeam2;

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
            _configurationUI.FirstOrDefault(c => c.LinkedPlayer == null).LinkToPlayer(player);
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void UpdateUI(int teamIndex, int playerIndex)
        {
            //_teamManager.Teams[teamIndex].Players[playerIndex].
        }


        // --------------------------------------------------------------------------------------------
    }
}