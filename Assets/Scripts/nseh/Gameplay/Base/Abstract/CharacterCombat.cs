using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat.Defense;
using Constants = nseh.Utils.Constants.Animations.Combat;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class CharacterCombat : MonoBehaviour
    {
        protected bool defend;

        protected CharacterMovement characterMovement;

        protected Animator anim;
        public Animator Anim
        {
            get
            {
                return this.anim;
            }
        }

        protected Dictionary<string, int> animParameters;

        private Rigidbody body;

        private IAction currentAction;

        private List<IAction> actions;

        protected virtual void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.characterMovement = GetComponent<CharacterMovement>();
            this.body = GetComponent<Rigidbody>();

            this.actions = this.FillCharacterActions();
            this.currentAction = null;
        }

        private List<IAction> FillCharacterActions()
        {
            var list = new List<IAction>()
            {
                new CharacterAttack(AttackType.CharacterComboAAA, Animator.StringToHash(Constants.CHARACTER_COMBO_AAA_01), Constants.CHARACTER_COMBO_AAA_01, this.Anim, KeyCode.C),
                new CharacterAttack(AttackType.CharacterComboBB, Animator.StringToHash(Constants.CHARACTER_COMBO_BB_01), Constants.CHARACTER_COMBO_BB_01, this.Anim, KeyCode.B),
                new CharacterAttack(AttackType.CharacterDefinitive, Animator.StringToHash(Constants.CHARACTER_DEFINITIVE), Constants.CHARACTER_DEFINITIVE, this.Anim, KeyCode.N),
                new CharacterAttack(AttackType.CharacterHability, Animator.StringToHash(Constants.CHARACTER_HABILITY), Constants.CHARACTER_HABILITY, this.Anim, KeyCode.M),
                new CharacterDefense(DefenseType.NormalDefense, Animator.StringToHash(Constants.CHARACTER_DEFENSE), Constants.CHARACTER_DEFENSE, this.Anim, KeyCode.F)
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
            this.Defend();

            this.Attack();
        }

        protected virtual void Defend()
        {
            if (this.currentAction != null && this.currentAction.GetType() == typeof(CharacterDefense))
            {
                this.currentAction.DoAction();
                this.currentAction = null;
            }
        }

        protected virtual void Attack()
        {
            if (this.currentAction != null && this.currentAction.GetType() == typeof(CharacterAttack))
            {
                this.currentAction.DoAction();
                this.currentAction = null;
            }
        }
    }
}
