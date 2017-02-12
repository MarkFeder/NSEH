using nseh.Gameplay.Base.Interfaces;
using System;
using UnityEngine;
using nseh.GameManager.General;
using Constants = nseh.Utils.Constants;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class CharacterHealth : MonoBehaviour, IHealth
    {
        private BarComponent healthBar;

        public BarComponent HealthBar
        {
            set
            {
                healthBar = value;
            }
        }

        [SerializeField]
        protected int startingHealth = 100;
        [SerializeField]
        private int maxHealth = 100;
        private int currentHealth;

        protected int CurrentHealth
        {
            get
            {
                return currentHealth;
            }

            set
            {
                currentHealth = value;
                //healthBar.Value = currentHealth;
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
                //healthBar.MaxValue = maxHealth;
            }
        }

        private Animator anim;
        protected CharacterMovement characterMovement;
        protected bool isDead;
        protected int animDead;



        protected virtual void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.characterMovement = GetComponent<CharacterMovement>();
            this.animDead = Animator.StringToHash(Constants.Animations.Combat.CHARACTER_DEAD);

            // Set initial health
            this.CurrentHealth = startingHealth;
            this.MaxHealth = maxHealth;
        }

        protected virtual void Update()
        {
            Debug.Log("Current health of " + this.gameObject.name + " is " + this.CurrentHealth);
        }

        public virtual void TakeDamage(int amount)
        {
            // Reduce current health
            this.CurrentHealth -= amount;
            this.CurrentHealth = (int)Mathf.Clamp(this.CurrentHealth, 0.0f, this.MaxHealth);

            if (this.CurrentHealth == 0.0f && !this.isDead)
            {
                this.Death();
            }
        }

        protected virtual void Death()
        {
            this.isDead = true;

            this.anim.SetTrigger(animDead);

            this.characterMovement.enabled = false;
        }
    }
}
