using System;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Player;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Weapon
{

    [Serializable]
    [RequireComponent(typeof(Collider))]

    public class WeaponCollisionEnemy : MonoBehaviour
    {

        #region Private Properties

        private PlayerCombat _playerCombat;
        private PlayerInfo _playerInfo;

        private List<GameObject> _enemyTargets;

        [SerializeField]
        private int _damage;

        #endregion

        #region Private Methods

        private void Start()
        {
            Rigidbody _rigidBody = GetComponent<Rigidbody>();
            Collider _collider = GetComponent<Collider>();
            _enemyTargets = new List<GameObject>();
            _playerInfo = this.gameObject.transform.root.GetComponent<PlayerInfo>();
            _playerCombat = this.gameObject.transform.root.GetComponent<PlayerCombat>();

            _rigidBody.isKinematic = false;
            _rigidBody.angularDrag = 0;
            _rigidBody.mass = 0;
            _collider.enabled = false;
            _collider.isTrigger = false;
        }

        private void OnDisable()
        {
            _enemyTargets.Clear();
        }

        #endregion

        #region Trigger Methods

        private void OnCollisionEnter(Collision collider)
        {
            GameObject enemy = collider.transform.root.gameObject;
            if (enemy.tag == Tags.PLAYER && !_enemyTargets.Contains(enemy))
            {
                _enemyTargets.Add(enemy);
                PlayerInfo enemyInfo = enemy.GetComponent<PlayerInfo>();
                enemyInfo.TakeDamage(_damage);
            }
        }

        public void ResetList()
        {
            _enemyTargets.Clear();
        }
        #endregion

    }
}
