using nseh.Managers.General;
using UnityEngine;
using System.Collections.Generic;
using nseh.Managers.Main;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Entities.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {

        #region Private Properties

        [Header ("Health")]
        [SerializeField]
        private float _maxHealth;
        [SerializeField]
        private float _currentHealth;
        [SerializeField]
        private BarComponent _lifeBar;
        [Space (10)]

        [Header("HitBox")]
        [SerializeField]
        private List<Collider> _hitBox;
        [Space(10)]

        #endregion

        #region Public Properties

        [Header("Audio")]
        public List<AudioClip> hitClip;

        #endregion

        #region Public Properties

        public float CurrentHealth
        {
            get { return _currentHealth; }

            set 
            { 
                _currentHealth = value;

                if (_lifeBar != null)
                {
                    _lifeBar.Value = _currentHealth;
                }
            }
        }

        public float MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;

				if (_lifeBar != null)
				{
					_lifeBar.MaxValue = _maxHealth;
				}
            }
        }

        #endregion

        #region Public Methods

        public void Awake()
        {
            MaxHealth = GameManager.Instance._numberPlayers * 100;
            CurrentHealth = MaxHealth;
            _lifeBar.Value = MaxHealth;
        }

        public void TakeDamage(float amount, PlayerInfo sender, Vector3 position)
        {
            CurrentHealth -= amount;
            GameManager.Instance.SoundManager.PlayAudioFX(hitClip[Random.Range(0, hitClip.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            sender.IncreaseEnergy(amount / 2);
            sender.IncreaseScore((int)amount);
            GameObject particleGameObject = Instantiate(sender.HitParticle, position, sender.HitParticle.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 1f);
        }

        public void ActivateHitbox(int index)
        {
            _hitBox[index].enabled = true;
        }

        public void DeactivateHitbox(int index)
        {
            _hitBox[index].enabled = false;
        }

        #endregion

    }
}
