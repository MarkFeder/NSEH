using nseh.GameManager.General;
using nseh.Gameplay.Base.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using Constants = nseh.Utils.Constants;

namespace nseh.Gameplay.Base.Abstract
{
    public enum HealthMode
    {
        Normal = 0,
        Invulnerability = 1
    }

    [RequireComponent(typeof(Animator))]
    public abstract class CharacterHealth : MonoBehaviour, IHealth
    {
        public int startingHealth = 100;
        public int maxHealth = 100;

        private Animator anim;
        private BarComponent healthBar;

        protected CharacterMovement characterMovement;
        protected HealthMode healthMode;

        protected float currentHealth;
        protected bool isDead;
        protected int animDead;

        #region Public Properties

        public float CurrentHealth
        {
            get
            {
                return this.currentHealth;
            }

            set
            {
                this.currentHealth = value;
                healthBar.Value = currentHealth;
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

        protected int MaxHealth
        {
            get
            {
                return maxHealth;
            }

            set
            {
                maxHealth = value;
                healthBar.MaxValue = maxHealth;
            }
        }

        #endregion

        protected virtual void Start()
        {
            this.anim = GetComponent<Animator>();
            this.characterMovement = GetComponent<CharacterMovement>();
            this.animDead = Animator.StringToHash(Constants.Animations.Combat.CHARACTER_DEAD);

            // Set initial health
            this.MaxHealth = this.maxHealth;
            this.CurrentHealth = this.startingHealth;
        }

        protected virtual void Update()
        {
           // Debug.Log("Current health of " + this.gameObject.name + " is " + this.CurrentHealth);
        }

        #region Public Methods

        public void IncreaseHealth(float percent)
        {
            if (percent > 0.0f)
            {
                var oldHealth = this.CurrentHealth;

                this.CurrentHealth += (this.CurrentHealth * percent);

                Debug.Log(String.Format("Health of {0} is: {1} and applying {2}% more has changed to: {3}", this.gameObject.name, oldHealth, percent * 100.0f, this.CurrentHealth));
            }
        }

        public void DecreaseHealth(float percent)
        {
            if (percent > 0.0f)
            {
                var oldHealth = this.CurrentHealth;

                this.CurrentHealth -= (this.CurrentHealth * percent);

                Debug.Log(String.Format("Health of {0} is: {1} and reducing {2}% has changed to: {3}", this.gameObject.name, oldHealth, percent * 100.0f, this.CurrentHealth));
            }
        }

        public void DecreaseHealthForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.DecreaseHealthForSecondsInternal(percent, seconds));
        }

        public void InvulnerabilityModeForSeconds(float seconds)
        {
            StartCoroutine(this.InvulnerabilityModeForSecondsInternal(seconds));
        }

        public void TakeDamage(int amount)
        {
            if (this.healthMode == HealthMode.Normal)
            {
                // Reduce current health
                this.CurrentHealth -= amount;
                this.CurrentHealth = (int)Mathf.Clamp(this.CurrentHealth, 0.0f, this.maxHealth);

                if (this.CurrentHealth == 0.0f && !this.isDead)
                {
                    this.Death();
                }
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator DecreaseHealthForSecondsInternal(float percent, float seconds)
        {
            float currentTime = 0;

            while (currentTime <= seconds)
            {
                currentTime += Time.deltaTime;

                this.DecreaseHealth(percent);

                yield return null;
            }
        }

        private IEnumerator InvulnerabilityModeForSecondsInternal(float seconds)
        {
            this.healthMode = HealthMode.Invulnerability;

            yield return new WaitForSeconds(seconds);

            this.healthMode = HealthMode.Normal;
        }

        #endregion

        protected void Death()
        {
            this.isDead = true;

            this.anim.SetTrigger(animDead);

            this.characterMovement.enabled = false;
        }
    }
}
