using nseh.Managers.General;
using UnityEngine;
using System.Collections.Generic;
using nseh.Managers.Audio;

namespace nseh.Gameplay.Entities.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        #region Private Properties

        [SerializeField]
        private float _maxHealth;
        [SerializeField]
        private float _currentHealth;
        [SerializeField]
        private BarComponent _lifeBar;
        [SerializeField]
        private List<Collider> _colliders;


        #endregion

        #region Public Properties

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

        public void TakeDamage(int amount)
        {
            // Reduce current health
            CurrentHealth -= amount;
            SoundManager.Instance.PlayAudioFX(hitClip[UnityEngine.Random.Range(0, hitClip.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);

            // Clamp current health
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }

        public void ActivateHitbox(int index)
        {
            _colliders[index].enabled = true;
        }

        /// <summary>
        /// Deactivate the collider. This event is triggered by the animation.
        /// </summary>
        /// <param name="index">The weapon to be deactivated.</param>
        public void DeactivateHitbox(int index)
        {
            _colliders[index].enabled = false;
        }

        #endregion
    }
}
