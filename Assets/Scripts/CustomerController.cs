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


            public void CreateCustomer(Customer prefab, Team team, CustomerController controller)
            {
                customer = Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                customer.Team = team;
                customer.SetCustomerController(controller);
                count++;
                Debug.Log($"New customer team {team}");
            }

        }

        // --- Enums ------------------------------------------------------------------------------------------------------

        // --- Fields -----------------------------------------------------------------------------------------------------
        [SerializeField] private Customer[] _customerPrefabs;
        [SerializeField] private CustomerData _customerDataTeam1;
        [SerializeField] private CustomerData _customerDataTeam2;
        [SerializeField] private float _moveSpeed = 2;
        [SerializeField] private GameMenu _gameMenu;
        [SerializeField] TeamManager _teamManager;

        public delegate void ScoreChangedEvent(int team);
        public ScoreChangedEvent ScoreChanged;

        private GameManager _gameManager;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------
        private void Start()
        {
            _teamManager = TeamManager.Instance;
            _gameManager = GameManager.Instance;

            foreach(Team team in _teamManager.Teams)
            {
                CreateCustomer(team);
            }
        }

        void LateUpdate()
        {
            MoveCustomer(_customerDataTeam1);
            MoveCustomer(_customerDataTeam2);
        }

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public Vector3 Move(Vector3 current, Vector3 target)
        {
            return Vector3.MoveTowards(current, target, _moveSpeed * Time.deltaTime);
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        private void CreateCustomer(Team team)
        {
            CustomerData data = team.Index == 0 ? _customerDataTeam1 : _customerDataTeam2;
            data.CreateCustomer(_customerPrefabs.GetRandomElement(), team, this);
        }

        private void MoveCustomer(CustomerData data)
        {
            Customer customer = data.customer;


            if(customer != null)
            {
                if(customer.IsEntering)
                {
                    MoveTowards(data.TargetPos, OnTargetReached);
                    customer.transform.LookAt(data.SpawnPos);
                    void OnTargetReached()
                    {
                        customer.IsEntering = false;
                        customer.Wait();
                    }
                }
                else if(customer.IsLeaving)
                {
                    MoveTowards(data.SpawnPos, OnSpawnReached);
                    customer.transform.LookAt(data.TargetPos);
                    void OnSpawnReached()
                    {
                        Team currentTeam = customer.Team;
                        Destroy(customer.gameObject);
                        if(data.count < GameManager.ORDERS_PER_ROUND)
                        {
                            data.CreateCustomer(_customerPrefabs.GetRandomElement(), currentTeam, this);
                        }
                        else
                        {
                            _gameMenu.ShowFinalScore(_teamManager.CalculateWinner());
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