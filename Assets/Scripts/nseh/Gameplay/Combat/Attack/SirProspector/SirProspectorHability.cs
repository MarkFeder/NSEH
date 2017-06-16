using nseh.Gameplay.Animations.Receivers.SirProspector;
using nseh.Utils.EditorCustomization;
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
        private float _bonification;
        [SerializeField]
        private float _seconds;
        [SerializeField]
        private GameObject _particle;
        [SerializeField]
        private GameObject _particleDefense;

        private float _usedTime;
        private SirProspectorAnimationEventReceiver _receiver;

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

                GameObject particleGameObjectDefense = Instantiate(_particleDefense, _playerInfo.particleBodyPos.transform.position, _playerInfo.particleBodyPos.transform.rotation, _playerInfo.particleBodyPos.transform);
                foreach (ParticleSystem particle_aux in particleGameObjectDefense.GetComponentsInChildren<ParticleSystem>())
                {
                    particle_aux.Play();
                }

                Destroy(particleGameObjectDefense, _seconds);

                // Disable this attack until it can be used again
                _enabled = false;

                // Apply bonification defense
                _playerInfo.PlayerHealth.BonificationDefenseForSeconds(_bonification, _seconds);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Setup the callbacks on this class to trigger animation events.
        /// </summary>
        private void SetupAnimationEvents()
        {
            // Setup proxy receivers
            _receiver = transform.root.gameObject.GetComponent<SirProspectorAnimationEventReceiver>();
            _receiver.OnDeactivateItemCallback += OnDeactivateItem;
            _receiver.OnActivateItemCallback += OnActivateItem;
        }

        /// <summary>
        /// This is an animation event triggered by the animation. It deactivates the special item.
        /// </summary>
        /// <param name="animationEvent"></param>
        private void OnDeactivateItem(AnimationEvent animationEvent)
        {
            // Deactivate special item
            if (_specialItem != null)
            {
                _specialItem.enabled = false;
            }
        }

        /// <summary>
        /// This is an animation event triggered by the animation. It activates the special item.
        /// </summary>
        /// <param name="animationEvent"></param>
        private void OnActivateItem(AnimationEvent animationEvent)
        {
            // Activate special item
            if (_specialItem != null)
            {
                _specialItem.enabled = true;
            }
        }

		/// <summary>
		/// Unsubscribe from events.
		/// </summary>
		private void OnDestroy()
		{
			_receiver.OnDeactivateItemCallback -= OnDeactivateItem;
			_receiver.OnActivateItemCallback -= OnActivateItem;
		}

        #endregion
    }
}
