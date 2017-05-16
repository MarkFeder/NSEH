using System.Linq;
using nseh.Gameplay.Animations.Receivers.Wrarr;
using nseh.Gameplay.Combat.Attack.Wrarr;
using nseh.Utils.Helpers;
using UnityEngine;
using Layers = nseh.Utils.Constants.Layers;

namespace nseh.Gameplay.Combat.Attack.SirProspector
{
    public class WrarrHability : CharacterAttack
    {
        #region Private Properties

        [SerializeField]
        private WaveComponent _wave;
        [SerializeField]
        [Range(0, 1)]
        private float _startRoarTime;
        [SerializeField]
        [Range(0, 1)]
        private float _stopRoarTime;
        [SerializeField]
        private float _force;


        [SerializeField]
        private GameObject _particle;

        private int _playerLayer;

        private AnimationClip _animationClip;
        private WrarrAnimationEventReceiver _receiver;
        private const string _clipName = "WrarrBasicSkill";

        #endregion

        #region Protected Methods

        protected override void Start()
        {
            base.Start();

            if (!(_startRoarTime < _stopRoarTime))
            {
                Debug.LogError("startRoarTime must be less than stopRoarTime");
                return;
            }

            _playerLayer = LayerMask.NameToLayer(Layers.PLAYER);
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
                GameObject particleGameObject = Instantiate(_particle, _playerInfo.particleBodyPos.transform.position, _playerInfo.particleBodyPos.transform.rotation, _playerInfo.particleBodyPos.transform);
                foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particle_aux.Play();
                }

                Destroy(particleGameObject, 3f);
                _enabled = false;
            }
        }

        #endregion

        #region Private Methods

        private void SetupAnimationEvents()
        {
            // Get this animation clip
            _animationClip = _playerInfo.Animator.runtimeAnimatorController.animationClips
                             .Where(clip => clip.name == _clipName).FirstOrDefault();

            if (_animationClip != null)
            {
                // Setup events
                AnimationEventExtensions.CreateAnimationEventForClip(ref _animationClip, "OnStartRoar", _startRoarTime * _animationClip.length);
                AnimationEventExtensions.CreateAnimationEventForClip(ref _animationClip, "OnEndRoar", _stopRoarTime * _animationClip.length);

                // Setup proxy receivers
                _receiver = transform.root.gameObject.GetComponent<WrarrAnimationEventReceiver>();
                _receiver.OnStartRoarCallback += OnStartRoar;
                _receiver.OnEndRoarCallback += OnEndRoar;
            }
            else
            {
                Debug.LogError("Could not setup animation events for Wrarr hability");
            }
        }

        private void OnStartRoar(AnimationEvent animationEvent)
        {
            // If hits enemies, then execute logic
            // Draw raycast
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(_playerInfo.particleBodyPos.position, forward, Color.green, 3.0f);

            // Activate warr's wave
            _wave.enabled = true;
            _wave.Sender = _playerInfo;
            _wave.WaveCollider.enabled = true;
            _wave.Damage = _currentDamage;
            _wave.Force = _force;
        }

        private void OnEndRoar(AnimationEvent animationEvent)
        {
            // Deactivate Wrarr's wave
            _wave.enabled = false;
            _wave.Sender = null;
            _wave.WaveCollider.enabled = false;
            _wave.Damage = 0.0f;
            _wave.Force = 0.0f;

            // Activate this attack again
            _enabled = true;
        }

        #endregion
    }
}
