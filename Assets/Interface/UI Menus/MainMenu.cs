using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LJ.UI
{
	public class MainMenu : MonoBehaviour
	{
		// --- Enums ------------------------------------------------------------------------------------------------------

		// --- Fields -----------------------------------------------------------------------------------------------------
		[SerializeField] private Button _startGame;
		[SerializeField] private Button _settings;
		[SerializeField] private Button _exitGame;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            _startGame.onClick.AddListener(GameManager.Instance.StartCharacterSelection);
            _exitGame.onClick.AddListener(GameManager.Instance.QuitGame);
        }

        private void OnDisable()
        {
            _startGame.onClick.RemoveListener(GameManager.Instance.StartCharacterSelection);
            _exitGame.onClick.RemoveListener(GameManager.Instance.QuitGame);

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}