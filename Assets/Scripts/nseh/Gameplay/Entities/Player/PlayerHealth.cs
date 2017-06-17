using nseh.Gameplay.Base.Interfaces;
using nseh.Managers.General;
using nseh.Managers.Level;
using nseh.Managers.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LOGS = nseh.Utils.Constants.Logs.PlayerHealth;

namespace nseh.Gameplay.Entities.Player
{
    public enum HealthMode
    {
        Normal = 0,
        Invulnerability = 1
    }

    public class PlayerHealth : MonoBehaviour, IHealth
    {
        #region Private Properties

        [SerializeField]
        public int _startingHealth = 100;
        [SerializeField]
        public int _maxHealth = 100;
        [SerializeField]
        public int _penalization = 10;
        public List<AudioClip> hitClip;
        public AudioClip deathClip;

        private int _deathCount;
        private float _bonificationDefense;
        private float _currentHealth;

        private BarComponent _healthBar;
        private List<Image> _playerLives;
        private PlayerInfo _playerInfo;
        private HealthMode _healthMode;
        //private LevelProgress lvlProgress;

        //private int lives;
        private bool _isDead;

        #endregion

        #region Public C# Properties

        public float BonificationDefense
        {
            get { return _bonificationDefense; }
            set { _bonificationDefense = value; }
        }

        public float CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;

