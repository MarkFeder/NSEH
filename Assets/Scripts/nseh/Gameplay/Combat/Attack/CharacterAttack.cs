using nseh.Gameplay.Base.Abstract;
using System;
using System.Collections;
using UnityEngine;

namespace nseh.Gameplay.Combat
{
    public enum AttackType
    {
        None = 0,
        CharacterAttackAStep1 = 1,
        CharacterAttackAStep2 = 2,
        CharacterAttackAStep3 = 3,
        CharacterAttackBStep1 = 4,
        CharacterAttackBStep2 = 5,
        CharacterAttackBSharp = 6,
        CharacterHability = 7,
        CharacterDefinitive = 8,
    }

    public class CharacterAttack : HandledAction
    {
        private float initialDamage;
        private float currentDamage;
        private bool critical;

        #region Public Properties

        public float InitialDamage
        {
            get
            {
                return this.initialDamage;
            }
        }

        public bool Critical
        {
            get
            {
                return this.critical;
            }

            set
            {
                this.critical = value;
            }
        }

        public float CurrentDamage
        {
            get
            {
                return this.currentDamage;
            }

            set
            {
                this.currentDamage = value;
            }
        }

        public bool EnabledAttack
        {
            get;
            set;
        }

        public AttackType AttackType
        {
            get;
            private set;
        }

        public bool IsCombo
        {
            get
            {
                return this.AttackType == AttackType.CharacterAttackAStep1 ||
                       this.AttackType == AttackType.CharacterAttackAStep2 ||
                       this.AttackType == AttackType.CharacterAttackAStep3 ||
                       this.AttackType == AttackType.CharacterAttackBStep1 ||
                       this.AttackType == AttackType.CharacterAttackBStep2;         
            }
        }

        public bool IsSimpleAttack
        {
            get
            {
                return !(this.AttackType == AttackType.CharacterDefinitive ||
                       this.AttackType == AttackType.CharacterHability);
            }
        }

        #endregion

        public CharacterAttack(AttackType attackType, int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None,
            string buttonToPress = null,
            float damage = 0.0f)
            : base(hashAnimation, stateName, animator)
        {
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
            this.AttackType = attackType;
            this.EnabledAttack = true;

            this.initialDamage = damage;
            this.currentDamage = damage;
            this.critical = false;
        }

        #region Public Methods

        public override void DoAction()
        {
            if (this.EnabledAttack)
            {
                if (this.IsSimpleAttack && this.critical)
                {
                    base.DoAction();

                    this.critical = false;
                    this.currentDamage = this.initialDamage;
                }
                else
                {
                    base.DoAction();
                }
            }
        }

        public void IncreaseDamageForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.IncreaseDamageForSecondsInternal(percent, seconds));
        }

        public void IncreaseDamage(float percent)
        {
            if (percent > 0.0f)
            {
                var oldDamage = this.currentDamage;

                this.currentDamage += (this.currentDamage * percent);

                Debug.Log(String.Format("[{0}] damage of {1} is: {2} and applying {3}% more has changed to: {4}",
                    this.AttackType.ToString(), this.gameObject.name, oldDamage, percent * 100.0f, this.currentDamage));
            }
        }

        public void DecreaseDamageForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.DecreaseDamageForSecondsInternal(percent, seconds));
        }

        public void DecreaseDamage(float percent)
        {
            if (percent > 0.0f)
            {
                var oldDamage = this.currentDamage;

                this.currentDamage -= (this.currentDamage * percent);

                Debug.Log(String.Format("[{0}] damage of {1} is: {2} and reducing {3}% has changed to: {4}",
                    this.AttackType.ToString(), this.gameObject.name, oldDamage, percent * 100.0f, this.currentDamage));
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator IncreaseDamageForSecondsInternal(float percent, float seconds)
        {
            float currentTime = 0;

            while (currentTime <= seconds)
            {
                currentTime += Time.deltaTime;

                this.IncreaseDamage(percent);

                yield return null;
            }
        }

        private IEnumerator DecreaseDamageForSecondsInternal(float percent, float seconds)
        {
            float currentTime = 0;

            while (currentTime <= seconds)
            {
                currentTime += Time.deltaTime;

                this.DecreaseDamage(percent);

                yield return null;
            }
        }

        #endregion
    }
}
