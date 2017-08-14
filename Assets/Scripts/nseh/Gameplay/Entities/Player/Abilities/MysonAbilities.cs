using UnityEngine;
using System.Collections;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Player.Abilities
{
    public class MysonAbilities : MonoBehaviour
    {

        #region Private Properties

        [Header("Particles")]
        [SerializeField]
        private GameObject _particleDefinitive;
        [Space(10)]

        [Header("Definitive GameObjects")]
        [SerializeField]
        private Renderer _body;
        [SerializeField]
        private Renderer _head;

        [Header("Parameters Abilities")]
        [SerializeField]
        private float _timeGhost;

        private PlayerInfo _playerInfo;

        #endregion

        #region Private Methods

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
        }

        #endregion

        #region Animation Events

        public virtual void OnParticlesDefinitive(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleDefinitive, _playerInfo.ParticleBodyPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 3f);
        }

        public virtual void OnHideHead(AnimationEvent animationEvent)
        {
            _head.enabled = false;
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
            _head.enabled = true;
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
