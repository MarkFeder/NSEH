using System.Collections.Generic;
using nseh.Gameplay.Entities.Player;
using UnityEngine;
using System.Collections;
using nseh.Utils;
using System.Linq;

namespace nseh.Gameplay.Combat.Special.Granhilda
{
    public class StaticShieldComponent : MonoBehaviour
    {

        #region Private Properties 

        private List<GameObject> _playersInShield;
        private float _nextApplyEffect = 0;
        [SerializeField]
        private int _damageAbility;
        [SerializeField]
        private float _timeDamage;
        [SerializeField]
        private float _timeEffect;
        [SerializeField]
        private GameObject _particle;
        private GameObject particleGameObject;
        private PlayerInfo _playerInfo;

        #endregion

        #region Private Methods

        // Use this for initialization
        void Awake()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _playersInShield = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_playersInShield.Any())
            {
                DealDamagePeriodically();
            }
        }

        private void OnEnable()
        {
            //_playersInShield.Add(_playerInfo.gameObject);
            particleGameObject = Instantiate(_particle, _playerInfo.ParticleFootPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            StartCoroutine(Effect());
        }

        private void OnDisable()
        {
            Destroy(particleGameObject);
            _playersInShield = new List<GameObject>();
        }

        private IEnumerator Effect()
        {
            yield return new WaitForSeconds(_timeEffect);
            this.enabled = false;

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Constants.Tags.PLAYER_BODY) && !PlayerListContains(other.gameObject))
            {
                _playersInShield.Add(other.gameObject);
            }
        }

        private bool PlayerListContains(GameObject playerToRegister)
        {
            PlayerInfo player = playerToRegister.GetComponent<PlayerInfo>();

            if (_playersInShield.Any())
            {
                foreach (PlayerInfo element in _playersInShield.Select(t => t.GetComponent<PlayerInfo>()))
                {
                    if (element.GamepadIndex == player.GamepadIndex)
                    { return true; }
                }
            }

            return false;
        }

        private void DealDamagePeriodically()
        {
            if (Time.time >= _nextApplyEffect)
            {
                _nextApplyEffect = Time.time + _timeDamage;
                foreach (PlayerInfo element in _playersInShield.Select(t => t.GetComponent<PlayerInfo>()))
                {
                    element.DecreaseHealth(_damageAbility);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _playersInShield.Remove(other.gameObject);
        }

        #endregion

    }
}
