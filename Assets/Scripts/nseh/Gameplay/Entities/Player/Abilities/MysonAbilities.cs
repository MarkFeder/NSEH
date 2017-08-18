using UnityEngine;
using System.Collections;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;

namespace nseh.Gameplay.Player.Abilities
{
    public class MysonAbilities : MonoBehaviour
    {

        #region Private Properties

        [Header("Definitive GameObjects")]
        [SerializeField]
        private Renderer _body;
        [SerializeField]
        private Renderer _headRenderer;
        [SerializeField]
        private GameObject _headObject;
        [Space(10)]

        [Header("Parameters Abilities")]
        [SerializeField]
        private float _timeGhost;
        [Space(10)]

        [Header("Parameters Abilities")]
        [SerializeField]
        private GameObject _particlePropulsor;
        [SerializeField]
        private GameObject _rightHand;
        [SerializeField]
        private GameObject _leftHand;

        [Header("Additional Sounds")]
        [SerializeField]
        private AudioClip _broFist;


        private PlayerInfo _playerInfo;

        #endregion

        #region Private Methods

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
        }

        #endregion

        #region Animation Events

        public virtual void OnBroFistSound(AnimationEvent animationEvent)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(_broFist, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        public virtual void OnActivateParticle(int mano)
        {
            if(mano == 0)
            {
                GameObject particleGameObject = Instantiate(_particlePropulsor, _leftHand.transform.position, this.gameObject.transform.rotation);
                foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particle_aux.Play();
                }
                Destroy(particleGameObject, 0.3f);
            }

            else
            {
                GameObject particleGameObject = Instantiate(_particlePropulsor, _rightHand.transform.position, this.gameObject.transform.rotation);
                foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particle_aux.Play();
                }
                Destroy(particleGameObject, 0.3f);
            }
        }

        public virtual void OnActivateHead(AnimationEvent animationEvent)
        {
            _headObject.GetComponent<Collider>().enabled = true;
        }

        public virtual void OnHideHead(AnimationEvent animationEvent)
        {
            _headRenderer.enabled = false;
            _headObject.GetComponent<Collider>().enabled = false;
        }

        public virtual void OnHideBody(AnimationEvent animationEvent)
        {
            _body.enabled = false;
            Physics.IgnoreLayerCollision(8, 8, true);
            Physics.IgnoreLayerCollision(8, 12, true);
            _playerInfo.EnableAttack = false;
            StartCoroutine(Ghost(_timeGhost));

        }

        public virtual void OnShowHead(AnimationEvent animationEvent)
        {
            _headRenderer.enabled = true;
        }

        public IEnumerator Ghost(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            _body.enabled = true;
            _playerInfo.EnableAttack = true;
            Physics.IgnoreLayerCollision(8, 8, false);
            Physics.IgnoreLayerCollision(8, 12, false);

        }

        #endregion

    }
}
