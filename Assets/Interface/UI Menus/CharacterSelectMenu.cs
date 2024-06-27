using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LJ.UI
{
    public class CharacterSelectMenu : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private TeamManager _teamManager;
        [SerializeField] private CharacterConfigurationUI[] _configurationUI;
        [SerializeField] private GameObject _returnMenu;

        [SerializeField] private GameObject _readyError;
        private Image _errorImage;
        private TMP_Text _errorText;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _teamManager.PlayerJoined += OnPlayerJoined;
        }

        private void Start()
        {
            _teamManager = TeamManager.Instance;
            GameManager.Instance.RoundPaused += OnGamePaused;
            TeamManager.Instance.ReadyError += ShowReadyError;

            _errorImage = _readyError.GetComponent<Image>();
            _errorText = _readyError.GetComponentInChildren<TMP_Text>();
        }


        private void OnDestroy()
        {
            _teamManager.PlayerJoined -= OnPlayerJoined;
            GameManager.Instance.RoundPaused -= OnGamePaused;
            TeamManager.Instance.ReadyError -= ShowReadyError;
        }

        private void OnPlayerJoined(Player player)
        {
            player.ReadyStatusChanged += OnReadyStatusChanged;
            _configurationUI.FirstOrDefault(c => c.LinkedPlayer == null).LinkToPlayer(player);
        }


        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void ShowReadyError(string errorMessage)
        {
            _readyError.SetActive(true);
            _errorImage.color = Color.red;
            _errorText.color = Color.black;
            _errorText.text = errorMessage;
            StartCoroutine(ShowErrorRoutine());
        }

        private void OnReadyStatusChanged(Player player, bool isReady)
        {
            if(_configurationUI.Count(c => c.LinkedPlayer != null && c.LinkedPlayer.IsReady) == _teamManager.CurrentPlayerCount)
            {
                Debug.Log("All Players Ready!");
                _teamManager.Approve();
            }
            else { Debug.Log("Not all players ready");  }
        }

        public void OnGamePaused()
        {
            _returnMenu.SetActive(true);
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private IEnumerator ShowErrorRoutine()
        {
            yield return new WaitForSeconds(1f);
            float fadeCount = 1;

            while(fadeCount>= 0)
            {
                fadeCount -= 0.01f;
                yield return new WaitForSeconds(0.01f);
                _errorImage.color = new Color(255, 0, 0, fadeCount);
                _errorText.color = new Color(0, 0, 0, fadeCount);
            }
            _readyError.SetActive(false);

        }

        // --------------------------------------------------------------------------------------------
    }
}