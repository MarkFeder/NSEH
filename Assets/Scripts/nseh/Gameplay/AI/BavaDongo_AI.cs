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
        public GameObject spike;
        public float numPlayers;
        public float percentageFrenzy;
        public float animationSpeedFrenzy;
        public float frenzyHealth;
        public AudioClip stampSound;
        public float attackPercent;
        public GameObject platform;

        #endregion

        #region Private Properties
        private Animator _animator;
        private bool _frenzy;
        private bool _isDeath;
        private float _health;
        private float _percentage;
        private float _animationSpeedMax;
        private float _animationSpeedMin;
        private int _stampMax;
        private int _stampNum;
        private int _stampAux;
        private Vector3 _gravity;
        [SerializeField]
        private List<Collider> _colliders;


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
            _isDeath = false;        
            _frenzy = false;
            _animator.speed = 1;
            _animationSpeedMax = 1;
            _animationSpeedMin = 1;
            _health = GetComponent<EnemyHealth>().CurrentHealth;
            _stampMax = 5;
            _stampNum = 0;
            _stampAux = Random.Range(3, _stampMax);
            _percentage = (GetComponent<EnemyHealth>().CurrentHealth / GetComponent<EnemyHealth>().MaxHealth) *100;
            _gravity = Physics.gravity;
        }

        // Update is called once per frame
        public void Update()
        {
            _health = GetComponent<EnemyHealth>().CurrentHealth;
            if ((_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Fall")) && _health <= 0 && _isDeath == false)
            {
                _isDeath = true;
                _animator.SetTrigger("Death");
                //AudioSource.PlayClipAtPoint(deathSound, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);

            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle") && _health <= frenzyHealth && _frenzy == false)
            { 
                _animator.SetTrigger("Frenzy");
                _animationSpeedMax = 1.5f;
                _animationSpeedMin = 0.5f;
                _stampMax = 10;
                _frenzy = true;
                _animator.speed = _animationSpeedMax;
                //AudioSource.PlayClipAtPoint(frenzySound, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);               
            }
        }

        #endregion

        #region Private Methods
        private void SelectAttack()
        {
            float randomNum = Random.value;
            if(randomNum < 0.33f)
            {
                _animator.speed = _animationSpeedMax;
                _animator.SetTrigger("Scepter");
            }
            else if (randomNum < 0.66f)
            {
                _animator.speed = _animationSpeedMin;
                _animator.SetTrigger("Platform");
            }
            else
            {
                _animator.speed = _animationSpeedMax;
                _animator.SetTrigger("FireBall");
            }

            _percentage = (GetComponent<EnemyHealth>().CurrentHealth / GetComponent<EnemyHealth>().MaxHealth) * 100;
        }

        private void EventSelectAttack(AnimationEvent animationEvent)
        {
            //_animator.SetTrigger("SelectAttack");
            SelectAttack();
        }

        private void EventSummonFireBall(AnimationEvent animationEvent)
        {
            //instantiate
            GameObject fireBall = Instantiate(spike);
            float randomNum = Random.value;
            if (randomNum <= 0.5f)
            {
                fireBall.GetComponent<Rigidbody>().AddForce(new Vector3(100000, 0, 0));
                fireBall.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, -10000000));
            }
                
            else
            {
                fireBall.GetComponent<Rigidbody>().AddForce(new Vector3(-100000, 0, 0));
                fireBall.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, 10000000));
            }

            Destroy(fireBall, 2.5f);
        }

        private void EventGoToIdle(AnimationEvent animationEvent)
        {
            _animator.SetTrigger("Idle");
        }

        private void EventInclinePlatform(AnimationEvent animationEvent)
        {
            platform.GetComponent<Animator>().SetTrigger("Incline");
            Physics.gravity = new Vector3 (-75, Physics.gravity.y, Physics.gravity.z);
        }

        private void EventResetPlatform(AnimationEvent animationEvent)
        {
            platform.GetComponent<Animator>().SetTrigger("Reset");
            _animator.speed = _animationSpeedMax;
            Physics.gravity = _gravity;

        }

        private void EventSelectStamp(AnimationEvent animationEvent)
        {
            _stampNum++;
            if (_stampNum <= _stampAux)
            {
                _animator.speed = _animationSpeedMax;
                float randomNum = Random.value;
                Debug.Log("ssss " + randomNum+ " "+ _stampNum +  " "+ _stampAux);
                if (randomNum <= 0.5f)
                    _animator.SetTrigger("LeftStamp");
                else
                    _animator.SetTrigger("RightStamp");
            }
            else
            {
                _stampNum = 0;
                _stampAux = Random.Range(3, _stampMax);
                float percentageAux = (GetComponent<EnemyHealth>().CurrentHealth / GetComponent<EnemyHealth>().MaxHealth) * 100;
                Debug.Log("Porcentaje " + _percentage + " "+ percentageAux);
                if ((_percentage - percentageAux)>= attackPercent) 
                {
                    float randomNum = Random.value;
                    if (randomNum <= Mathf.Clamp((_health / GetComponent<EnemyHealth>().MaxHealth) * 100, 0.25f, 0.75f))
                    {
                        _animator.speed = _animationSpeedMax;
                        Debug.Log(randomNum + " Tantrum " + Mathf.Clamp((_health / GetComponent<EnemyHealth>().MaxHealth) * 100, 0.25f, 0.75f));
                        _animator.SetTrigger("Tantrum");
                    }


                    else
                    {
                        _animator.speed = 1;
                        Debug.Log(randomNum + " Fall " + Mathf.Clamp((_health / GetComponent<EnemyHealth>().MaxHealth) * 100, 0.25f, 0.75f));
                        _animator.SetTrigger("Fall");
                    }
                }else
                {
                    //_animator.SetTrigger("SelectAttack");
                    SelectAttack();
                }
            }
        }

        public void ActivateColliderbox(int index)
        {
            _colliders[index].enabled = true;
        }

        /// <summary>
        /// Deactivate the collider. This event is triggered by the animation.
        /// </summary>
        /// <param name="index">The weapon to be deactivated.</param>
        public void DeactivateColliderbox(int index)
        {
            _colliders[index].enabled = false;
        }

        #endregion

    }
}
