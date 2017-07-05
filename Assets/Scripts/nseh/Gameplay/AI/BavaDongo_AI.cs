using System;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Main;
using nseh.Gameplay.Entities.Enemies;

namespace nseh.Gameplay.AI
{
    public class BavaDongo_AI : MonoBehaviour
    {

        #region Public Properties

        [Header("Parameters")]
        public float percentageFrenzy;
        public float animationSpeedFrenzy;

        public AudioClip stampSound;
        public float attackPercent;
        [Space(10)]

        [Header("GameObjects")]
        public GameObject Fireball;
        public GameObject platform;
        [Space(10)]

        [Header("Sounds")]
        public List<AudioClip> stompClip;
        public List<AudioClip> genericSounds;

        [Serializable]
        public struct ParticlePosition
        {
            public GameObject particle;
            public Transform position;
        }
        [Header("Particles")]
        public List<ParticlePosition> ParticlesPositions;

        #endregion

        #region Private Properties

        private Animator _animator;
        private bool _frenzy;
        private bool _isDeath;
        private float _maxHealth;
        private float _percentage;
        private float _frenzyHealth;
        private float _animationSpeedMax;
        private float _animationSpeedMin;
        private int _stampMax;
        private int _stampNum;
        private int _stampAux;
        private Vector3 _gravity;

        #endregion

        #region Public C# Properties

        public bool IsDeath
        {
            set
            {
                _isDeath = value;
            }
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _isDeath = false;        
            _frenzy = false;
            _animator.speed = 1;
            _animationSpeedMax = 1;
            _animationSpeedMin = 1;       
            _maxHealth = GetComponent<EnemyHealth>().MaxHealth;
            _stampMax = 3;
            _stampNum = 0;
            _stampAux = UnityEngine.Random.Range(1, _stampMax);
            _percentage = 100;
            _frenzyHealth = GetComponent<EnemyHealth>().MaxHealth * percentageFrenzy;
            _gravity = Physics.gravity;
        }

        public void Update()
        {
            float _health = GetComponent<EnemyHealth>().CurrentHealth;
            float percentageAux = (_health / _maxHealth) * 100;

            if ((_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Fall")) && _health <= 0 && _isDeath == false)
            {
                _animator.speed = 1;
                _isDeath = true;
                _animator.SetTrigger("Death");
                GameManager.Instance.BossEvent.death = true;
            }

            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle") && _health <= _frenzyHealth && _frenzy == false)
            { 
                _animator.SetTrigger("Frenzy");
                _animationSpeedMax = 1.5f;
                _animationSpeedMin = 0.5f;
                _frenzy = true;
                _animator.speed = _animationSpeedMax;              
            }

            else if((_percentage - percentageAux) >= attackPercent)
            {
                _stampNum = 0;
                _stampAux = UnityEngine.Random.Range(3, _stampMax);
                float randomNum = UnityEngine.Random.value;
                if (randomNum <= Mathf.Clamp((_health / _maxHealth) * 100, 0.25f, 0.75f))
                {
                    _animator.speed = _animationSpeedMax;
                    _animator.SetTrigger("Tantrum");
                    _percentage = (_health / _maxHealth) * 100;
                }

                else
                {
                    _animator.speed = 1;
                    _animator.SetTrigger("Fall");
                    _percentage = (_health / _maxHealth) * 100;
                }  
            }
        }

        #endregion

        #region Private Methods

        private void SelectAttack()
        {
            float randomNum = UnityEngine.Random.value;
            if(randomNum < 0.33f)
            {
                _animator.speed = _animationSpeedMax;
                _animator.SetTrigger("Scepter");
            }

            else if (randomNum < 0.66f)
            {
                
                _animator.SetTrigger("Platform");
            }

            else
            {
                _animator.speed = _animationSpeedMax;
                _animator.SetTrigger("FireBall");
            }
        }

        private void EventSelectAttack(AnimationEvent animationEvent)
        {
            SelectAttack();
        }

        private void EventSummonFireBall(AnimationEvent animationEvent)
        {
            float _health = GetComponent<EnemyHealth>().CurrentHealth;
            GameObject fireBall = Instantiate(Fireball);
            float randomNum = UnityEngine.Random.value;
            if (randomNum <= 0.5f)
            {
                fireBall.transform.eulerAngles = new Vector3(-35,-75,0);
                fireBall.GetComponent<Rigidbody>().AddForce(new Vector3(100000, 0, 0));
                fireBall.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, -10000000));
            }
                
            else
            {
                fireBall.transform.eulerAngles = new Vector3(-35, 75, 0);
                fireBall.GetComponent<Rigidbody>().AddForce(new Vector3(-100000, 0, 0));
                fireBall.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, 10000000));
            }

            Destroy(fireBall, 2.5f);
            _percentage = (_health / _maxHealth) * 100;
        }

        private void EventGoToIdle(AnimationEvent animationEvent)
        {
            float _health = GetComponent<EnemyHealth>().CurrentHealth;
            _animator.SetTrigger("Idle");
            _percentage = (_health / _maxHealth) * 100;
        }

        private void ActivateSound(int index)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(genericSounds[index], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        private void EventInclinePlatform(AnimationEvent animationEvent)
        {
            platform.GetComponent<Animator>().SetTrigger("Incline");
            _animator.speed = _animationSpeedMin;
            GameManager.Instance.SoundManager.PlayAudioFX(genericSounds[6], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            Physics.gravity = new Vector3 (-75, Physics.gravity.y, Physics.gravity.z);
        }

        private void EventResetPlatform(AnimationEvent animationEvent)
        {
            float _health = GetComponent<EnemyHealth>().CurrentHealth;
            platform.GetComponent<Animator>().SetTrigger("Reset");
            _animator.speed = _animationSpeedMax;
            Physics.gravity = _gravity;
            _percentage = (_health / _maxHealth) * 100;
        }

        private void EventSelectStamp(AnimationEvent animationEvent)
        {
            _stampNum++;
            if (_stampNum <= _stampAux)
            {
                _animator.speed = _animationSpeedMax;
                float randomNum = UnityEngine.Random.value;
                if (randomNum <= 0.5f)
                    _animator.SetTrigger("LeftStamp");

                else
                    _animator.SetTrigger("RightStamp");
            }

            else
            {
                _stampNum = 0;
                _stampAux = UnityEngine.Random.Range(3, _stampMax);
                SelectAttack();    
            }
        }

        public void ActivateParticle(int index)
        {
            GameObject particleGameObject = Instantiate(ParticlesPositions[index].particle, ParticlesPositions[index].position.position, ParticlesPositions[index].position.rotation, ParticlesPositions[index].position);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 5f);

        }

        public void ActivateSomeParticle(int index)
        {
            GameObject particleGameObject = Instantiate(ParticlesPositions[index].particle, ParticlesPositions[index].position.position, ParticlesPositions[index].position.rotation, ParticlesPositions[index].position);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

           Destroy(particleGameObject, 5f);

        }

        #endregion

    }
}
