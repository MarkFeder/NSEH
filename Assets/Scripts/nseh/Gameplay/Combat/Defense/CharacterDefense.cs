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
        // Properties

        private DefenseType CurrentMode { get; set; }

        public CharacterDefense(DefenseType defenseType, int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None,
            string buttonToPress = null)
            : base(hashAnimation, stateName, animator)
        {
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
            this.CurrentMode = defenseType;
        }

        public override void DoAction()
        {
            if (!this.IsInDefenseMode())
            {
                Debug.Log("Put in defense mode");
                this.PutInDefendMode();
            }
            else
            {
                Debug.Log("Release from defense mode");
                this.DeclineDefendMode();
            }
        }

        #region Private Methods

        private void DeclineDefendMode()
        {
            if (this.KeyHasBeenReleased() || this.ButtonHasBeenReleased())
            {
                this.Animator.SetBool(this.HashAnimation, false);
            }
        }

        private void PutInDefendMode()
        {
            if (this.KeyIsHoldDown() || this.ButtonIsHoldDown())
            {
                this.Animator.SetBool(this.HashAnimation, true);
            }
        }

        private bool IsInDefenseMode()
        {
            return this.CurrentMode == DefenseType.NormalDefense && this.Animator.GetBool(this.HashAnimation) /*&& this.Animator.GetCurrentAnimatorStateInfo(0).IsName(this.StateName)*/;
        }

        #endregion
    }
}
