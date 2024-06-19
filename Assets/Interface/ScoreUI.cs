using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LJ.UI
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

        private int[] _scores = { 0, 0 };

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            foreach(Team team in TeamManager.Instance.Teams)
            {
                team.ScoreChanged += OnScoreChanged;
            }
        }

        private void OnDisable()
        {
            foreach(Team team in TeamManager.Instance.Teams)
            {
                team.ScoreChanged -= OnScoreChanged;
            }
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnScoreChanged(Team team, int score)
        {
            Debug.Log("ScoreUI on score changed");
            _scores[team.Index]++;
            _scoresUI[team.Index].text = _scores[team.Index].ToString();
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}