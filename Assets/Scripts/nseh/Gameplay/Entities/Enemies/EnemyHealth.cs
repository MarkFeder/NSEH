using nseh.Gameplay.Base.Interfaces;
using nseh.Managers.General;
using UnityEngine;

namespace nseh.Gameplay.Entities.Enemies
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        #region Private Properties

        [SerializeField]
        private int _maxHealth;
        [SerializeField]
        private int _currentHealth;

        private bool _isDead;
        private int _deathCounter;

        [SerializeField]
        private BarComponent _lifeBar;

        #endregion

        #region Public Properties

        public int CurrentHealth
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

        public int MaxHealth
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

        public bool IsDead
        {
            get
            {
                return _isDead;
            }

        }

        #endregion

        #region Private Methods

        private void Start()
        {
            // TODO: reference here to enemy info

            _deathCounter = 0;
            _isDead = false;

        }

        private void Update()
        {
           
        }

        private void Death()
        {
            _deathCounter++;
            //_currentHealth = _maxHealth;
            _isDead = true;

            Debug.Log(string.Format("[{0}] has died: {1} times", gameObject.name, _deathCounter));
        }

        #endregion


        #region Public Methods

        public void TakeDamage(int amount)
        {
            // Reduce current health
            CurrentHealth -= amount;

            // Play hit animation


            // Clamp current health
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            if (CurrentHealth == 0 && !IsDead)
            {
                Death();
            }
        }

        #endregion
    }
}
