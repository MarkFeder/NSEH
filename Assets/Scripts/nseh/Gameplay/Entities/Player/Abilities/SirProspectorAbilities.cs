using UnityEngine;
using System.Collections;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Player.Abilities
{
    public class SirProspectorAbilities : MonoBehaviour
    {

        #region Private Properties

        [Header ("Particles")]
        [SerializeField]
        private GameObject _particleAbility;
        [SerializeField]
        private GameObject _particleDefense;
        [SerializeField]
        private GameObject _particleDefinitive;
        [Space(10)]

        [Header("Ability Parameters")]
        [SerializeField]
        private int _defenseBonification;
        [SerializeField]
        private int _timeDefenseBonification;
        [Space(10)]

        [Header("Definitive Parameters")]
        [SerializeField]
        private int _attackBonification;
        [SerializeField]
        private int _timeAttackBonification;
        [Space(10)]

        [Header("Definitive GameObjects")]
        [SerializeField]
        private Renderer _sword;
        [SerializeField]
        private Renderer _shovel;
        [SerializeField]
        private GameObject _swordAura;

        private PlayerInfo _playerInfo;

        #endregion

        #region Private Methods

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
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

        public virtual void OnIncreaseDefense(AnimationEvent animationEvent)
        {
            GameObject particleGameObjectDefense = Instantiate(_particleDefense, _playerInfo.ParticleBodyPos.transform.position, _playerInfo.ParticleBodyPos.transform.rotation, _playerInfo.ParticleBodyPos.transform);
            foreach (ParticleSystem particle_aux in particleGameObjectDefense.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObjectDefense, _timeDefenseBonification);

            StartCoroutine(_playerInfo.BonificationBaseDefenseForSeconds(_defenseBonification, _timeDefenseBonification));
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

        public virtual void OnIncreaseAttack(AnimationEvent animationEvent)
        {
            StartCoroutine(_playerInfo.BonificationBaseAttackForSeconds(_attackBonification, _timeAttackBonification));
            StartCoroutine(HideSword(_timeAttackBonification));
        }

        public virtual void OnHideShovel(AnimationEvent animationEvent)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
            _playerInfo.PlayerMovement.IsFacingRight = (transform.localEulerAngles.y == 270.0f) ? true : false;
            _shovel.enabled = false;
        }

        public virtual void OnShowSword(AnimationEvent animationEvent)
        {
            _sword.enabled = true;
            _swordAura.SetActive(true);
        }

        public IEnumerator HideSword(int seconds)
        {
            yield return new WaitForSeconds(seconds);

            _shovel.enabled = true;
            _sword.enabled = false;
            _swordAura.SetActive(false);
        }

        #endregion

    }
}
