using nseh.Gameplay.Animations.Receivers.Wrarr;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Combat.Attack.Wrarr
{
    public class WrarrDefinitive : CharacterAttack
    {
        #region Private Properties

        private GameObject _rockRuntime;
        private Rigidbody _rockBody;
        private WrarrAnimationEventReceiver _receiver;

        [SerializeField]
        private GameObject _rockMesh;
        [SerializeField]
        private Transform _bone;
        [SerializeField]
        private float _rockForce;
        [SerializeField]
        private GameObject _particle;

        #endregion

        #region Protected Methods

        protected override void Start()
        {
            base.Start();

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
            _receiver.OnStartLaunchRockCallback += OnStartLaunchRock;
            _receiver.OnStopLaunchRockCallback += OnStopLaunchRock;
        }

        /// <summary>
        /// This is an animation event triggered by the animation. It creates the rock.
        /// </summary>
        /// <param name="animationEvent"></param>
        private void OnStartLaunchRock(AnimationEvent animationEvent)
        {
            if (_rockRuntime == null)
            {
                // Disable this attack until rock has been launched
                _enabled = false;

                // Setup rock on runtime
                _rockRuntime = Instantiate(_rockMesh, _bone.position, Quaternion.identity);
                RockComponent component = _rockRuntime.transform.GetChild(0).GetComponent<RockComponent>();
                component.Damage = _currentDamage;
                component.Sender = _playerInfo;

                // Set the rock to use parent's local position and rotation
                _rockRuntime.transform.parent = _bone.transform;
                _rockRuntime.transform.localPosition = Vector3.zero;
                _rockRuntime.transform.localRotation = _bone.localRotation;

                SetUnMovableRock(); 
            }
        }

        /// <summary>
        /// This is an animation event triggered by the animation. It detaches the rock from wrarr in forward direction.
        /// </summary>
        /// <param name="animationEvent"></param>
        private void OnStopLaunchRock(AnimationEvent animationEvent)
        {
            if (_rockRuntime != null)
            {
                SetMovableRock();

                // Detach rock from parent
                _rockRuntime.transform.parent = null;

                // Apply force in forward direction
                Vector3 vForward = transform.TransformDirection(Vector3.forward);
                _rockBody.AddForce(vForward * _rockForce, ForceMode.Force);

                // Enable this attack again
                _enabled = true;
            }
        }

        /// <summary>
        /// Set the rock to be unmovable.
        /// </summary>
        private void SetUnMovableRock()
        {
            _rockBody = _rockRuntime.transform.GetChild(0).GetComponent<Rigidbody>();
            _rockBody.velocity = Vector3.zero;
            _rockBody.useGravity = false;
            _rockBody.isKinematic = true;
        }

        /// <summary>
        /// Set the rock to be movable.
        /// </summary>
        private void SetMovableRock()
        {
            _rockBody.useGravity = true;
            _rockBody.isKinematic = false;
        }

		/// <summary>
		/// Unsubscribe from events.
		/// </summary>
		private void OnDestroy()
		{
			_receiver.OnStartLaunchRockCallback -= OnStartLaunchRock;
			_receiver.OnStopLaunchRockCallback -= OnStopLaunchRock;
		}

        #endregion
    }
}
