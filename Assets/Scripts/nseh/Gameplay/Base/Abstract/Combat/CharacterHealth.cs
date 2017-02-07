using nseh.Gameplay.Base.Interfaces;
using System;
using UnityEngine;
using Constants = nseh.Utils.Constants;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class CharacterHealth : MonoBehaviour, IHealth
    {
        [SerializeField]
        protected int startingHealth = 100;
        [SerializeField]
        protected int maxHealth = 100;
        protected int currentHealth;

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
            this.currentHealth = startingHealth;
        }

        protected virtual void Update()
        {
            Debug.Log("Current health of " + this.gameObject.name + " is " + this.currentHealth);
        }

        public virtual void TakeDamage(int amount)
        {
            // Reduce current health
            this.currentHealth -= amount;
            this.currentHealth = (int)Mathf.Clamp(this.currentHealth, 0.0f, this.maxHealth);

            if (this.currentHealth == 0.0f && !this.isDead)
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
