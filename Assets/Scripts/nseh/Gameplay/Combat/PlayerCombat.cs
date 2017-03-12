using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using nseh.Gameplay.Entities.Player;
using nseh.Gameplay.Movement;
using nseh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Constants = nseh.Utils.Constants.Animations.Combat;
using Inputs = nseh.Utils.Constants.Input;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat
{
    [RequireComponent(typeof(Animator))]
    public class PlayerCombat : MonoBehaviour
    {
        #region Protected Properties

        protected PlayerMovement characterMovement;
        protected Animator anim;
        protected Dictionary<string, int> animParameters; 

        #endregion

        #region Private Properties

        private Collider weaponCollision;
        private Rigidbody body;
        private PlayerInfo playerInfo;

        private IAction currentAction;
        private IAction currentDefenseAction;
        private List<IAction> actions;

        #endregion

        #region Public C# Properties

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
            this.characterMovement = GetComponent<PlayerMovement>();
            this.body = GetComponent<Rigidbody>();
            this.playerInfo = GetComponent<PlayerInfo>();

            this.actions = this.FillCharacterActions();
            this.currentAction = null;

            this.weaponCollision = this.gameObject.GetSafeComponentsInChildren<Collider>().Where(c => c.tag.Equals(Tags.WEAPON)).FirstOrDefault();
        }

        #region Actions 

        private List<IAction> FillCharacterActions()
        {
            var list = new List<IAction>()
            {
                new CharacterAttack(AttackType.CharacterAttackAStep1, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_01), Constants.CHARACTER_COMBO_AAA_01, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.playerInfo.GamepadIndex), this.playerInfo.DamageAttackA),
                new CharacterAttack(AttackType.CharacterAttackAStep2, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_02), Constants.CHARACTER_COMBO_AAA_02, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.playerInfo.GamepadIndex), this.playerInfo.DamageComboAAA01),
                new CharacterAttack(AttackType.CharacterAttackAStep3, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_03), Constants.CHARACTER_COMBO_AAA_03, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.playerInfo.GamepadIndex), this.playerInfo.DamageComboAAA01),
                new CharacterAttack(AttackType.CharacterAttackBStep1, Animator.StringToHash(Constants.CHARACTER_COMBO_BB_01), Constants.CHARACTER_COMBO_BB_01, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.B, this.playerInfo.GamepadIndex), this.playerInfo.DamageAttackB),
                new CharacterAttack(AttackType.CharacterAttackBStep2, Animator.StringToHash(Constants.CHARACTER_COMBO_BB_02), Constants.CHARACTER_COMBO_BB_02, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.B, this.playerInfo.GamepadIndex), this.playerInfo.DamageComboBB01),
                new CharacterAttack(AttackType.CharacterDefinitive, Animator.StringToHash(Constants.CHARACTER_DEFINITIVE), Constants.CHARACTER_DEFINITIVE, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.DEFINITIVE, this.playerInfo.GamepadIndex), this.playerInfo.DamageDefinitive),
                new CharacterAttack(AttackType.CharacterHability, Animator.StringToHash(Constants.CHARACTER_HABILITY), Constants.CHARACTER_HABILITY, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.HABILITY, this.playerInfo.GamepadIndex), this.playerInfo.DamageHability),
                new CharacterDefense(DefenseType.NormalDefense, Animator.StringToHash(Constants.CHARACTER_DEFENSE), Constants.CHARACTER_DEFENSE, this.anim, KeyCode.None, String.Format("{0}{1}", Inputs.DEFENSE, this.playerInfo.GamepadIndex))
            };

            return list;
        }

        #endregion

        private void Update()
        {
            this.currentAction = this.actions.OfType<CharacterAttack>().Where(action => action.ReceiveInput() /*action => action.KeyHasBeenPressed() || action.ButtonHasBeenPressed()*/).FirstOrDefault();
            this.currentDefenseAction = this.actions.OfType<CharacterDefense>().Where(action => action.ReceiveInput() /*act => act.KeyHasBeenReleased() || act.KeyIsHoldDown() || act.ButtonHasBeenReleased() || act.ButtonIsHoldDown()*/).FirstOrDefault();
        }

        private void FixedUpdate()
        {
            this.Defense();

            this.Attack();
        }

        #region Protected Methods

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

        #endregion

        #region Animation Events

        private void ActivateCollider(string stateName)
        {
            if (this.weaponCollision != null && !this.weaponCollision.enabled)
            {
                this.weaponCollision.enabled = true;
            }
        }

        private void DeactivateCollider(string stateName)
        {
            if (this.weaponCollision != null && this.weaponCollision.enabled)
            {
                this.weaponCollision.enabled = false;
            }
        }

        #endregion
    }
}
