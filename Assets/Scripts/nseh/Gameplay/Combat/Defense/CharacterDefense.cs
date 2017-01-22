using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nseh.Gameplay.Base.Abstract;
using UnityEngine;

namespace nseh.Gameplay.Combat.Defense
{
    public enum DefenseType
    {
        None = 0,
        NormalDefense = 1
    }

    public class CharacterDefense : HandledAction
    {
        private DefenseType CurrentMode { get; set; }

        public CharacterDefense(DefenseType defenseType, int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None,
            string buttonToPress = null)
            : base(hashAnimation, stateName, animator)
        {
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
        }

        public override void DoAction()
        {
            if (!this.IsInDefenseMode())
            {
                this.PutInDefendMode();
                base.DoAction();
            }
            else
            {
                this.DeclineDefendMode();
            }
        }

        private void DeclineDefendMode()
        {
            if (this.KeyHasBeenReleased() && this.CurrentMode == DefenseType.NormalDefense)
            {
                this.CurrentMode = DefenseType.None;
                this.Animator.SetBool(this.HashAnimation, false);
            }
        }

        private void PutInDefendMode()
        {
            if (this.CurrentMode != DefenseType.NormalDefense && this.KeyHasBeenHoldDown())
            {
                this.CurrentMode = DefenseType.NormalDefense;
                this.Animator.SetBool(this.HashAnimation, true);
            }
        }

        private bool IsInDefenseMode()
        {
            return this.CurrentMode != DefenseType.NormalDefense && this.Animator.GetBool(this.HashAnimation) && this.Animator.GetCurrentAnimatorStateInfo(0).IsName(this.StateName);
        }

        private bool KeyHasBeenHoldDown()
        {
            return this.KeyToPress != KeyCode.None && Input.GetKey(this.KeyToPress);
        }

        private bool KeyHasBeenReleased()
        {
            return this.KeyToPress != KeyCode.None && Input.GetKeyUp(this.KeyToPress);
        }



        //private void PutInDefendMode()
        //{
        //    if (this.ActionType != AttackType.CharacterDefense && Input.GetKey(KeyCode.F))
        //    {
        //        this.currentMode = AttackType.CharacterDefense;
        //        this.anim.SetBool(this.animParameters[Constants.CHARACTER_DEFENSE], true);
        //    }
        //}

        //private void DeclineDefendMode()
        //{
        //    if (Input.GetKeyUp(KeyCode.F) && this.currentMode == AttackType.CharacterDefense)
        //    {
        //        this.currentMode = AttackType.None;
        //        this.anim.SetBool(this.animParameters[Constants.CHARACTER_DEFENSE], false);
        //    }
        //}

        //private bool IsInDefenseMode()
        //{
        //    return this.ActionType == AttackType.CharacterDefense && this.anim.GetBool(this.animParameters[Constants.CHARACTER_DEFENSE]) && this.anim.GetCurrentAnimatorStateInfo(0).IsName(Constants.CHARACTER_DEFENSE);
        //}
    }
}
