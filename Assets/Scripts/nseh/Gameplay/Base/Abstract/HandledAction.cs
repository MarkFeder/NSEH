using nseh.Gameplay.Base.Interfaces;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Base.Abstract
{
    public abstract class HandledAction : IAction
    {
        public int HashAnimation { get; set; }
        public string StateName { get; set; }
        public KeyCode KeyToPress { get; set; }
        public string ButtonToPress { get; set; }
        public Animator Animator { get; set; }

        private AnimatorControllerParameterType ParameterType { get; set; }

        public HandledAction(int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None, 
            string buttonToPress = null)
        {
            this.HashAnimation = hashAnimation;
            this.StateName = stateName;
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
            this.Animator = animator;

            if (this.HashAnimation != 0.0f)
            {
                this.ParameterType = this.TypeOfParamAnimator(this.HashAnimation);
            }
        }

        protected AnimatorControllerParameterType TypeOfParamAnimator(int hashAnimation)
        {
            AnimatorControllerParameter[] parameters = this.Animator.parameters;
            var animatorController = parameters.Where(p => p.nameHash == hashAnimation).FirstOrDefault();
            return animatorController.type;
        }

        public bool KeyHasBeenPressed()
        {
            return this.KeyToPress != KeyCode.None && Input.GetKeyDown(this.KeyToPress);
        }

        public bool ButtonHasBeenPressed()
        {
            return this.ButtonToPress != null && Input.GetButtonDown(this.ButtonToPress);
        }

        public virtual void DoAction()
        {
            switch (this.ParameterType)
            {
                case AnimatorControllerParameterType.Bool:
                    this.Animator.SetBool(this.HashAnimation, true);
                    break;

                case AnimatorControllerParameterType.Trigger:
                    this.Animator.SetTrigger(this.HashAnimation);
                    break;
            }
        }

        public virtual void DoAction(float value)
        {
            switch (this.ParameterType)
            {
                case AnimatorControllerParameterType.Float:
                    this.Animator.SetFloat(this.HashAnimation, value);
                    break;
            }
        }

        public virtual void DoAction(int value)
        {
            switch (this.ParameterType)
            {
                case AnimatorControllerParameterType.Int:
                    this.Animator.SetInteger(this.HashAnimation, value);
                    break;
            }
        }
    }
}
