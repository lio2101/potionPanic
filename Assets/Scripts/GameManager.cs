using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LJ
{
    public class GameManager : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        private static GameManager _instance;

        public const int ORDERS_PER_ROUND = 10;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    Debug.Log("Game Manager is null");
                }
                return _instance;
            }
        }
        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            //SceneManager.LoadScene(0);
            //StartCharacterSelection();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void StartCharacterSelection()
        {
            Debug.Log("Start Character Select");
            
        }

        public void StartRound()
        {
            Debug.Log("Start Game");
            SceneManager.LoadScene(1);
        }

        public void PauseRound()
        {
            Debug.Log("Paused Game");
        }

        public void EndRound()
        {
            Debug.Log("Game Over");

        }

        public void QuitGame()
        {
            Application.Quit();
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}