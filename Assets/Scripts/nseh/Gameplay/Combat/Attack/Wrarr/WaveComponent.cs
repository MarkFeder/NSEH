using nseh.Gameplay.Entities.Player;
using System.Collections.Generic;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Attack.Wrarr
{
    public class WaveComponent : MonoBehaviour
    {
        #region Private Properties

        private Collider _collider;
        private List<GameObject> _enemies;
        private PlayerInfo _senderInfo;

        private float _damage;
        private float _force;

        #endregion

        #region Public Properties

        public float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public float Force
        {
            get { return _force; }
            set { _force = value; }
        }

        public PlayerInfo Sender
        {
            get { return _senderInfo; }
            set { _senderInfo = value; }
        }

        public Collider WaveCollider
        {
            get { return _collider; }
        }

        #endregion

        #region Private Methods

        private void OnEnable()
        {
			_collider = GetComponent<Collider>();
			_collider.enabled = false;

			_enemies = new List<GameObject>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            string colTag = collision.tag;

            if (colTag == Tags.PLAYER_BODY)
            {
                GameObject enemyObj = collision.gameObject;

                if (!_enemies.Contains(enemyObj))
                {
                    PlayerInfo enemyInfo = enemyObj.GetComponent<PlayerInfo>();
                    if (enemyInfo != null && enemyInfo.Player != _senderInfo.Player)
                    {
                        // Set score and energy on sender
                        _senderInfo.Score += (int)_damage;
                        _senderInfo.PlayerEnergy.IncreaseEnergy(_damage / 2);

                        // Set health
                        enemyInfo.PlayerHealth.TakeDamage((int)_damage);

                        // Push enemy body
                        enemyInfo.Body.AddForceAtPosition(_force * transform.forward, enemyInfo.ParticleBodyPos.position, ForceMode.Acceleration);

                        // Add this enemy to the list so as to cause damage again
                        _enemies.Add(enemyObj);
                    }
                }
            }
        }

        #endregion
    }
}
