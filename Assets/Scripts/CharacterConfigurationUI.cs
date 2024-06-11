using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
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

        private void OnTeamSwitched(Player player, int teamIndex)
        {
            _teamColorImage.color = teamIndex switch
            {
                0 => Color.blue,
                1 => Color.red,
                _ => Color.white
            };
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