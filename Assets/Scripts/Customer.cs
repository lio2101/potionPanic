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

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Image _icon;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SO_RecipeCollection _potions;

        private SO_PotionData _potionOrder;
        private int _team;
        private bool _isEntering, _isWaiting, _isLeaving;

        public delegate void ScoredEvent(int team);
        public event ScoredEvent Scored;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool IsEntering { get { return _isEntering; } set {  _isEntering = value; } }
        public bool IsWaiting { get { return _isWaiting;} set { _isWaiting = value; } }
        public bool IsLeaving { get { return _isLeaving; } set { _isLeaving = value; } }

        public int Team { get { return _team; } set { _team = value; } }
        // --- Unity Functions -----------------------------------------------------------------------------------------------
        void Start()
        {
            _potionOrder = _potions.Recipes.GetRandomElement().PotionData;
            _icon.sprite = _potionOrder.Icon;

            _isEntering = true;
            _canvas = GetComponentInChildren<Canvas>();
            _icon = GetComponentInChildren<Image>();
            this.GetComponent<BoxCollider>().enabled = false;
            _canvas.enabled = false;


        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        public bool TryGivePotion(SO_PotionData potion)
        {
            if(_potionOrder.Type == potion.Type)
            {
                StopCoroutine(CountDown());
                Debug.Log("Force Stop Timer");
                _isWaiting = false;
                _isLeaving = true;
                _canvas.enabled = false;

                Scored?.Invoke(_team);

                return true;
            }

            return false;
        }

        public void Wait()
        {
            _canvas.enabled = true;
            this.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(CountDown());
        }

        public IEnumerator CountDown()
        {
            Debug.Log("StartTimer");
            _isWaiting = true;
            yield return new WaitForSeconds(GameManager.ORDER_TIME);
            _isWaiting = false;
            _isLeaving = true;
            _canvas.enabled = false;
            Debug.Log("Stop Timer");
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}