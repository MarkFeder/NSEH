using nseh.Gameplay.Animations.Receivers.Wrarr;
using nseh.Gameplay.Combat.Attack.Wrarr;
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
        private GameObject _particle;
        
        private WrarrAnimationEventReceiver _receiver;
        private int _playerLayer;

        #endregion
        
        #region Protected Methods

        protected override void Start()
        {
            base.Start();

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

        /// <summary>
        /// Setup the callbacks on this class to trigger animation events.
        /// </summary>
        private void SetupAnimationEvents()
        {
            // Setup proxy receivers
            _receiver = transform.root.gameObject.GetComponent<WrarrAnimationEventReceiver>();
            _receiver.OnStartRoarCallback += OnStartRoar;
            _receiver.OnEndRoarCallback += OnEndRoar;
        }

        /// <summary>
        /// This is an animation event triggered by the animation. Wrarr starts roaring.
        /// </summary>
        /// <param name="animationEvent"></param>
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
        }

        /// <summary>
        /// This is an animation event triggered by the animation. Wrarr stops roaring.
        /// </summary>
        /// <param name="animationEvent"></param>
        private void OnEndRoar(AnimationEvent animationEvent)
        {
            // Deactivate Wrarr's wave
            _wave.enabled = false;
            _wave.Sender = null;
            _wave.WaveCollider.enabled = false;
            _wave.Damage = 0.0f;

            // Activate this attack again
            _enabled = true;
        }

		/// <summary>
		/// Unsubscribe from events.
		/// </summary>
		private void OnDestroy()
		{
			_receiver.OnStartRoarCallback -= OnStartRoar;
			_receiver.OnEndRoarCallback -= OnEndRoar;
		}

        #endregion
    }
}
