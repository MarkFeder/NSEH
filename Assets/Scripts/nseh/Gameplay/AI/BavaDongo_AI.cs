﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using nseh.Managers.Main;
using nseh.Gameplay.Entities.Enemies;


namespace nseh.Gameplay.AI
{
    public class BavaDongo_AI : MonoBehaviour
    {

        #region Public Properties
        public Transform left_Limit;
        public Transform right_Limit;
        public GameObject spike;
        public GameObject platform;
        public GameObject throne;   
        public float maxPlayers;
        public float numPlayers;
        public float percentageFrenzy;
        public float animationSpeedFrenzy;
        public float agentSpeedFrenzy;
        #endregion

        #region Private Properties
        private Transform _nextPoint;
        private Rigidbody _myRigidBody;
        private NavMeshAgent _agent;
        private GameObject _throne;
        private Animator _animator;
        private bool _isDice;
        private bool _frenzy;
        private bool _isDeath;
        private int _wait;
        private float _dice;
        private float _prob_Patrol_in;
        private float _prob_AttackSpikes_in;
        private float _prob_AttackRoll_in;
        private float _health;
        private float _frenzyHealth;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _nextPoint = right_Limit;
            _isDeath = false;
            _myRigidBody = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.SetDestination(_nextPoint.position);
            _prob_Patrol_in = 0.5f;
            _prob_AttackSpikes_in = 1f;
            _health = GetComponent<EnemyHealth>().CurrentHealth;
            _frenzyHealth = (GetComponent<EnemyHealth>().MaxHealth * percentageFrenzy) / 100;
            _frenzy = false;
            _wait = 2;
            _animator.speed = 1;
            _animator.SetBool("Appear", true);
            _animator.SetBool("Appear", false);
            _throne = GameObject.Find("throne");
            //maxPlayers = GameObject.Find("GameManager").GetComponent<GameManager>()._numberPlayers;
            maxPlayers = 1;
            _animator.SetBool("Walk", true);
            _isDice = true;
        }

        // Update is called once per frame
        public void Update()
        {
            _health = GetComponent<EnemyHealth>().CurrentHealth;
            if (_health <= 0 && _isDeath == false)
            {
                _isDeath = true;
                _animator.SetBool("Death", true);
            }
            else
            {
                if (_health <= _frenzyHealth && _frenzy == false)
                {
                    _agent.enabled = false;
                    _frenzy = true;
                    _animator.SetTrigger("Frenzy");
                    _prob_Patrol_in = 0.25f;
                    _prob_AttackSpikes_in = 1f;
                    _agent.speed = agentSpeedFrenzy;
                    _animator.speed = animationSpeedFrenzy;
                    //acelerar
                    _wait = 1;
                
                }
                //Debug.Log("0 " + animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")+ " "+isDice);
                if (Mathf.Abs(this.gameObject.transform.position.x - _nextPoint.transform.position.x) <= 1f)
                {
                    _animator.SetBool("Walk", false);
                    _animator.SetBool("AttackRoll", false);

                    if (_nextPoint == right_Limit)
                    {
                        _nextPoint = left_Limit;
                        _isDice = false;
                        Debug.Log("1");
                    }

                    else if (_nextPoint == left_Limit)
                    {
                        _nextPoint = right_Limit;
                        _isDice = false;
                        Debug.Log("2");
                    }         
                }

                else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"))
                {
                    _agent.enabled = true;
                    if (_isDice == false)
                    {
                        Debug.Log("3 " + " " + _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"));
                        _isDice = true;
                        Invoke("SelectAttack", _wait);
                    }

                }
            }
        }

        #endregion

        #region Private Methods
        private void SelectAttack()
        {          
            _dice = Random.Range(0.0f, 1.0f);
            numPlayers = throne.GetComponent<Throne>().players_throne;
            //numPlayers = 1;
            float prob_AttackRoll = _prob_AttackSpikes_in - ((_prob_AttackSpikes_in - _prob_Patrol_in) * numPlayers / maxPlayers);
            Debug.Log("Dice " + _dice + " " + _prob_AttackSpikes_in + " "+ _prob_Patrol_in+" "+ numPlayers+" " +maxPlayers);
            float angle = 179;

            if (Vector3.Angle(this.transform.forward, _nextPoint.transform.position - this.transform.position) > angle)
            {
                this.gameObject.transform.Rotate(0, 180, 0);
            }

            if (_dice < _prob_Patrol_in)
            {
                _animator.SetBool("Walk", true);
                //funcion caminar
                Debug.Log("PATROL");
                _agent.enabled = false;
                //_agent.SetDestination(_nextPoint.position);
                //_agent.speed = 0;
            }

            else if (_dice < prob_AttackRoll)
            {             
                _animator.SetBool("AttackRoll", true);
                //funcion rodar
                Debug.Log("ATTACKROLL");
                _agent.enabled = true;
                _agent.SetDestination(_nextPoint.position);
            }

            else
            {
                _animator.SetTrigger("AttackSpikes");
                _isDice = false;
                //funcion pinchos
                Debug.Log("ATTACKSPIKES"); 
                GameObject clone = Instantiate(spike, this.transform.position, spike.transform.rotation);
                if(_nextPoint == left_Limit)
                {
                    clone.transform.Rotate(0, 180, 0);
                    clone.GetComponent<Rigidbody>().AddForce(-8000, 8000, 0);

                }
                    
                else
                clone.GetComponent<Rigidbody>().AddForce(8000,8000,0);
                Destroy(clone, 2);

            }
        }

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
        #endregion

    }
}