                if (_healthBar != null)
                {
                    _healthBar.Value = _currentHealth;
                }
            }
        }

        public HealthMode HealthMode
        {
            get { return _healthMode; }
            set { _healthMode = value; }
        }

        public BarComponent HealthBar
        {
            set { _healthBar = value; }
        }

        /*
        public LevelProgress LvlProgress
        {
            set
            {
                lvlProgress = value;
            }
        }*/

        public int StartingHealth
        {
            get { return _startingHealth; }
            set { _startingHealth = value; }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;

                if (_healthBar != null)
                {
                    _healthBar.MaxValue = _maxHealth;
                }
            }
        }

        public List<Image> PlayerLives
        {
            get { return _playerLives; }
            set { _playerLives = value; }
        }

        #endregion

        protected virtual void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();

            // Set initial health
            MaxHealth = _maxHealth;
            CurrentHealth = _startingHealth;
            //lives = 3;
            _deathCount = 0;
            _isDead = false;
        }

        protected virtual void Update()
        {
           // Debug.Log("Current health of " + gameObject.name + " is " + CurrentHealth);
        }

        #region Public Methods

        /// <summary>
        /// Restore health values to default.
        /// </summary>
        public void ResetHealth()
        {
            MaxHealth = _maxHealth;
            CurrentHealth = _startingHealth;
            _isDead = false;
        }

        /// <summary>
        /// Restore death counter to zero.
        /// </summary>
        public void ResetDeathCounter()
        {
            _deathCount = 0;
        }

        /// <summary>
        /// Restore lives values to default.
        /// </summary>
        public void RestoreAllLives()
        {
            foreach (Image life in PlayerLives)
            {
                if (life.enabled == false)
                {
                    life.enabled = true;
                }
            }

            //lives = 3;
        }

        /// <summary>
        /// Increase health by percent every second for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public void IncreaseHealthForEverySecond(float percent, float totalSeconds)
        {
            StartCoroutine(IncreaseHealthForEverySecondInternal(percent, totalSeconds));
        }

        /// <summary>
        /// Decrease health by percent every second for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public void DecreaseHealthForEverySecond(float percent, float totalSeconds)
        {
            StartCoroutine(DecreaseHealthForEverySecondInternal(percent, totalSeconds));
        }

        /// <summary>
        /// Activate invulnerability mode for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public void InvulnerabilityModeForSeconds(float seconds)
        {
            StartCoroutine(InvulnerabilityModeForSecondsInternal(seconds));
        }

        /// <summary>
        /// Activate bonification defense for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void BonificationDefenseForSeconds(float percent, float seconds)
        {
            StartCoroutine(BonificationDefenseForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase health by percent.
        /// </summary>
        /// <param name="percent"></param>
        public void IncreaseHealth(float percent)
        {
            if (percent > 0.0f)
            {
                var oldHealth = CurrentHealth;

                float amount = (MaxHealth * percent / 100.0f);
                CurrentHealth += amount;
                CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);
                /*
                if (lvlProgress.IsActivated && oldHealth != maxHealth)
                {
                    lvlProgress.DecreaseProgress(CurrentHealth-oldHealth);
                }*/

                Debug.Log(String.Format(LOGS.INCREASE_HEALTH, gameObject.name, oldHealth, percent, CurrentHealth));
            }
        }

        /// <summary>
        /// Decrease health by percent.
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseHealth(float percent)
        {
            if (_healthMode == HealthMode.Normal && percent > 0.0f)
            {
                var oldHealth = CurrentHealth;

                float amount = (MaxHealth * percent / 100.0f);
                CurrentHealth -= amount;
                CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);
                /*
                if (lvlProgress.IsActivated)
                {
                    lvlProgress.IncreaseProgress(oldHealth-CurrentHealth);
                }*/
                
                Debug.Log(String.Format(LOGS.DECREASE_HEALTH, gameObject.name, oldHealth, percent, CurrentHealth));

                /*if (CurrentHealth == 0.0f && !isDead && lives == 1)
                {
                    Death();
                }*/
                if (CurrentHealth == 0.0f && !_isDead /*&& lives > 0*/)
                {
                    _playerInfo.PlayerScore.DecreaseScore(_penalization);
                    StartCoroutine(LoseLife(3));
                }
            }
        }

        /// <summary>
        /// Character takes an amount of damage.
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {
            if (_healthMode == HealthMode.Normal)
            {
                var oldHealth = CurrentHealth;

                // Reduce current health and applying bonus defense if exists
                float famount = (float)amount;

                if (_bonificationDefense > 0.0f)
                {
                    famount = famount - (famount * (_bonificationDefense / 100.0f));
                }

                CurrentHealth -= famount;

                // Play hit animation
                _playerInfo.Animator.SetTrigger(_playerInfo.TakeDamageHash);

                // Clamp current health
                CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);

                _playerInfo.PlayerEnergy.IncreaseEnergy(oldHealth - CurrentHealth);
                AudioSource.PlayClipAtPoint(hitClip[UnityEngine.Random.Range(0, hitClip.Count)], new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);

                /*if (CurrentHealth == 0.0f && !isDead && lives == 1)
                {
                    Death();
                }*/

                if (CurrentHealth == 0.0f && !_isDead /*&& lives > 0*/)
                {
                    _playerInfo.PlayerScore.DecreaseScore(_penalization);
                    StartCoroutine(LoseLife(3));
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator DecreaseHealthForEverySecondInternal(float percent, float seconds)
        {
            int counterSeconds = 0;

            while (counterSeconds < seconds)
            {
                DecreaseHealth(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }
        }

        private IEnumerator IncreaseHealthForEverySecondInternal(float percent, float seconds)
        {
            int counterSeconds = 0;

            while (counterSeconds < seconds)
            {
                IncreaseHealth(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }
        }

        private IEnumerator InvulnerabilityModeForSecondsInternal(float seconds)
        {
            _healthMode = HealthMode.Invulnerability;

            Debug.Log(string.Format(LOGS.INVULNERABILITY_MODE_ACTIVATED, gameObject.name));

            yield return new WaitForSeconds(seconds);

            Debug.Log(string.Format(LOGS.INVULNERABILITY_MODE_DEACTIVATED, gameObject.name));

            _healthMode = HealthMode.Normal;
        }

        private IEnumerator BonificationDefenseForSecondsInternal(float percent, float seconds)
        {
            _bonificationDefense = percent;

            Debug.Log(string.Format(LOGS.BONIFICATION_DEFENSE_ACTIVATED, gameObject.name, _bonificationDefense));

            yield return new WaitForSeconds(seconds);

            _bonificationDefense = 0.0f;

            Debug.Log(string.Format(LOGS.BONIFICATION_DEFENSE_DEACTIVATED, gameObject.name, _bonificationDefense));
        }

        private void DisableLife(int life)
        {
            switch (life)
            {
                case 1:
                    PlayerLives[0].enabled = false;
                    break;
                case 2:
                    PlayerLives[1].enabled = false;
                    break;
                case 3:
                    PlayerLives[2].enabled = false;
                    break;
                default:
                    return;
            }
        }

        private void Death()
        {
            //DisableLife(lives);
            //lives--;
            _deathCount++;
            _isDead = true;

            // Disable player
            _playerInfo.Animator.SetTrigger(_playerInfo.DeadHash);
            _playerInfo.PlayerMovement.DisableMovement(0f);
            _playerInfo.PlayerCollider.enabled = false;
            AudioSource.PlayClipAtPoint(deathClip, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
        }

        private IEnumerator LoseLife(float respawnTime)
        {
            Death();

            yield return new WaitForSeconds(respawnTime);

            GameManager.Instance.Find<LevelManager>().RespawnPlayerFromDeath(_playerInfo.Player);
            _isDead = false;

        }

        #endregion
    }
}
