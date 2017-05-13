using nseh.Gameplay.Animations.Receivers.Wrarr;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.EditorCustomization;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;
using Layers = nseh.Utils.Constants.Layers;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Attack.SirProspector
{
    [HidePropertiesInInspector("_initialDamage")]
    public class WrarrHability : CharacterAttack
    {
        #region Private Properties

        [SerializeField]
        [Range(0, 1)]
        private float _startRoarTime;
        [SerializeField]
        [Range(0, 1)]
        private float _stopRoarTime;
        [SerializeField]
        private float _force;
        [SerializeField]
        private float _distance;
        [SerializeField]
        private float _percent;
        [SerializeField]
        private float _seconds;
        [SerializeField]
        private float _reusedTime;

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
            if (_enabled)
            {
                base.StartAction();
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
            Vector3 forward = transform.TransformDirection(Vector3.forward) * _distance;
            Debug.DrawRay(_playerInfo.particleBodyPos.position, forward, Color.green, 3.0f);

            RaycastHit[] hits = Physics.RaycastAll(_playerInfo.particleBodyPos.position, forward, _distance);
            for (int i = 0; i < hits.Length; ++i)
            {
                GameObject enemy = hits[i].transform.gameObject;
                Debug.Log("Hitted: " + enemy.name);

                if (enemy.CompareTag(Tags.PLAYER_BODY))
                {
                    PlayerInfo enemyInfo = enemy.GetComponent<PlayerInfo>();
                    if (enemyInfo != null)
                    {
                        enemyInfo.Body.AddForceAtPosition(forward * _force, hits[i].point, ForceMode.Impulse);
                        enemyInfo.PlayerMovement.DecreaseSpeedForSeconds(_percent, _seconds);
                    }
                }
            }
        }

        private void OnEndRoar(AnimationEvent animationEvent)
        {
            // Reestablish timer
            _enabled = false;
        }

        #endregion
    }
}
