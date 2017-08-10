using UnityEngine;
using nseh.Gameplay.Entities.Player;
using nseh.Gameplay.Combat.Special.Granhilda;
using nseh.Managers.Main;
using BaseParameters = nseh.Utils.Constants.PlayerInfo;


namespace nseh.Gameplay.Player.Abilities
{

    public class GranhildaAbilities : MonoBehaviour
    {

        #region Private Properties

        [Header("Particles")]
        [SerializeField]
        private GameObject _particleAAA;
        [SerializeField]
        private GameObject _particleDragonFire;
        [SerializeField]
        private GameObject _particleDeath;
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
        [SerializeField]
        private GameObject _colliderDefinitive;

        [SerializeField]
        private AudioClip _soundIdle;

        private PlayerInfo _playerInfo;
        private PlayerMovement _playerMovement;

        private bool _canJump;
        private float _jumpHeight;
        private Collider _collider;
        private ChargeComponent _auxCharge;

        #endregion

        #region Private Methods

        void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _collider = GetComponent<Collider>();
            _playerMovement = GetComponent<PlayerMovement>();
            _canJump = false;
            _jumpHeight = BaseParameters.JUMPHEIGHT;
        }

        void Update()
        {
            if(_canJump && _playerMovement.grounded && _playerInfo.JumpPressed)
                _playerInfo.Body.velocity = new Vector3(_playerInfo.Body.velocity.x, _jumpHeight, 0);
        }

        #endregion

        #region Animation Events

        public virtual void OnParticlesDeath(AnimationEvent animationEvent)
        {

            GameObject particleGameObject = Instantiate(_particleDeath, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 2.5f);
        }

        public virtual void OnParticlesAttackAAA(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleAAA, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 0.5f);
        }

        public virtual void OnParticlesDragonFire(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleAbility, _playerInfo.ParticleBodyPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 1f);
        }

        public virtual void OnParticlesAbility(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleAbility, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
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

            Destroy(particleGameObject, 2f);
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
            Physics.IgnoreLayerCollision(8, 8, true);
            Physics.IgnoreLayerCollision(8, 12, true);
            _colliderDefinitive.GetComponent<Collider>().enabled = true;
            _colliderDefinitive.GetComponent<ChargeComponent>().enabled = true;
            Vector3 vForward = transform.TransformDirection(Vector3.forward);
            _playerInfo.Body.velocity = new Vector3 (_forceDefinitive * vForward.x, 0, 0);
            _canJump = true;

        }

        public virtual void OnStopDefinitive(AnimationEvent animationEvent)
        {
            Physics.IgnoreLayerCollision(8, 8, false);
            Physics.IgnoreLayerCollision(8, 12, false);
            _colliderDefinitive.GetComponent<Collider>().enabled = false;
            _colliderDefinitive.GetComponent<ChargeComponent>().enabled = false;
            _playerInfo.Body.velocity = Vector3.zero;
            _canJump = false;
        }

        public virtual void OnSoundIdle(AnimationEvent animationEvent)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(_soundIdle, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        #endregion
    }

}
