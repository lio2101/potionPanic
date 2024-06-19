using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LJ
{
    public class ReturnMenu : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Button _yes;
        [SerializeField] private Button _no;
        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_no.gameObject);
            _no.onClick.AddListener(Cancel);
            _yes.onClick.AddListener(GameManager.Instance.StartMainMenu); //this might not delete players actually
        }

        private void OnDisable()
        {
            _yes.onClick.RemoveAllListeners();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void Cancel()
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                GameManager.Instance.StartCharacterSelection();
            }
            // --------------------------------------------------------------------------------------------
        }
    }
}