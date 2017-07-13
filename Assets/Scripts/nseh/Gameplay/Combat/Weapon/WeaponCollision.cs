using System;
using System.Collections.Generic;
using nseh.Gameplay.Entities.Player;
using UnityEngine;
using nseh.Gameplay.Entities.Enemies;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Weapon
{

    [Serializable]
    [RequireComponent(typeof(Collider))]

    public class WeaponCollision : MonoBehaviour
    {

        #region Private Properties

        private PlayerCombat _playerCombat;
        private PlayerInfo _playerInfo;
        private List<GameObject> _enemyTargets;
        [SerializeField]
        private GameObject _trailParticle;

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

            _enemyTargets.Add(this.gameObject.transform.root.gameObject);
        }

        private void OnDisable()
        {
            _enemyTargets.Clear();
            _enemyTargets.Add(this.gameObject.transform.root.gameObject);
        }

        #endregion

        #region Trigger Methods

        private void OnCollisionEnter(Collision collider)
        {
            GameObject enemy = collider.transform.root.gameObject; 
            if (enemy.tag == Tags.PLAYER_BODY && !_enemyTargets.Contains(enemy))
            {
                ContactPoint position = collider.contacts[0];
                
                PlayerInfo _auxPlayerInfo = enemy.GetComponent<PlayerInfo>();
                _enemyTargets.Add(enemy);
                _auxPlayerInfo.TakeDamage((float)(((int)_playerCombat._currentAttack + ((int)(_playerCombat._currentAttack) * 0.05 * _playerInfo.CurrentStrength))* _playerCombat.CriticalIncrement), _playerInfo, position.point);
                _playerCombat.CriticManagement();
            }

            else if (collider.transform.root.tag == Tags.ENEMY)
            {
                ContactPoint position = collider.contacts[0];
                EnemyHealth _auxEnemyHealth = enemy.GetComponent<EnemyHealth>();
                _auxEnemyHealth.TakeDamage((float)(((int)_playerCombat._currentAttack + ((int)(_playerCombat._currentAttack) * 0.05 * _playerInfo.CurrentStrength)) * _playerCombat.CriticalIncrement), _playerInfo, position.point);
                _playerCombat.CriticManagement();

            }
        }

        #endregion

    }
}
