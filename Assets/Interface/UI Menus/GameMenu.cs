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

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            GameManager.Instance.RoundPaused += OnGamePaused;
            GameManager.Instance.RoundActive += OnGameActive;
            //GameManager.Instance.RoundFinished += OnGameFinished;
        }


        private void OnDisable()
        {
            GameManager.Instance.RoundPaused -= OnGamePaused;
            GameManager.Instance.RoundActive -= OnGameActive;
            //GameManager.Instance.RoundFinished -= OnGameFinished;
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

        //private void OnGameFinished()
        //{

        //}
        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void ShowFinalScore(Team team)
        {
            _finishCanvas.SetActive(true);
            if(team != null)
            {
                string baseString = _winnerText.text;
                StringBuilder sb = new();
                sb.Append(baseString);
                sb.Replace("%TEAM%", $"{team.TeamName}");
                //sb.Replace("%POINTS%", $"{team.TeamScore}");
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