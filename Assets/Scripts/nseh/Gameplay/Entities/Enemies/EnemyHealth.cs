using nseh.Gameplay.Base.Interfaces;
using UnityEngine;

namespace nseh.Gameplay.Entities.Enemies
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        #region Private Properties

        [SerializeField]
        private int _startingHealth;
        [SerializeField]
        private int _maxHealth;
        private int _currentHealth;

        private bool _isDead;
        private int _deathCounter;

        #endregion

        #region Public Properties

        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            // TODO: reference here to enemy info

            _deathCounter = 0;
            _currentHealth = _startingHealth;
            _isDead = false;
        }

        private void Update()
        {
            Debug.Log("Current health of: " + gameObject.name + " is: " + _currentHealth);
        }

        private void Death()
        {
            _deathCounter++;
            _currentHealth = _maxHealth;
            _isDead = true;

            Debug.Log(string.Format("[{0}] has died: {1} times", gameObject.name, _deathCounter));
        }

        #endregion


        #region Public Methods

        public void TakeDamage(int amount)
        {
            // Reduce current health
            _currentHealth -= amount;

            // Play hit animation


            // Clamp current health
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            if (_currentHealth == 0 && !_isDead)
            {
                Death();
            }
        }

        #endregion
    }
}
