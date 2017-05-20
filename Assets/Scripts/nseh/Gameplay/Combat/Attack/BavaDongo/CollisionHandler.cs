using nseh.Gameplay.Entities.Player;
using System.Collections.Generic;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Attack.BavaDongo
{
    public class CollisionHandler : MonoBehaviour
    {
        #region Private Properties

        [SerializeField]
        private int _damage;
        [SerializeField]
        private int _index;

        private List<GameObject> _enemies;

        #endregion

        #region Public Properties

        public int Damage { get { return _damage; } set { _damage = value; } }

        public int Index { get { return _index; } }

        #endregion

        #region Private Methods

        private void Start()
        {
            _enemies = new List<GameObject>();
        }

        #endregion

        #region Public Methods

        public void EnableHandler()
        {
            gameObject.SetActive(true);
        }

        public void DisableHandler()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Trigger Methods

        private void OnTriggerEnter(Collider other)
        {
            GameObject enemyObj = other.gameObject;
            string colTag = other.tag;

            if (colTag == Tags.PLAYER)
            {
                PlayerInfo enemyInfo = enemyObj.GetComponent<PlayerInfo>();
                if (enemyInfo != null)
                {
                    enemyInfo.PlayerHealth.TakeDamage(_damage);
                }
            }
        }

        #endregion
    }
}
