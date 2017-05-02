using nseh.Gameplay.Base.Interfaces;
using System.Linq;
using UnityEngine;
using System;
using System.Collections;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    public abstract class HandledAction : MonoBehaviour, IAction
    {
        #region Protected Properties

        protected int _hash;

        protected string _stateName;
        protected string _button;

        protected PlayerInfo _playerInfo;
        protected Animator _animator;
        protected AnimatorControllerParameterType _paramType;

        #endregion

        #region Public C# Properties

        public int Hash { get { return _hash; } }

        public string StateName { get { return _stateName; } }

        public AnimatorControllerParameterType ParamType { get { return _paramType; } }

        public string Button { get { return _button; } }

        public Animator Animator { get { return _animator; } }

        #endregion

        #region Protected Methods

        protected AnimatorControllerParameterType TypeOfParamAnimator(int hash)
        {
            AnimatorControllerParameter[] parameters = _animator.parameters;
            AnimatorControllerParameter animatorController = parameters.Where(p => p.nameHash == hash).FirstOrDefault();

            return animatorController.type;
        }

        #endregion

        #region Input Support

        public bool ButtonHasBeenPressed()
        {
            return !String.IsNullOrEmpty(_button) && Input.GetButtonDown(_button);
        }

        public bool ButtonIsHoldDown()
        {
            return !String.IsNullOrEmpty(_button) && Input.GetButton(_button);
        }

        public bool ButtonHasBeenReleased()
        {
            return !String.IsNullOrEmpty(_button) && Input.GetButtonUp(_button);
        }

        public bool ReceiveInput()
        {
            return ButtonHasBeenPressed() || ButtonIsHoldDown() || ButtonHasBeenReleased();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StateName;
        }

        #endregion

        #region Virtual Methods

        public virtual void StartAction()
        {
            switch (_paramType)
            {
                case AnimatorControllerParameterType.Bool:
                    Animator.SetBool(_hash, true);
                    break;

                case AnimatorControllerParameterType.Trigger:
                    Animator.SetTrigger(_hash);
                    break;
            }
        }

        public virtual void StartAction(float value)
        {
            switch (_paramType)
            {
                case AnimatorControllerParameterType.Float:
                    Animator.SetFloat(_hash, value);
                    break;
            }
        }

        public virtual void StartAction(int value)
        {
            switch (_paramType)
            {
                case AnimatorControllerParameterType.Int:
                    Animator.SetInteger(_hash, value);
                    break;
            }
        }

        public virtual void StopAction()
        {
            switch (_paramType)
            {
                case AnimatorControllerParameterType.Bool:
                    Animator.SetBool(_hash, false);
                    break;

                case AnimatorControllerParameterType.Trigger:
                    Animator.ResetTrigger(_hash);
                    break;
            }
        }

        #endregion
    }
}
