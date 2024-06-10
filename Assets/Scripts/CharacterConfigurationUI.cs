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
		private Image _readyImage;
		private Image _teamColorImage;

        // --- Properties -------------------------------------------------------------------------------------------------
		public Player LinkedPlayer { get; private set; }

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _readyImage = GetComponentInChildren<Image>();
			_teamColorImage = GetComponentInChildren<Image>();

            //player.TeamSwitched += ChangeTeamColor;
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
            throw new NotImplementedException();
        }

        private void OnTeamSwitched(Player player, int teamIndex)
        {
            if(teamIndex == 0)
            {
                _readyImage.color = Color.red;
            }
            else if(teamIndex == 1)
            {
                _readyImage.color = Color.blue;
            }

            _readyImage.color = teamIndex switch
            {
                0 => Color.red,
                1 => Color.blue,
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