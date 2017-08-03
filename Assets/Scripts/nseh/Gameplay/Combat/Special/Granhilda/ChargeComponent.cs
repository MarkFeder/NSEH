using nseh.Gameplay.Entities.Player;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Enemies;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Special.Granhilda
{
    public class ChargeComponent : MonoBehaviour
    
    {

        #region Private Properties

        private Rigidbody _body;
        private Collider _collider;
        private List<GameObject> _enemies;

        private PlayerInfo _senderInfo;
        private PlayerCombat _playerCombat;
        [SerializeField]
        private float _damage;

        #endregion

        #region Private Methods

        private void Start()
        {
            Rigidbody _rigidBody = GetComponent<Rigidbody>();
            Collider _collider = GetComponent<Collider>();
            _enemies = new List<GameObject>();
            _enemies.Add(this.gameObject.transform.root.gameObject);
            _senderInfo = this.gameObject.transform.root.GetComponent<PlayerInfo>();
            _playerCombat = this.gameObject.transform.root.GetComponent<PlayerCombat>();

            _rigidBody.isKinematic = false;
            _rigidBody.angularDrag = 0;
            _collider.enabled = false;
            _collider.isTrigger = false;
        }

        private void OnCollisionEnter(Collision collider)
        {
            GameObject hit = collider.transform.root.gameObject;
            ContactPoint position = collider.contacts[0];

            if (hit.tag == Tags.PLAYER_BODY && !_enemies.Contains(hit))
            {
                Physics.IgnoreCollision(_collider, collider.collider);
                PlayerInfo _auxPlayerInfo = hit.GetComponent<PlayerInfo>();
                _enemies.Add(hit);
                _auxPlayerInfo.TakeDamage(_damage, _senderInfo);
            }

            else if (hit.tag == Tags.ENEMY && !_enemies.Contains(hit))
            {
                Physics.IgnoreCollision(_collider, collider.collider);
                EnemyHealth _auxEnemyHealth = hit.GetComponent<EnemyHealth>();
                _auxEnemyHealth.TakeDamage((float)((int)_playerCombat._currentAttack + ((int)(_playerCombat._currentAttack) * 0.05 * _senderInfo.CurrentStrength)), _senderInfo, position.point);
            }
        }

        #endregion

    }
}
