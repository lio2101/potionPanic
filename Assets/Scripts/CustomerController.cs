using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace LJ
{
    public class CustomerController : MonoBehaviour, IInteractable
    {
        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        //[SerializeField] private GameObject[] _customersTeam1 = new GameObject[ORDERSPERROUND];
        //[SerializeField] private GameObject[] _customersTeam2 = new GameObject[ORDERSPERROUND];

        [SerializeField] private Transform _windowTeam1;
        [SerializeField] private Transform _windowTeam2;

        [SerializeField] private Customer[] _customerPrefabs;
        [SerializeField] private int _offset = 3;
        [SerializeField] private float _moveSpeed = 2;

        const int ORDERSPERROUND = 10;

        private Vector3 _spawnPosition1;
        private Vector3 _spawnPosition2;

        private Customer _customerTeam1;
        private Customer _customerTeam2;


        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _spawnPosition1 = new Vector3(_windowTeam1.position.x - _offset, _windowTeam1.position.y + 0.5f, _windowTeam1.position.z);
            _spawnPosition2 = new Vector3(_windowTeam2.position.x + _offset, _windowTeam2.position.y + 0.5f, _windowTeam2.position.z);
            _customerTeam1 = CreateCustomer(_spawnPosition1);
            _customerTeam2 = CreateCustomer(_spawnPosition2);
        }

        void LateUpdate()
        {
            if(_customerTeam1.IsEntering)
            {
                float directionToMoveX = _windowTeam1.position.x - _customerTeam1.transform.position.x;
                directionToMoveX = Mathf.Sign(directionToMoveX) * Time.deltaTime * _moveSpeed;

                float maxDistanceX = Mathf.Abs(_windowTeam1.position.x - _customerTeam1.transform.position.x);

                _customerTeam1.transform.position = new Vector3(
                    Mathf.MoveTowards(_customerTeam1.transform.position.x, _windowTeam1.position.x, Mathf.Min(maxDistanceX, directionToMoveX)),
                    _customerTeam1.transform.position.y,
                    _customerTeam1.transform.position.z
                );
            }
            else if(_customerTeam1.IsLeaving)
            {
                _customerTeam1.transform.position = Vector3.MoveTowards(_spawnPosition1, _windowTeam1.position, Time.deltaTime * _moveSpeed);
            }

            if(_customerTeam2.IsEntering)
            {
                float directionToMoveX = _windowTeam2.position.x - _customerTeam2.transform.position.x;
                directionToMoveX = Mathf.Sign(directionToMoveX) * Time.deltaTime * _moveSpeed;

                float maxDistanceX = Mathf.Abs(_windowTeam2.position.x - _customerTeam2.transform.position.x);

                _customerTeam2.transform.position = new Vector3(
                    Mathf.MoveTowards(_customerTeam2.transform.position.x, _windowTeam2.position.x, Mathf.Min(maxDistanceX, directionToMoveX)),
                    _customerTeam2.transform.position.y,
                    _customerTeam2.transform.position.z
                    );
            }
            else if(_customerTeam2.IsLeaving)
            {
                _customerTeam2.transform.position = Vector3.MoveTowards(_customerTeam2.transform.position, _windowTeam2.position, Time.deltaTime * _moveSpeed);
            }

        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public Customer CreateCustomer(Vector3 spawnPos)
        {
            Customer prefab = _customerPrefabs.GetRandomElement();
            return Instantiate(prefab, spawnPos, Quaternion.identity);
        }

        //public void MoveCustomer(Transform customer, Transform )

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}