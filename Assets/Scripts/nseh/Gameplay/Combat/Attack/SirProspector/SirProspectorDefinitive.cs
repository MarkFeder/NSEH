using nseh.Gameplay.Animations.Receivers.SirProspector;
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
        private float _percent;
        [SerializeField]
        private float _seconds;
        [SerializeField]
        private GameObject _particle;
        [SerializeField]
        private GameObject _particleSword;
        [SerializeField]
        private MeshRenderer _sword;
        [SerializeField]
        private MeshRenderer _shovel;

        private SirProspectorAnimationEventReceiver _receiver;
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
/*
                GameObject particleGameObject = Instantiate(_particle, _playerInfo.particleBodyPos.transform.position, _playerInfo.particleBodyPos.transform.rotation, _playerInfo.transform);
                foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particle_aux.Play();
                }

                Destroy(particleGameObject, 3f);
                */

                StartCoroutine(ExecuteDefinitiveAction());
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Execute SirProspector's definitive attack.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExecuteDefinitiveAction()
        {
            // Deactivate/Activate items
            _enabled = false;
            _shovel.enabled = false;
            _sword.enabled = false;
            _particleSword.SetActive(true);
            GameObject particleGameObject = Instantiate(_particle, _playerInfo.particleFootPos.transform.position, _playerInfo.particleBodyPos.transform.rotation, _playerInfo.particleBodyPos.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 5f);
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
            _particleSword.SetActive(false);
        }

        /// <summary>
        /// Setup the callbacks on this class to trigger animation events.
        /// </summary>
        private void SetupAnimationEvents()
        {
            // Setup proxy receivers
            _receiver = transform.root.gameObject.GetComponent<SirProspectorAnimationEventReceiver>();
            _receiver.OnHideSwordCallback += OnHideSword;
            _receiver.OnShowSwordCallback += OnShowSword; 
        }

        /// <summary>
        /// This is an animation event triggered by the animation. It hides SirProspector's sword.
        /// </summary>
        /// <param name="animationEvent"></param>
        private void OnHideSword(AnimationEvent animationEvent)
        {
            _sword.enabled = false;
        }

        /// <summary>
        /// This is an animation event triggered by the animation. It shows SirProspector's sword.
        /// </summary>
        /// <param name="animationEvent"></param>
        private void OnShowSword(AnimationEvent animationEvent)
        {
            _sword.enabled = true;
        }

        /// <summary>
        /// Unsubscribe from events.
        /// </summary>
        private void OnDestroy()
        {
			_receiver.OnHideSwordCallback -= OnHideSword;
			_receiver.OnShowSwordCallback -= OnShowSword;
		}

        #endregion
    }
}
