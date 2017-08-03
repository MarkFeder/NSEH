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
        private Collider _definitiveCollider;

        private PlayerInfo _playerInfo;
        private PlayerMovement _playerMovement;

        private bool _canJump;
        private float _jumpHeight;
        private Collider _collider;

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
            Debug.Log(_canJump + " " + _playerMovement.grounded + " " + _playerInfo.JumpPressed);
            if(_canJump && _playerMovement.grounded && _playerInfo.JumpPressed)
                _definitiveCollider.GetComponent<Rigidbody>().velocity = new Vector3(_playerInfo.Body.velocity.x, _jumpHeight, 0);
        }

        #endregion

        #region Animation Events

        public virtual void OnParticlesDeath(AnimationEvent animationEvent)
        {

            GameObject particleGameObject = Instantiate(_particleDeath, this.gameObject.transform.position, _particleDeath.transform.rotation);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 1f);
        }

        public virtual void OnParticlesAttackAAA(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleAbility, _playerInfo.ParticleBodyPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 1f);
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
            GameObject particleGameObject = Instantiate(_particleAbility, _playerInfo.ParticleBodyPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 3f);
        }

        public virtual void OnParticlesDefinitive(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleDefinitive, this.gameObject.transform.position, _particleDefinitive.transform.rotation, this.gameObject.transform);
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
            _definitiveCollider.enabled = true;
            _collider.enabled = false;
            
            Vector3 vForward = transform.TransformDirection(Vector3.forward);
            _definitiveCollider.GetComponent<Rigidbody>().AddForce(new Vector3(_forceDefinitive * vForward.x, 0, 0), ForceMode.Force);
            _canJump = true;

        }

        public virtual void OnStopDefinitive(AnimationEvent animationEvent)
        {
            _playerInfo.Body.isKinematic = true;
            _collider.enabled = true;
            _definitiveCollider.enabled = false;
            _playerInfo.Body.velocity = Vector3.zero;
            _canJump = false;
        }

            #endregion
        }

}
