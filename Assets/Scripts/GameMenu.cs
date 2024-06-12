using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LJ
{
	public class GameMenu : MonoBehaviour
	{
		// --- Enums ------------------------------------------------------------------------------------------------------

		// --- Fields -----------------------------------------------------------------------------------------------------
		[SerializeField] private GameObject _pauseCanvas;
		[SerializeField] private GameObject _returnCanvas;

        private GameManager _gm;
        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _gm = GameManager.Instance;
            _gm.GamePaused += OnGamePaused;
            _gm.GameActive += OnGameActive;
        }


        private void OnDisable()
        {
            _gm.GamePaused -= OnGamePaused;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnGamePaused()
        {
            _pauseCanvas.SetActive(true);
        }

        private void OnGameActive()
        {
            _pauseCanvas.SetActive(false);
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}