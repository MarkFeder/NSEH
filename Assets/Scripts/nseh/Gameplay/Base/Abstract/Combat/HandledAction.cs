﻿using System;
using System.Collections.Generic;
using System.Linq;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Audio;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Gameplay.Base.Abstract
{
    public abstract class HandledAction : MonoBehaviour, IAction
    {
        #region Protected Properties

        [SerializeField]
        protected List<AudioClip> _audioClips;
        protected List<AudioController> _audioControllers;

        protected int _hash;
		protected string _stateName;
		protected string _button;

        protected PlayerInfo _playerInfo;
        protected Animator _animator;
        protected AnimatorControllerParameterType _paramType;

		protected bool _enabled;

		#endregion

		#region Public C# Properties

		public int Hash { get { return _hash; } }

        public string StateName { get { return _stateName; } }

        public AnimatorControllerParameterType ParamType { get { return _paramType; } }

        public string Button { get { return _button; } }

        public Animator Animator { get { return _animator; } }

        public bool IsEnabled { get { return _enabled; } set { _enabled = value; } }

        #endregion

        #region Protected Methods

        protected AnimatorControllerParameterType TypeOfParamAnimator(int hash)
        {
            AnimatorControllerParameter[] parameters = _animator.parameters;
            AnimatorControllerParameter animatorController = parameters.Where(p => p.nameHash == hash).FirstOrDefault();

            return animatorController.type;
        }

        protected virtual void Start()
        {
            _audioControllers = new List<AudioController>();

			// Register sounds
			for (int i = 0; i < _audioClips.Count; i++)
			{
				AudioController audioController = GameManager.Instance.SoundManager.LoadSoundFX(_audioClips[i], false);
				if (audioController == null)
				{
					Debug.LogError(string.Format("The AudioController could not be instantiated for the audioclip: {0}", _audioClips[i].name));
					continue;
				}

				_audioControllers.Add(audioController);
			}
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
