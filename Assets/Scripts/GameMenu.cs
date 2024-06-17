using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;

namespace LJ
{
    public class GameMenu : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private GameObject _pauseCanvas;
        [SerializeField] private GameObject _returnCanvas;
        [SerializeField] private GameObject _finishCanvas;

        [SerializeField] private TMP_Text _winnerText;

        private GameManager _gm;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _gm = GameManager.Instance;
            _gm.RoundPaused += OnGamePaused;
            _gm.RoundActive += OnGameActive;
            _gm.RoundFinished += OnGameFinished;
        }


        private void OnDisable()
        {
            _gm.RoundPaused -= OnGamePaused;
            _gm.RoundActive -= OnGameActive;
            _gm.RoundFinished -= OnGameFinished;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnGamePaused()
        {
            Debug.Log("OnGamePaused GameMenu");
            _pauseCanvas.SetActive(true);
        }

        private void OnGameActive()
        {
            _pauseCanvas.SetActive(false);
        }

        private void OnGameFinished()
        {

        }
        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void ShowFinalScore(Team team)
        {
            if(team != null)
            {
                _finishCanvas.SetActive(true);
                string baseString = _winnerText.text;
                StringBuilder sb = new();
                sb.Append(baseString);
                sb.Replace("%TEAM%", $"{team.TeamName}");
                sb.Replace("%POINTS%", $"{team.TeamScore}");
                _winnerText.text = sb.ToString();
            }
            else
            {
                _winnerText.text = "DRAW";
            }
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}