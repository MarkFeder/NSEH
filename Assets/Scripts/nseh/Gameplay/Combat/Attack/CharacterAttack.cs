using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat.Defense;
using nseh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Colors = nseh.Utils.Constants.Colors;
using Constants = nseh.Utils.Constants.Animations.Combat;

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
        public float Damage { get; set; }
        public AttackType AttackType { get; private set; }

        public CharacterAttack(AttackType attackType, int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None,
            string buttonToPress = null,
            float damage = 0.0f)
            : base(hashAnimation, stateName, animator)
        {
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
            this.Damage = damage;
            this.AttackType = attackType;
        }

        public override void DoAction()
        {
            base.DoAction();
        }
    }
}
