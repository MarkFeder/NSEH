using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat.Defense;
using nseh.Utils.Helpers;
using Constants = nseh.Utils.Constants.Animations.Combat;
using Inputs = nseh.Utils.Constants.Input;
using Colors = nseh.Utils.Constants.Colors;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class CharacterCombat : MonoBehaviour
    {
        // Properties
        
        protected CharacterMovement characterMovement;
        protected Animator anim;
        protected Dictionary<string, int> animParameters;

        private Collider weaponCollision;
        private Rigidbody body;
        private int gamePadIndex;
        private string targetType;
        private IAction currentAction;
        private IAction currentDefenseAction;

        private List<IAction> actions;

        #region Public Properties

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

        public IAction CurrentAction
        {
            get
            {
                return this.actions.Where(act => act.HashAnimation == this.CurrentHashAnimation).FirstOrDefault();
            }
        }

        public IAction CurrentDefenseAction
        {
            get
            {
                return this.actions.OfType<CharacterDefense>().Where(act => act.HashAnimation == this.CurrentHashAnimation).FirstOrDefault();
            }
        }

        public GameObject TargetEnemy { get; set; }

        #endregion

        protected virtual void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.characterMovement = GetComponent<CharacterMovement>();
            this.body = GetComponent<Rigidbody>();

            this.gamePadIndex = this.characterMovement.GamepadIndex;
            this.actions = this.FillCharacterActions();
            this.currentAction = null;
            this.targetType = Tags.PLAYER;

            this.weaponCollision = this.gameObject.GetSafeComponentsInChildren<Collider>().Where(c => c.tag.Equals(Tags.WEAPON)).FirstOrDefault();
        }

        #region Actions 

        private List<IAction> FillCharacterActions()
        {
            var list = new List<IAction>()
            {
                new CharacterAttack(AttackType.CharacterAttackAStep1, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_01), Constants.CHARACTER_COMBO_AAA_01, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.gamePadIndex), 10.0f),
                new CharacterAttack(AttackType.CharacterAttackAStep2, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_02), Constants.CHARACTER_COMBO_AAA_02, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.gamePadIndex), 5.0f),
                new CharacterAttack(AttackType.CharacterAttackAStep3, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_03), Constants.CHARACTER_COMBO_AAA_03, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.gamePadIndex), 1.0f),
                new CharacterAttack(AttackType.CharacterAttackBStep1, Animator.StringToHash(Constants.CHARACTER_COMBO_BB_01), Constants.CHARACTER_COMBO_BB_01, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.B, this.gamePadIndex), 10.0f),
                new CharacterAttack(AttackType.CharacterAttackBStep2, Animator.StringToHash(Constants.CHARACTER_COMBO_BB_02), Constants.CHARACTER_COMBO_BB_02, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.B, this.gamePadIndex), 10.0f),
                new CharacterAttack(AttackType.CharacterDefinitive, Animator.StringToHash(Constants.CHARACTER_DEFINITIVE), Constants.CHARACTER_DEFINITIVE, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.DEFINITIVE, this.gamePadIndex), 20.0f),
                new CharacterAttack(AttackType.CharacterHability, Animator.StringToHash(Constants.CHARACTER_HABILITY), Constants.CHARACTER_HABILITY, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.HABILITY, this.gamePadIndex), 20.0f),
                new CharacterDefense(DefenseType.NormalDefense, Animator.StringToHash(Constants.CHARACTER_DEFENSE), Constants.CHARACTER_DEFENSE, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.DEFENSE, this.gamePadIndex))
            };

            return list;
        }

        #endregion

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            this.currentAction = this.actions.OfType<CharacterAttack>().Where(action => action.KeyHasBeenPressed() || action.ButtonHasBeenPressed()).FirstOrDefault();
            this.currentDefenseAction = this.actions.OfType<CharacterDefense>().Where(act => act.KeyHasBeenReleased() || act.KeyIsHoldDown() || act.ButtonHasBeenReleased() || act.ButtonIsHoldDown()).FirstOrDefault();
        }

        protected virtual void FixedUpdate()
        {
            this.Defense();

            this.Attack();
        }

        protected virtual void Defense()
        {
            if (this.currentDefenseAction != null && this.currentDefenseAction.GetType() == typeof(CharacterDefense))
            {
                this.currentDefenseAction.DoAction();
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
            if (!String.IsNullOrEmpty(stateName) && !this.weaponCollision.enabled)
            {
                this.weaponCollision.enabled = true;
            }
        }

        private void DeactivateCollider(string stateName)
        {
            if (!String.IsNullOrEmpty(stateName) && this.weaponCollision.enabled)
            {
                this.weaponCollision.enabled = false;
            }
        }

        private void EventDamage(string stateName)
        {
            //weaponCollision collision = this.gameObject.GetSafeComponentInChildren<IWeapon>() as weaponCollision;

            //if (!String.IsNullOrEmpty(stateName) && this.weaponCollision.enabled && collision != null)
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
            //    Debug.Log("weaponCollision is null");
            //}
        }

        #endregion
    }
}
