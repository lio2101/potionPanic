using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LJ
{
	public class PauseMenu : MonoBehaviour
	{
		// --- Enums ------------------------------------------------------------------------------------------------------

		// --- Fields -----------------------------------------------------------------------------------------------------
		[SerializeField] private Button _resumeButton;
		// --- Properties -------------------------------------------------------------------------------------------------

		// --- Unity Functions --------------------------------------------------------------------------------------------
		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(_resumeButton.gameObject);

            _resumeButton.onClick.AddListener(GameManager.Instance.ResumeRound);
        }
        private void OnDisable()
        {
            _resumeButton.onClick.RemoveListener(GameManager.Instance.ResumeRound);
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}