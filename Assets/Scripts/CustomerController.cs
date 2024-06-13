using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static LJ.CustomerController;

namespace LJ
{
    public class CustomerController : MonoBehaviour
    {
        [Serializable]
        public class CustomerData
        {
            [SerializeField] private Transform _targetPosition;
            [SerializeField] private Transform _spawnPosition;
            //[SerializeField] private int _team;

            [NonSerialized] public Customer customer;
            [NonSerialized] public int count;

            public Vector3 SpawnPos => _spawnPosition.position;
            public Vector3 TargetPos => _targetPosition.position;


            public void CreateCustomer(Customer prefab, int team)
            {
                customer = Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                customer.Team = team;
                count++;
                Debug.Log($"New customer team {team}");
            }

        }

        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Customer[] _customerPrefabs;
        [SerializeField] private CustomerData[] _customerDatas = new CustomerData[2];
        [SerializeField] private float _moveSpeed = 2;

        public delegate void ScoreChangedEvent(int team);
        public ScoreChangedEvent ScoreChanged;

        private GameManager _gameManager;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _gameManager = GameManager.Instance;
            int index = 0;
            foreach(CustomerData customerData in _customerDatas)
            {
                customerData.CreateCustomer(_customerPrefabs.GetRandomElement(), index);
                customerData.customer.Scored += OnScored;
                index++;
            }
        }

        private void OnDisable()
        {
            foreach(CustomerData customerData in _customerDatas)
            {
                customerData.customer.Scored -= OnScored;
            }

        }

        void LateUpdate()
        {
            foreach(CustomerData customerData in _customerDatas)
            {
                MoveCustomer(customerData);
            };
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------
        private void OnScored(int index)
        {
            Debug.Log("CustomerController on scored");
            ScoreChanged?.Invoke(index);
        }

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public Vector3 Move(Vector3 current, Vector3 target)
        {
            return Vector3.MoveTowards(current, target, _moveSpeed * Time.deltaTime);
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void MoveCustomer(CustomerData data)
        {
            Customer customer = data.customer;

            if(customer != null)
            {
                if(customer.IsEntering)
                {
                    MoveTowards(data.TargetPos, OnTargetReached);

                    void OnTargetReached()
                    {
                        customer.IsEntering = false;
                        customer.Wait();
                    }
                }
                else if(customer.IsLeaving)
                {
                    MoveTowards(data.SpawnPos, OnSpawnReached);

                    void OnSpawnReached()
                    {
                        int currentTeam = customer.Team;
                        customer.Scored -= OnScored;
                        Destroy(customer.gameObject);
                        if(data.count < GameManager.ORDERS_PER_ROUND)
                        {
                            data.CreateCustomer(_customerPrefabs.GetRandomElement(), currentTeam);
                            data.customer.Scored += OnScored;
                        }
                        else 
                        { 
                            _gameManager.EndRound();
                        }
                    }
                }
            }

            void MoveTowards(Vector3 targetPos, Action onTargetReached)
            {
                customer.transform.position = Move(customer.transform.position, targetPos);

                if(Mathf.Approximately(customer.transform.position.x, targetPos.x)
                    && Mathf.Approximately(customer.transform.position.z, targetPos.z))
                {
                    onTargetReached?.Invoke();
                }
            }
        }

        // --------------------------------------------------------------------------------------------
    }
}