using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Gameflow;
using nseh.Managers.General;
using nseh.Managers.Main;
using nseh.Managers.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Gameplay.Entities.Player
{
    public enum HealthMode
    {
        Normal = 0,
        Invulnerability = 1
    }

    public class PlayerHealth : MonoBehaviour, IHealth
    {
        #region Public Properties

        public int startingHealth = 100;
        public int maxHealth = 100;

        #endregion

        #region Private Properties

        private BarComponent healthBar;
        private List<Image> playerLives;
        private PlayerInfo playerInfo;
        private HealthMode healthMode;

        private LevelProgress lvlProgress;

        private float currentHealth;
        private int lives;
        private bool isDead;
        private int animDead;

        #endregion

        #region Public C# Properties

        public float CurrentHealth
        {
            get
            {
                return this.currentHealth;
            }

            set
            {
                this.currentHealth = value;

                if (healthBar != null)
                {
                    healthBar.Value = currentHealth;
                }
            }
        }

        public HealthMode HealthMode
        {
            get
            {
                return this.healthMode;
            }

            set
            {
                this.healthMode = value;
            }
        }

        public BarComponent HealthBar
        {
            set
            {
                this.healthBar = value;
            }
        }

        public LevelProgress LvlProgress
        {
            set
            {
                this.lvlProgress = value;
            }
        }

        public int StartingHealth
        {
            get { return this.startingHealth; }
            set { this.startingHealth = value; }
        }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }

            set
            {
                maxHealth = value;

                if (healthBar != null)
                {
                    healthBar.MaxValue = maxHealth;
                }
            }
        }

        public List<Image> PlayerLives
        {
            get
            {
                return playerLives;
            }

            set
            {
                playerLives = value;
            }
        }

        #endregion

        protected virtual void Start()
        {
            this.playerInfo = GetComponent<PlayerInfo>();

            // Set initial health
            this.MaxHealth = this.maxHealth;
            this.CurrentHealth = this.startingHealth;
            this.lives = 3;
            this.isDead = false;
        }

        protected virtual void Update()
        {
           Debug.Log("Current health of " + this.gameObject.name + " is " + this.CurrentHealth);
        }

        #region Public Methods

        /// <summary>
        /// Restore health values to default
        /// </summary>
        public void ResetHealth()
        {
            this.MaxHealth = this.maxHealth;
            this.CurrentHealth = this.startingHealth;
            this.isDead = false;
        }

        /// <summary>
        /// Restore lives values to default
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

            this.lives = 3;
        }

        /// <summary>
        /// Increase health by percent every second for a total of seconds
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public void IncreaseHealthForEverySecond(float percent, float totalSeconds)
        {
            StartCoroutine(this.IncreaseHealthForEverySecondInternal(percent, totalSeconds));
        }

        /// <summary>
        /// Decrease health by percent every second for a total of seconds
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public void DecreaseHealthForEverySecond(float percent, float totalSeconds)
        {
            StartCoroutine(this.DecreaseHealthForEverySecondInternal(percent, totalSeconds));
        }

        /// <summary>
        /// Activate invulnerability mode for a total of seconds
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public void InvulnerabilityModeForSeconds(float seconds)
        {
            StartCoroutine(this.InvulnerabilityModeForSecondsInternal(seconds));
        }

        /// <summary>
        /// Increase health by percent
        /// </summary>
        /// <param name="percent"></param>
        public void IncreaseHealth(float percent)
        {
            if (percent > 0.0f)
            {
                var oldHealth = this.CurrentHealth;

                float amount = (this.MaxHealth * percent / 100.0f);
                this.CurrentHealth += amount;
                this.CurrentHealth = (int)Mathf.Clamp(this.CurrentHealth, 0.0f, this.maxHealth);

                if (lvlProgress.IsActivated && oldHealth != this.maxHealth)
                {
                    lvlProgress.DecreaseProgress(this.CurrentHealth-oldHealth);
                }

                Debug.Log(String.Format("Health of {0} is: {1} and applying {2}% more has changed to: {3}", this.gameObject.name, oldHealth, percent, this.CurrentHealth));
            }
        }

        /// <summary>
        /// Decrease health by percent
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseHealth(float percent)
        {
            if (this.healthMode == HealthMode.Normal && percent > 0.0f)
            {
                var oldHealth = this.CurrentHealth;

                float amount = (this.MaxHealth * percent / 100.0f);
                this.CurrentHealth -= amount;
                this.CurrentHealth = (int)Mathf.Clamp(this.CurrentHealth, 0.0f, this.maxHealth);

                if (lvlProgress.IsActivated)
                {
                    lvlProgress.IncreaseProgress(oldHealth-this.CurrentHealth);
                }

                Debug.Log(String.Format("Health of {0} is: {1} and reducing {2}% has changed to: {3}", this.gameObject.name, oldHealth, percent, this.CurrentHealth));

                if (this.CurrentHealth == 0.0f && !this.isDead && lives == 1)
                {
                    this.Death();
                }
                else if (this.CurrentHealth == 0.0f && !this.isDead && lives > 0)
                {
                    StartCoroutine(this.LoseLife(3));
                }
            }
        }

        /// <summary>
        /// Character takes an amount of damage
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {
            if (this.healthMode == HealthMode.Normal)
            {
                var oldHealth = this.CurrentHealth;

                // Reduce current health
                this.CurrentHealth -= amount;
                this.CurrentHealth = (int)Mathf.Clamp(this.CurrentHealth, 0.0f, this.maxHealth);

                if (lvlProgress.IsActivated)
                {
                    lvlProgress.IncreaseProgress(oldHealth-this.CurrentHealth);
                }

                if (this.CurrentHealth == 0.0f && !this.isDead && lives == 1)
                {
                    this.Death();
                }
                else if(this.CurrentHealth == 0.0f && !this.isDead && lives > 0)
                {
                    StartCoroutine(this.LoseLife(3));
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
                this.DecreaseHealth(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }
        }

        private IEnumerator IncreaseHealthForEverySecondInternal(float percent, float seconds)
        {
            int counterSeconds = 0;

            while (counterSeconds < seconds)
            {
                this.IncreaseHealth(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }
        }

        private IEnumerator InvulnerabilityModeForSecondsInternal(float seconds)
        {
            this.healthMode = HealthMode.Invulnerability;

            Debug.Log(string.Format("Character {0} is entering Invulnerability mode", this.gameObject.name));

            yield return new WaitForSeconds(seconds);

            Debug.Log(string.Format("Character {0} is exiting Invulnerability mode", this.gameObject.name));

            this.healthMode = HealthMode.Normal;
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
            DisableLife(lives);
            lives--;
            this.isDead = true;

            // Disable player
            this.playerInfo.Animator.SetTrigger(this.playerInfo.DeadHash);
            this.playerInfo.PlayerMovement.DisableMovement();
            this.playerInfo.PlayerCollider.enabled = false;
        }

        private IEnumerator LoseLife(float respawnTime)
        {
            this.Death();

            yield return new WaitForSeconds(respawnTime);

            GameManager.Instance.Find<LevelManager>().RespawnPlayerFromDeath(this.playerInfo.Player);
            this.isDead = false;

        }
        #endregion
    }
}
