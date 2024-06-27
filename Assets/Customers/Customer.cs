using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
{
    [Serializable]
    public class PotionOrder
    {

        public SO_PotionData potionData;
        public Image potionImage;
        public bool hasReceived;

    }

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
        [SerializeField] private Canvas _canvasOrder;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;
        [SerializeField] private Image _orderImagePrefab;

        [SerializeField] private PotionOrder[] _orders;

        [SerializeField] private SO_RecipeCollection _potions;

        [SerializeField] private AudioClip _enterShop;
        [SerializeField] private AudioClip _payItem;

        private Coroutine _countdownRoutine;
        private AudioSource _audioSource;
        private CustomerState _state;
        private Team _team;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool IsEntering => _state == CustomerState.Entering;
        public bool IsWaiting => _state == CustomerState.Waiting;
        public bool IsLeaving => _state == CustomerState.Leaving;

        public Team Team { get { return _team; } set { _team = value; } }
        // --- Unity Functions -----------------------------------------------------------------------------------------------
        void Start()
        {
            int playerCount = Mathf.Max(1, TeamManager.Instance.CurrentPlayerCount);
            _orders = new PotionOrder[Mathf.CeilToInt(playerCount / 2f)];
            _state = CustomerState.Entering;

            this.GetComponent<BoxCollider>().enabled = false;

            _canvasOrder.enabled = false;

            _audioSource = GetComponentInChildren<AudioSource>();

            for(int i = 0; i < _orders.Length; i++)
            {
                _orders[i] = new();
                Image orderImage = Instantiate(_orderImagePrefab, _layoutGroup.transform);
                _orders[i].potionImage = orderImage;

                _orders[i].potionData = _potions.Recipes.GetRandomElement().PotionData;
                _orders[i].potionImage.sprite = _orders[i].potionData.Icon;
            }
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------


        public bool TryGivePotion(SO_PotionData potion)
        {
            PotionOrder order = _orders.FirstOrDefault(o => o.potionData == potion);

            if(order != null)
            {
                _audioSource.clip = _payItem;
                _audioSource.Play();

                order.potionImage.SetAlpha(0.5f);
                order.potionData = null;
                order.hasReceived = true;

                bool allOrdersReceived = _orders.All(o => o.hasReceived);
                if(allOrdersReceived)
                {
                    StopCoroutine(_countdownRoutine);
                    _state = CustomerState.Leaving;
                    _canvasOrder.enabled = false;
                    _team.AddPoint();
                    Debug.Log("All orders received!");
                }
            }
            return order != null;
        }

        public void Wait()
        {
            _audioSource.clip = _enterShop;
            _audioSource.Play();

            _canvasOrder.enabled = true;

            this.GetComponent<BoxCollider>().enabled = true;
            _countdownRoutine = StartCoroutine(CountDown());
        }

        public IEnumerator CountDown()
        {
            Debug.Log("StartTimer");
            _state = CustomerState.Waiting;
            yield return new WaitForSeconds(GameManager.ORDER_TIME);
            _state = CustomerState.Leaving;
            _canvasOrder.enabled = false;
            Debug.Log("Stop Timer");
            _countdownRoutine = null;
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