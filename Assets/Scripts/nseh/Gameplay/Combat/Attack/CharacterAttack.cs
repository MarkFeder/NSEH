using nseh.Gameplay.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace nseh.Gameplay.Combat
{
    public enum AttackType
    {
        None = 0,
        CharacterAttackA = 1,
        CharacterAttackB = 2,
        CharacterComboAAA = 3,
        CharacterComboBB = 4,
        CharacterComboBSharp = 5,
        CharacterHability = 6,
        CharacterDefinitive = 7,
    }

    public class CharacterAttack : Base.Abstract.HandledAction
    {
        public float Damage { get; set; }

        public CharacterAttack(AttackType attackType, int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None, 
            string buttonToPress = null, 
            float damage = 0.0f) 
            : base(hashAnimation, stateName, animator)
        {
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
            this.Damage = damage;
        }

        public override void DoAction()
        {
            base.DoAction();
        }
    }
}
