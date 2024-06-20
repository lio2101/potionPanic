using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
{
    public class Customer : MonoBehaviour, IInteractable
    {
        // --- Enums ------------------------------------------------------------------------------------------------------		

        private enum CustomerState
        {
            None,
            Entering,
            Waiting,
            Leaving
        }

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Image _icon;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SO_RecipeCollection _potions;

        [SerializeField] private AudioClip _enterShop;
        [SerializeField] private AudioClip _payItem;

        private AudioSource _audioSource;

        private CustomerState _state;
        private CustomerController _controller;
        private SO_PotionData _potionOrder;
        private Team _team;
        private bool _receivedOrder;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool IsEntering => _state == CustomerState.Entering;
        public bool IsWaiting => _state == CustomerState.Waiting;
        public bool IsLeaving => _state == CustomerState.Leaving;
        public bool ReceivedOrder => _receivedOrder;

        public Team Team { get { return _team; } set { _team = value; } }
        // --- Unity Functions -----------------------------------------------------------------------------------------------
        void Start()
        {
            _potionOrder = _potions.Recipes.GetRandomElement().PotionData;
            _icon.sprite = _potionOrder.Icon;

            _state = CustomerState.Entering;
            _canvas = GetComponentInChildren<Canvas>();
            _icon = GetComponentInChildren<Image>();
            this.GetComponent<BoxCollider>().enabled = false;
            _canvas.enabled = false;

            _audioSource = GetComponentInChildren<AudioSource>();
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        public void SetCustomerController(CustomerController controller)
        {
            _controller = controller;
        }

        public bool TryGivePotion(SO_PotionData potion)
        {
            if(_potionOrder.Type == potion.Type)
            {
                StopCoroutine(CountDown());
                Debug.Log("Force Stop Timer");
                _state = CustomerState.Leaving;
                _receivedOrder = true;
                _canvas.enabled = false;

                _team.AddPoint();

                _audioSource.clip = _payItem;
                _audioSource.Play();
                return true;
            }

            return false;
        }

        public void Wait()
        {
            _audioSource.clip = _enterShop;
            _audioSource.Play();
            _canvas.enabled = true;
            this.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(CountDown());
        }

        public IEnumerator CountDown()
        {
            Debug.Log("StartTimer");
            _state = CustomerState.Waiting;
            yield return new WaitForSeconds(GameManager.ORDER_TIME);
            _state = CustomerState.Leaving;
            _canvas.enabled = false;
            Debug.Log("Stop Timer");
        }

        public void PlayPaySound()
        {
            _audioSource.clip = _payItem;
            _audioSource.Play();
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}