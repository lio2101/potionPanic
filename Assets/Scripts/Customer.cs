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
        [SerializeField] private SO_PotionData _potionOrder;
        [SerializeField] private int timer;
        [SerializeField] private Image _icon;

        private bool _isEntering, _isWaiting, _isLeaving;

        const int ORDERTIME = 60;

        // --- Properties -------------------------------------------------------------------------------------------------
        public bool IsEntering { get { return _isEntering; } set {  _isEntering = value; } }
        public bool IsWaiting { get { return _isWaiting;} set { _isWaiting = value; } }
        public bool IsLeaving { get { return _isLeaving; } set { _isLeaving = value; } }
        // --- Unity Functions -----------------------------------------------------------------------------------------------
        void Start()
        {
            //EnterShop();
            //_customerPrefab = prefab;
            _isEntering = true;
            //_icon.sprite = _potionOrder.Icon;

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------

        public void InspectPotion(SO_PotionData potion)
        {
            if(_potionOrder.Type == potion.Type)
            {
                StopCoroutine(CountDown());
                Debug.Log("Force Stop Timer");
                _isLeaving = true;
            }
        }

        public IEnumerator CountDown()
        {
            Debug.Log("StartTimer");
            _isWaiting = true;
            yield return new WaitForSeconds(ORDERTIME);
            _isWaiting = false;
            _isLeaving = true;
            Debug.Log("Stop Timer");
        }
        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}