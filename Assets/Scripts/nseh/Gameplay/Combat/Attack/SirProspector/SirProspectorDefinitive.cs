﻿using nseh.Gameplay.Animations.Receivers.SirProspector;
using nseh.Utils.EditorCustomization;
using nseh.Utils.Helpers;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Combat.Attack.SirProspector
{
    [HidePropertiesInInspector("_initialDamage")]
    public class SirProspectorDefinitive : CharacterAttack
    {
        #region Private Properties

        [SerializeField]
        [Range(0,1)]
        private float _hideSwordTime;
        [SerializeField]
        [Range(0, 1)]
        private float _showSwordTime;
        [SerializeField]
        private float _percent;
        [SerializeField]
        private float _seconds;

        [SerializeField]
        private MeshRenderer _sword;
        [SerializeField]
        private MeshRenderer _shovel;

        private SirProspectorAnimationEventReceiver _receiver;
        private AnimationClip _animationClip;

        private const string _clipName = "ULTIMATESKILL";
        private const string _coroutinesGroup = "SirProspectorDefinitive";

        #endregion

        #region Protected Methods

        protected override void Start()
        {
            base.Start();

            _initialDamage = 0.0f;

            _sword.enabled = false;
            _shovel.enabled = true;

            SetupAnimationEvents();
        }

        #endregion

        #region Public Methods

        public override void StartAction()
        {
            if (_enabled && CanStartSpecialHability())
            {
                ReduceEnergyOnSpecialHability();

                base.StartAction();
                StartCoroutine(ExecuteDefinitiveAction());
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator ExecuteDefinitiveAction()
        {
            // Deactivate/Activate items
            _enabled = false;
            _shovel.enabled = false;
            _sword.enabled = false;

            // Trigger damage increment
            var attacks = _playerInfo.PlayerCombat.Actions.OfType<CharacterAttack>();
            RunInfo info = null;

            foreach (CharacterAttack attack in attacks)
            {
                info = attack.IncreaseDamageForSecondsExternal(_percent, _seconds).ParallelCoroutine(_coroutinesGroup);
            }
            while (info.count > 0) yield return null;

            // Deactivate/Activate items
            _shovel.enabled = true;
            _sword.enabled = false;
            _enabled = true;
        }

        private void SetupAnimationEvents()
        {
            // Get this animation clip
            _animationClip = _playerInfo.Animator.runtimeAnimatorController.animationClips
                             .Where(clip => clip.name == _clipName).FirstOrDefault();

            if (_animationClip != null)
            {
                // Setup events
                AnimationEventExtensions.CreateAnimationEventForClip(ref _animationClip, "OnHideSword", _hideSwordTime * _animationClip.length);
                AnimationEventExtensions.CreateAnimationEventForClip(ref _animationClip, "OnShowSword", _showSwordTime * _animationClip.length);

                // Setup proxy receivers
                _receiver = transform.root.gameObject.GetComponent<SirProspectorAnimationEventReceiver>();
                _receiver.OnHideSwordCallback += OnHideSword;
                _receiver.OnShowSwordCallback += OnShowSword; 
            }
            else
            {
                Debug.LogError("Could not setup animation events for SirProspector definitive");
            }
        }

        private void OnHideSword(AnimationEvent animationEvent)
        {
            _sword.enabled = false;
        }

        private void OnShowSword(AnimationEvent animationEvent)
        {
            _sword.enabled = true;
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _receiver.OnHideSwordCallback -= OnHideSword;
            _receiver.OnShowSwordCallback -= OnShowSword;
        }

        #endregion
    }
}
