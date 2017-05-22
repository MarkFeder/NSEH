using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Entities.Player;
using System.Collections.Generic;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Attack.Wrarr
{
    public class RockComponent : MonoBehaviour
    {
        #region Private Properties

        private Rigidbody _body;
        private Collider _collider;
        private List<GameObject> _enemies;

        private PlayerInfo _senderInfo;
        private float _damage;

        [SerializeField]
        private float _destructionTime;

        [SerializeField]
        private GameObject _particle;

        #endregion

        #region Public Properties

        public float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public PlayerInfo Sender
        {
            get { return _senderInfo; }
            set { _senderInfo = value; }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _enemies = new List<GameObject>();

            Destroy(transform.parent.gameObject, _destructionTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            string colTag = collision.collider.tag;
            GameObject enemyObj = collision.gameObject;

            GameObject particleGameObject = Instantiate(_particle, transform.position, transform.rotation);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 3f);

            if (colTag == Tags.PLAYER_BODY)
            {
                if (!_enemies.Contains(enemyObj))
                {
                    PlayerInfo enemyInfo = enemyObj.GetComponent<PlayerInfo>();
                    if (enemyInfo != null && enemyInfo.Player != _senderInfo.Player)
                    {
                        // Set score and energy on sender
                        _senderInfo.PlayerScore.IncreaseScore((int)_damage);
                        _senderInfo.PlayerEnergy.IncreaseEnergy(_damage / 2);

                        // Set health
                        enemyInfo.PlayerHealth.TakeDamage((int)_damage);

                        // Add this enemy to the list so as to cause damage again
                        _enemies.Add(enemyObj);
                    }
                }
            }
            else if (colTag == Tags.ENEMY)
            {
                if (!_enemies.Contains(enemyObj))
                {
                    IHealth enemyHealth = enemyObj.GetComponent<IHealth>();
                    if (enemyHealth != null)
                    {
                        // Set health
                        enemyHealth.TakeDamage((int)_damage);

                        // Add this enemy to the list so as not to cause damage again
                        _enemies.Add(enemyObj);
                    }
                }
            }
            else if (colTag == Tags.PLATFORM)
            {
                _body.isKinematic = true;
                Destroy(transform.parent.gameObject, _destructionTime);
            }
        }

        #endregion
    }
}