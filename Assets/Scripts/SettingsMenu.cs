using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LJ.UI
{
	public class SettingsMenu : MonoBehaviour
	{
		// --- Enums ------------------------------------------------------------------------------------------------------

		// --- Fields -----------------------------------------------------------------------------------------------------
		[SerializeField] private Button _resolutionButton;
		[SerializeField] private Button _volumeButton;
		[SerializeField] private Button _returnButton;
        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_resolutionButton.gameObject);
            

           // EventSystem.current.firstSelectedGameObject = _resolutionButton.gameObject;
            //add listeners
        }
        private void OnDisable()
        {
            
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}