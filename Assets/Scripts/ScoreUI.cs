using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LJ
{
    //[Serializable]
    //public class Score
    //{
    //    [SerializeField] private TextMeshPro _scoreUI;
    //    private int _scoreCount;

    //    public TextMeshPro ScoreUI { get { return _scoreUI; } }
    //    public int ScoreCount { get { return _scoreCount; } }
    //}


	public class ScoreUI : MonoBehaviour
	{
		// --- Enums ------------------------------------------------------------------------------------------------------

		// --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private TMP_Text[] _scoresUI = new TMP_Text[2];
        [SerializeField] CustomerController _customerController;

        private int[] _scores = { 0, 0 };

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            _customerController.ScoreChanged += OnScoreChanged;

        }


        private void OnDisable()
        {
            _customerController.ScoreChanged -= OnScoreChanged;
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnScoreChanged(int team)
        {
            Debug.Log("ScoreUI on score changed");
            _scores[team]++;
            _scoresUI[team].text = _scores[team].ToString();
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}