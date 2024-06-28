using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace LJ
{
    public class CountDownController : MonoBehaviour
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private GameObject _wrapper;
        [SerializeField] private AudioClip _coundDownSound;
        [SerializeField] private AudioClip _goSound;

        private AudioSource _source;
        private TextMeshProUGUI _text;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>(true);
            _source = GetComponent<AudioSource>();
            _wrapper.SetActive(false);
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public void StartCountDown()
        {
            _wrapper.SetActive(true);
            StartCoroutine(CountDownRoutine());
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private IEnumerator CountDownRoutine()
        {
            int count = 3;

            _source.clip = _coundDownSound;
            while(count > 0)
            {
                Debug.Log(count);
                _text.text = count.ToString();
                _source.Play();
                yield return new WaitForSecondsRealtime(1);
                count--;
            }
            _text.text = "GO";
            _source.clip = _goSound;
            _source.Play();
            yield return new WaitForSecondsRealtime(1);
            _wrapper.SetActive(false);

        }
        // --------------------------------------------------------------------------------------------
    }
}