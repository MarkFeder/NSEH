using nseh.Gameplay.Animations.Receivers.SirProspector;
using nseh.Utils.EditorCustomization;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Combat.Attack.SirProspector
{
    [HidePropertiesInInspector("_initialDamage")]
    public class SirProspectorHability : CharacterAttack
    {
        #region Private Properties

        [SerializeField]
        private MeshRenderer _specialItem;

        [SerializeField]
        private GameObject _sword;
        [SerializeField]
        private GameObject _shovel;

        [SerializeField]
        [Range(0, 1)]
        private float _deactivateItemTime;
        [SerializeField]
        [Range(0, 1)]
        private float _activateItemTime;

        [SerializeField]
        private float _bonification;
        [SerializeField]
        private float _seconds;


        [SerializeField]
        private GameObject _particle;

        private float _usedTime;

        private SirProspectorAnimationEventReceiver _receiver;
        private AnimationClip _animationClip;
        private const string _clipName = "SPECIALSKILL";

        #endregion

        #region Protected Methods

        protected override void Start()
        {
            base.Start();

            _initialDamage = 0.0f;
            _usedTime = 0.0f;

            SetupAnimationEvents();
        }

        protected void Update()
        {
            _usedTime += Time.deltaTime;

            if (!_enabled && _usedTime >= _seconds)
            {
                _usedTime = 0.0f;
                _enabled = true;
            }
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
                // Disable this attack until it can be used again
                _enabled = false;

                // Apply bonification defense
                _playerInfo.PlayerHealth.BonificationDefenseForSeconds(_bonification, _seconds);
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
                AnimationEventExtensions.CreateAnimationEventForClip(ref _animationClip, "OnDeactivateItem", _deactivateItemTime * _animationClip.length);
                AnimationEventExtensions.CreateAnimationEventForClip(ref _animationClip, "OnActivateItem", _activateItemTime * _animationClip.length);

                // Setup proxy receivers
                _receiver = transform.root.gameObject.GetComponent<SirProspectorAnimationEventReceiver>();
                _receiver.OnDeactivateItemCallback += OnDeactivateItem;
                _receiver.OnActivateItemCallback += OnActivateItem;
            }
            else
            {
                Debug.LogError("Could not setup animation events for SirProspector hability");
            }
        }

        private void OnDeactivateItem(AnimationEvent animationEvent)
        {
            // Deactivate special item
            if (_specialItem != null)
            {
                _specialItem.enabled = false;
            }
        }

        private void OnActivateItem(AnimationEvent animationEvent)
        {
            // Activate special item
            if (_specialItem != null)
            {
                _specialItem.enabled = true;
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _receiver.OnDeactivateItemCallback -= OnDeactivateItem;
            _receiver.OnActivateItemCallback -= OnActivateItem;
        }

        #endregion
    }
}
