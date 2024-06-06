using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace LJ
{
    public class CustomerController : MonoBehaviour
    {
        [Serializable]
        public class CustomerData
        {
            [SerializeField] private Transform _targetPosition;
            [SerializeField] private Transform _spawnPosition;
            
            [NonSerialized] public Customer customer;
            [NonSerialized] public int count;

            public Vector3 SpawnPos => _spawnPosition.position;
            public Vector3 TargetPos => _targetPosition.position;

            public void CreateCustomer(Customer prefab)
            {
                customer = Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                count++;
            }

        }

        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Customer[] _customerPrefabs;
        [SerializeField] private CustomerData[] _customerDatas = new CustomerData[2];
        [SerializeField] private float _moveSpeed = 2;

        const int ORDERS_PER_ROUND = 10;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            foreach(CustomerData customerData in _customerDatas)
            {
                customerData.CreateCustomer(_customerPrefabs.GetRandomElement());
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

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public Vector3 Move(Vector3 current, Vector3 target)
        {
            return Vector3.MoveTowards(current, target, _moveSpeed * Time.deltaTime);
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------
        private void MoveCustomer(CustomerData data)
        {
            Customer customer = data.customer;
            
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
                    Destroy(customer.gameObject);
                    if(data.count < ORDERS_PER_ROUND)
                    {
                        data.CreateCustomer(_customerPrefabs.GetRandomElement());
                        Debug.Log("Created new customer");
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