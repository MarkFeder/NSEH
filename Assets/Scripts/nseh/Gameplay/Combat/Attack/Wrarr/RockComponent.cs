using nseh.Gameplay.Entities.Player;
using System.Collections.Generic;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Attack.Wrarr
{
    public class RockComponent : MonoBehaviour
    {
        #region Private Properties

        private List<GameObject> _enemies;
        private int _player;
        private float _damage;

        [SerializeField]
        private float _destructionTime;

        #endregion

        #region Public Properties

        public float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public int Player
        {
            get { return _player; }
            set { _player = value; }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            _enemies = new List<GameObject>();

            Destroy(transform.parent.gameObject, _destructionTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(Tags.PLAYER_BODY))
            {
                GameObject enemyObj = collision.gameObject;

                if (!_enemies.Contains(enemyObj))
                {
                    PlayerInfo enemyInfo = enemyObj.GetComponent<PlayerInfo>();
                    if (enemyInfo != null && enemyInfo.Player != _player)
                    {
                        enemyInfo.PlayerHealth.TakeDamage((int)_damage);
                        _enemies.Add(enemyObj);
                    }
                }
            }
        }

        #endregion
    }
}
