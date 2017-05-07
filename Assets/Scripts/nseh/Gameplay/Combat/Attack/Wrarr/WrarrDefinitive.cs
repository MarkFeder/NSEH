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

        [SerializeField]
        private GameObject _rockMesh;
        [SerializeField]
        private Transform _bone;

        [SerializeField]
        private float _rockForce;
        [SerializeField]
        [Range(0, 1)]
        private float _startTime;
        [SerializeField]
        [Range(0, 1)]
        private float _endTime;


        private AnimationClip _animationClip;
        private WrarrAnimationEventReceiver _receiver;
        private const string _clipName = "WrarrUltimateSkill";

        #endregion

        #region Protected Methods

        protected override void Start()
        {
            base.Start();

            if (!(_startTime < _endTime))
            {
                Debug.LogError("startTime must be less than endTime");
                return;
            }

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
                AnimationEvent startLaunchRockEvent = new AnimationEvent();
                startLaunchRockEvent.functionName = "OnStartLaunchRock";
                startLaunchRockEvent.messageOptions = SendMessageOptions.RequireReceiver;
                startLaunchRockEvent.time = _animationClip.length * _startTime;

                AnimationEvent stopLaunchRockEvent = new AnimationEvent();
                stopLaunchRockEvent.functionName = "OnStopLaunchRock";
                stopLaunchRockEvent.messageOptions = SendMessageOptions.RequireReceiver;
                stopLaunchRockEvent.time = _animationClip.length * _endTime;

                // Add events to this animation clip
                _animationClip.events = new AnimationEvent[] { startLaunchRockEvent, stopLaunchRockEvent };

                // Setup proxy receivers
                _receiver = transform.root.gameObject.GetComponent<WrarrAnimationEventReceiver>();
                _receiver.OnStartLaunchRockCallback += OnStartLaunchRock;
                _receiver.OnStopLaunchRockCallback += OnStopLaunchRock;
            }
            else
            {
                Debug.LogError("Could not setup animation events for Wrarr definitive");
            }
        }

        private void OnStartLaunchRock(AnimationEvent animationEvent)
        {
            _rockRuntime = Instantiate(_rockMesh, _bone.position, Quaternion.identity);

            _rockRuntime.transform.parent = _bone.transform;
            _rockRuntime.transform.localPosition = Vector3.zero;

            SetUnMovableRock();
        }

        private void OnStopLaunchRock(AnimationEvent animationEvent)
        {
            if (_rockRuntime != null)
            {
                SetMovableRock();

                _rockRuntime.transform.parent = null;

                Vector3 vForward = transform.TransformDirection(Vector3.forward);
                _rockBody.AddForce(vForward * _rockForce, ForceMode.Force);
            }
        }

        private void SetUnMovableRock()
        {
            _rockBody = _rockRuntime.transform.GetChild(0).GetComponent<Rigidbody>();
            // _rockBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            _rockBody.velocity = Vector3.zero;
            _rockBody.useGravity = false;
            _rockBody.isKinematic = true;
        }

        private void SetMovableRock()
        {
            _rockBody.useGravity = true;
            _rockBody.isKinematic = false;
        }

        #endregion
    }
}
