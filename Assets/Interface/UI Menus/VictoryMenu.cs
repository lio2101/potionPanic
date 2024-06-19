using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LJ
{
	public class VictoryMenu : MonoBehaviour
	{
		// --- Enums ------------------------------------------------------------------------------------------------------

		// --- Fields -----------------------------------------------------------------------------------------------------
		[SerializeField] Button _backToMainMenu;
		[SerializeField] Button _playAgain;
		// --- Properties -------------------------------------------------------------------------------------------------

		// --- Unity Functions --------------------------------------------------------------------------------------------
		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(_backToMainMenu.gameObject);
            _playAgain.onClick.AddListener(GameManager.Instance.StartRound);
            _backToMainMenu.onClick.AddListener(GameManager.Instance.StartMainMenu);
        }

        private void OnDisable()
        {
            _playAgain.onClick.RemoveListener(GameManager.Instance.StartRound);
            _backToMainMenu.onClick.RemoveListener(GameManager.Instance.StartMainMenu);
        }


        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}