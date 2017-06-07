using System.Collections;
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
        public float frenzyHealth;
        public AudioClip stampSound;
        public AudioClip deathSound;
        public AudioClip startSound;
        public AudioClip rollSound;
        public AudioClip frenzySound;
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
        private float _animationSpeed;

        public bool IsDeath
        {
            get
            {
                return _isDeath;
            }

            set
            {
                _isDeath = value;
            }
        }
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
            
            _frenzy = false;
            _wait = 2;
            _animator.speed = 1;
            _animationSpeed = 1;
            _agent.enabled = false;
            _animator.SetBool("Appear", true);
            _animator.SetTrigger("Appear");
            AudioSource.PlayClipAtPoint(startSound, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
            _throne = GameObject.Find("throne");
            maxPlayers = GameObject.Find("GameManager").GetComponent<GameManager>()._numberPlayers;
            //_animator.SetBool("Walk", true);
            _health = GetComponent<EnemyHealth>().CurrentHealth;
            
            if (Random.Range(0.0f, 1.0f) >= 0.6f)
            {
                _nextPoint = left_Limit;

            }
            else
            {
                _nextPoint = right_Limit;

            }
            _isDice = true;
            _animator.SetBool("Walk", true);

        }

        // Update is called once per frame
        public void Update()
        {
            _health = GetComponent<EnemyHealth>().CurrentHealth;
            if (_health <= 0 && _isDeath == false)
            {
                _isDeath = true;
                _agent.enabled = false;
                _nextPoint = null;
                _animator.SetTrigger("Death");
                AudioSource.PlayClipAtPoint(deathSound, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
                _animator.SetBool("Walk", false);
                _animator.SetBool("AttackRoll", false);
            }
            else
            {
                if (_health <= frenzyHealth && _frenzy == false)
                {
                    _animator.SetBool("Walk", false);
                    _animator.SetBool("AttackRoll", false);
                    _agent.enabled = false;
                    //_isDice = false;
                    _frenzy = true;
                   _animator.SetTrigger("Frenzy");
                    AudioSource.PlayClipAtPoint(frenzySound, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
                    _prob_Patrol_in = 0.25f;
                    _prob_AttackSpikes_in = 1f;
                    _agent.speed = agentSpeedFrenzy;
                    _animationSpeed = animationSpeedFrenzy;
                    //acelerar
                    _wait = 1;
                    _isDice = false;

                }
                //Debug.Log("0 " + animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")+ " "+isDice);
                if (Mathf.Abs(this.gameObject.transform.position.x - _nextPoint.transform.position.x) <= 1f && _isDeath == false)
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
                    _animator.speed = _animationSpeed;
                    if (_isDice == false)
                    {
                        Debug.Log("3 " + " " + _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"));
                        _isDice = true;
                        Invoke("SelectAttack", _wait);
                    }

                }else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk2"))
                {
                    Debug.Log("WALK2");
                    _animator.speed = _animationSpeed;
                    _agent.enabled = false;
                }else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    Debug.Log("WALK1");
                    _animator.speed = 1;
                    _agent.enabled = true;
                    _agent.SetDestination(_nextPoint.position);
                }
            }
        }

        #endregion

        #region Private Methods
        private void SelectAttack()
        {          
            _dice = Random.Range(0.0f, 1.0f);
            numPlayers = throne.GetComponent<Throne>().players_throne;
            float prob_AttackRoll = _prob_AttackSpikes_in - ((_prob_AttackSpikes_in - _prob_Patrol_in) * numPlayers / maxPlayers);
            Debug.Log("Dice " + _dice + " " + _prob_AttackSpikes_in + " "+ _prob_Patrol_in+" "+ numPlayers+" " +maxPlayers);
            float angle = 100;
            Debug.Log(Vector3.Angle(this.transform.forward, _nextPoint.transform.position - this.transform.position));
            if (Vector3.Angle(this.transform.forward, _nextPoint.transform.position - this.transform.position) > angle)
            {
                this.gameObject.transform.Rotate(0, 180, 0);
            }

            if (_dice < _prob_Patrol_in)
            {
                _animator.SetBool("Walk", true);
                //funcion caminar
                Debug.Log("PATROL");
                //_agent.enabled = false;
                _agent.SetDestination(_nextPoint.position);
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
                if(_nextPoint == right_Limit)
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

        public virtual void StampSound(AnimationEvent animationEvent)
        {
            AudioSource.PlayClipAtPoint(stampSound, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
        }

        public virtual void RollSound(AnimationEvent animationEvent)
        {
            AudioSource.PlayClipAtPoint(rollSound, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
        }
        #endregion

    }
}
