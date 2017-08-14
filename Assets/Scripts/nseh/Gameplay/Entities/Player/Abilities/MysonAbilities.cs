using UnityEngine;
using System.Collections;
using nseh.Gameplay.Entities.Player;

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
