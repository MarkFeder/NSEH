using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat.Defense;
using nseh.Utils.Helpers;
using Constants = nseh.Utils.Constants.Animations.Combat;
using Colors = nseh.Utils.Constants.Colors;
using Tags = nseh.Utils.Constants.Tags;
using nseh.Gameplay.Combat.Attack;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class CharacterCombat : MonoBehaviour
    {
        private string targetType;
        public GameObject TargetEnemy { get; set; }

        protected CharacterMovement characterMovement;
        protected Animator anim;
        protected Dictionary<string, int> animParameters;
        private Rigidbody body;

        private IAction currentAction;

        private List<IAction> actions;
        public List<IAction> Actions
        {
            get
            {
                return this.actions;
            }
        }

        public int CurrentHashAnimation
        {
            get
            {
                return (this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash);
            }
        }

        public HandledAction CurrentAction
        {
            get
            {
                if (this.actions != null && this.actions.Count > 0)
                {
                    return (this.actions.Where(act => act.HashAnimation == this.CurrentHashAnimation).FirstOrDefault()) as HandledAction;
                }

                return null;
            }
        }

        public CharacterAttack CurrentCharacterAttackAction
        {
            get
            {
                if (this.actions != null && this.actions.Count > 0)
                {
                    return this.actions.OfType<CharacterAttack>().Where(act => act.HashAnimation == this.CurrentHashAnimation).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        private Collider WeaponCollision;

        protected virtual void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.characterMovement = GetComponent<CharacterMovement>();
            this.body = GetComponent<Rigidbody>();

            this.actions = this.FillCharacterActions();
            this.currentAction = null;
            this.targetType = Tags.PLAYER;

            this.WeaponCollision = this.gameObject.GetSafeComponentsInChildren<Collider>().Where(c => c.tag.Equals(Tags.WEAPON)).FirstOrDefault();
        }

        public CharacterAttack GetCharacterAttack(int hashAnimation)
        {
            return this.actions.Where(a => a.GetType() == typeof(CharacterAttack)).Where(a => (a as CharacterAttack).HashAnimation == hashAnimation).FirstOrDefault() as CharacterAttack;
        }

        private List<IAction> FillCharacterActions()
        {
            var list = new List<IAction>()
            {
                new CharacterAttack(AttackType.CharacterAttackAStep1, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_01), Constants.CHARACTER_COMBO_AAA_01, this.anim, KeyCode.C, null, 10.0f),
                new CharacterAttack(AttackType.CharacterAttackAStep2, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_02), Constants.CHARACTER_COMBO_AAA_02, this.anim, KeyCode.C, null, 5.0f),
                new CharacterAttack(AttackType.CharacterAttackAStep3, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_03), Constants.CHARACTER_COMBO_AAA_03, this.anim, KeyCode.C, null, 1.0f),
                new CharacterAttack(AttackType.CharacterAttackBStep1, Animator.StringToHash(Constants.CHARACTER_COMBO_BB_01), Constants.CHARACTER_COMBO_BB_01, this.anim, KeyCode.B),
                new CharacterAttack(AttackType.CharacterAttackBStep2, Animator.StringToHash(Constants.CHARACTER_COMBO_BB_02), Constants.CHARACTER_COMBO_BB_02, this.anim, KeyCode.B),
                new CharacterAttack(AttackType.CharacterDefinitive, Animator.StringToHash(Constants.CHARACTER_DEFINITIVE), Constants.CHARACTER_DEFINITIVE, this.anim, KeyCode.N),
                new CharacterAttack(AttackType.CharacterHability, Animator.StringToHash(Constants.CHARACTER_HABILITY), Constants.CHARACTER_HABILITY, this.anim, KeyCode.M),
                new CharacterDefense(DefenseType.NormalDefense, Animator.StringToHash(Constants.CHARACTER_DEFENSE), Constants.CHARACTER_DEFENSE, this.anim, KeyCode.F),
                new CharacterImpact(ImpactType.Normal, Animator.StringToHash(Constants.CHARACTER_IMPACT), Constants.CHARACTER_IMPACT, this.anim)
            };

            return list;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {

            this.CheckInputAndSetAction();
        }

        private void CheckInputAndSetAction()
        {

            this.currentAction = this.actions.Where(action => action.ButtonHasBeenPressed() || action.KeyHasBeenPressed()).FirstOrDefault();
        }

        protected virtual void FixedUpdate()
        {
            this.Defense();

            this.Attack();
        }

        protected virtual void Defense()
        {
            if (this.currentAction != null && this.currentAction.GetType() == typeof(CharacterDefense))
            {
                this.currentAction.DoAction();
            }
        }

        protected virtual void Attack()
        {
            if (this.currentAction != null && this.currentAction.GetType() == typeof(CharacterAttack))
            {
                this.currentAction.DoAction();
            }
        }

        #region Animation Events

        private void ActivateCollider(string stateName)
        {
            if (!String.IsNullOrEmpty(stateName) && !this.WeaponCollision.enabled)
            {
                this.WeaponCollision.enabled = true;
            }
        }

        private void DeactivateCollider(string stateName)
        {
            if (!String.IsNullOrEmpty(stateName) && this.WeaponCollision.enabled)
            {
                this.WeaponCollision.enabled = false;
            }
        }

        private void EventDamage(string stateName)
        {
            //WeaponCollision collision = this.gameObject.GetSafeComponentInChildren<IWeapon>() as WeaponCollision;

            //if (!String.IsNullOrEmpty(stateName) && this.WeaponCollision.enabled && collision != null)
            //{
            //    var attack = this.Actions.OfType<CharacterAttack>().Where(at => at.HashAnimation == CurrentHashAnimation).FirstOrDefault();
            //    if (attack != null)
            //    {
            //        Debug.Log(String.Format("<color={0}> {1} does the attack: {2}</color>", Colors.FUCHSIA, this.gameObject.name, attack.StateName));

            //        attack.PerformDamage(this.gameObject, collision.EnemyTargets);
            //    }
            //    else
            //    {
            //        Debug.LogError("Attack is null when it should not!");
            //    }
            //}
            //else
            //{
            //    Debug.Log("WeaponCollision is null");
            //}
        }

        #endregion
    }
}
