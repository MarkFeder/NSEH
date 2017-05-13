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

        private Vector3 _forceDirection;
        private PlayerInfo _senderInfo;
        private float _damage;

        [SerializeField]
        private float _force;
        [SerializeField]
        private float _percent;
        [SerializeField]
        private float _seconds;

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

        public Collider Wave
        {
            get { return _collider; }
        }

        public Vector3 ForceDirection
        {
            get { return _forceDirection; }
            set { _forceDirection = value; }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _enemies = new List<GameObject>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            string colTag = collision.collider.tag;

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
                        enemyInfo.Body.AddForceAtPosition(_forceDirection * _force, collision.contacts[0].point,  ForceMode.Impulse);
                        enemyInfo.PlayerMovement.DecreaseSpeedForSeconds(_percent, _seconds);

                        // Add this enemy to the list so as to cause damage again
                        _enemies.Add(enemyObj);
                    }
                }
            }
        }

        #endregion
    }
}
