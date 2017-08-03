using UnityEngine;
using nseh.Gameplay.Entities.Player;
using nseh.Gameplay.Combat.Special.Granhilda;
using BaseParameters = nseh.Utils.Constants.PlayerInfo;


namespace nseh.Gameplay.Player.Abilities
{

    public class GranhildaAbilities : MonoBehaviour
    {

        #region Private Properties

        [Header("Particles")]
        [SerializeField]
        private GameObject _particleAbility;
        [SerializeField]
        private GameObject _particleDefinitive;
        [Space(10)]

        [Header("Ability Parameters")]

        [SerializeField]
        private int _forceDefinitive;
        [Space(10)]
        private int _damageDefinitive;
        [Space(10)]

        [Header("GameObjects")]
        [SerializeField]
        private GameObject _staticShield;
        [SerializeField]
        private Renderer _hammer;

        private PlayerInfo _playerInfo;
        private PlayerMovement _playerMovement;

        private bool _canJump;
        private float _jumpHeight;

        #endregion

        #region Private Methods

        void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _playerMovement = GetComponent<PlayerMovement>();
            _canJump = false;
            _jumpHeight = BaseParameters.JUMPHEIGHT;
        }

        void Update()
        {
            if(_canJump && _playerMovement.grounded)
                _playerInfo.Body.velocity = new Vector3(_playerInfo.Body.velocity.x, _jumpHeight, 0);
        }

        #endregion

        #region Animation Events

        public virtual void OnParticlesAbility(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleAbility, _playerInfo.ParticleBodyPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 3f);
        }

        public virtual void OnParticlesDefinitive(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleDefinitive, _playerInfo.ParticleBodyPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 3f);
        }

        public virtual void OnHideHammer(AnimationEvent animationEvent)
        {
            _hammer.enabled = false;
        }

        public virtual void OnShowHammer(AnimationEvent animationEvent)
        {
            _hammer.enabled = true;
        }

        public virtual void OnActivateStaticShield(AnimationEvent animationEvent)
        {
            _staticShield.GetComponent<StaticShieldComponent>().enabled = true;
        }

        public virtual void OnAddForce(AnimationEvent animationEvent)
        {
            _playerInfo.Body.isKinematic = false;
            _playerInfo.Body.AddForce(Vector3.forward * _forceDefinitive);
            _canJump = true;

        }

        public virtual void OnStopDefinitive(AnimationEvent animationEvent)
        {
            _playerInfo.Body.isKinematic = true;
            _canJump = false;
        }

            #endregion
        }

}
