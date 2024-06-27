using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
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
            //[SerializeField] private Transform _windowPosition;
            //[SerializeField] private int _team;

            [NonSerialized] public Customer customer;
            [NonSerialized] public int count;

            public Transform SpawnPos => _spawnPosition;
            public Transform TargetPos => _targetPosition;

            //public Vector3 WindowPos => _windowPosition.position;


            public void CreateCustomer(Customer prefab, Team team, CustomerController controller)
            {
                customer = Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                customer.Team = team;
                //customer.SetCustomerController(controller);
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

        public delegate void ScoreChangedEvent(int team);
        public ScoreChangedEvent ScoreChanged;

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Unity Functions --------------------------------------------------------------------------------------------

        private void Awake()
        {
            //GameManager.Instance.RoundFinished += Reset;
        }
        private void Start()
        {
            foreach(Team team in TeamManager.Instance.Teams)
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
        //private void Reset()
        //{
        //    Destroy(_customerDataTeam1.customer.gameObject);
        //    Destroy(_customerDataTeam2.customer.gameObject);
        //    Start();
        //}


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
                customer.transform.rotation = data.TargetPos.rotation;
                if(customer.IsEntering)
                {
                    MoveTowards(data.TargetPos.position, OnTargetReached);

                    void OnTargetReached()
                    {

                        customer.Wait();
                    }
                }
                else if(customer.IsLeaving)
                {
                    MoveTowards(data.SpawnPos.position, OnSpawnReached);
                    void OnSpawnReached()
                    {
                        Team currentTeam = customer.Team;
                        Destroy(customer.gameObject);

                        data.count++;

                        if(data.count < GameManager.ORDERS_PER_ROUND)
                        {
                            data.CreateCustomer(_customerPrefabs.GetRandomElement(), currentTeam, this);
                        }
                        else
                        {

                            GameManager.Instance.EndRound();
                            _gameMenu.ShowFinalScore(TeamManager.Instance.CalculateWinner());
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