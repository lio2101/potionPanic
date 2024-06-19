using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
{
	public class TrashInteractable : MonoBehaviour, IInteractable
    {
		// --- Enums ------------------------------------------------------------------------------------------------------

		// --- Fields -----------------------------------------------------------------------------------------------------
		[SerializeField] private AudioClip _trashSound;
		 private AudioSource _audioSource;
		// --- Properties -------------------------------------------------------------------------------------------------

		// --- Unity Functions --------------------------------------------------------------------------------------------
		void Start ()
		{
			_audioSource = GetComponent<AudioSource>();
		}
		// --- Event callbacks --------------------------------------------------------------------------------------------

		// --- Public/Internal Methods ------------------------------------------------------------------------------------
		public void PlayTrashSound()
		{
			_audioSource.clip = _trashSound;
			_audioSource.Play();
		}
		// --- Protected/Private Methods ----------------------------------------------------------------------------------
		
		// --------------------------------------------------------------------------------------------
	}
}