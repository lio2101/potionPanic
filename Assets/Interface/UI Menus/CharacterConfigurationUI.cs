using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LJ.UI
{
    public class CharacterConfigurationUI : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Image _readyImage;
        [SerializeField] private Image _teamColorImage;

        // --- Properties -------------------------------------------------------------------------------------------------
        public Player LinkedPlayer { get; private set; }

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _teamColorImage.color = Color.red;
            _readyImage.color = Color.grey;
        }

        private void OnDestroy()
        {
            if(LinkedPlayer != null)
            {
                LinkedPlayer.TeamSwitched -= OnTeamSwitched;
                LinkedPlayer.ReadyStatusChanged -= OnReadyStatusChanged;
            }
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnReadyStatusChanged(Player player, bool isReady)
        {
            if(isReady)
            {
                _readyImage.color = Color.green;

            }
            else
            {
                _readyImage.color = Color.grey;
            }
        }

        private void OnTeamSwitched(Player player, Team team)
        {
            Debug.Log("Change Team color");
            _teamColorImage.color = team.Color;
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void LinkToPlayer(Player player)
        {

            LinkedPlayer = player;
            LinkedPlayer.TeamSwitched += OnTeamSwitched;
            LinkedPlayer.ReadyStatusChanged += OnReadyStatusChanged;
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}