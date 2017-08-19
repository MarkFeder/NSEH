﻿using nseh.Gameplay.Entities.Player;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Enemies;
using Tags = nseh.Utils.Constants.Tags;


namespace nseh.Gameplay.Combat.Special.Myson
{
    public class ExplosionComponent : MonoBehaviour
    {
        #region Private Properties

        private Collider _collider;
        private Rigidbody _body;
        private List<GameObject> _enemies;

        private PlayerInfo _senderInfo;
        private PlayerCombat _playerCombat;
        [SerializeField]
        private float _damage;

        #endregion

        #region Private Methods

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _enemies = new List<GameObject>();
            _enemies.Add(this.gameObject.transform.root.gameObject);
            _senderInfo = this.gameObject.transform.root.GetComponent<PlayerInfo>();
            _playerCombat = this.gameObject.transform.root.GetComponent<PlayerCombat>();
            this.gameObject.transform.parent = null;

            _body.isKinematic = false;
            _body.angularDrag = 0;
            _body.mass = 0;
            _collider.isTrigger = false;
        }

        private void OnCollisionEnter(Collision collider)
        {
            GameObject hit = collider.transform.root.gameObject;

            if (hit.tag == Tags.PLAYER_BODY && !_enemies.Contains(hit))
            {
                PlayerInfo _auxPlayerInfo = hit.GetComponent<PlayerInfo>();
                _enemies.Add(hit);
                _auxPlayerInfo.TakeDamage(_damage, _senderInfo);
            }

            else if (hit.tag == Tags.ENEMY && !_enemies.Contains(hit))
            {
                EnemyHealth _auxEnemyHealth = hit.GetComponent<EnemyHealth>();
                _enemies.Add(hit);
                _auxEnemyHealth.TakeDamage(_damage*2, _senderInfo);
            }
        }

        #endregion

    }
}