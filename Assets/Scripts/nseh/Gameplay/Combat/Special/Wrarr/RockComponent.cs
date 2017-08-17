﻿using nseh.Gameplay.Entities.Player;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Enemies;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Special.Wrarr
{
    public class RockComponent : MonoBehaviour
    {

        #region Private Properties

        private Rigidbody _body;
        private Collider _collider;
        private List<GameObject> _enemies;

        private PlayerInfo _senderInfo;
        private PlayerCombat _playerCombat;
        [SerializeField]
        private float _damage;

        [SerializeField]
        private GameObject _particle;

        public bool canDestroy;
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

            _body.isKinematic = false;
            _body.angularDrag = 0;
            _body.mass = 1;
            _collider.enabled = false;
            _collider.isTrigger = false;
            canDestroy = false;
        }

        private void OnCollisionEnter(Collision collider)
        {
            GameObject hit = collider.transform.root.gameObject;
            ContactPoint position = collider.contacts[0];

            if (hit.tag == Tags.PLAYER_BODY && !_enemies.Contains(hit))
            {
                PlayerInfo _auxPlayerInfo = hit.GetComponent<PlayerInfo>();
                _enemies.Add(hit);
                FireParticles(position.point);
                _auxPlayerInfo.TakeDamage(_damage, _senderInfo);
            }

            else if (hit.tag == Tags.ENEMY && !_enemies.Contains(hit))
            {
                EnemyHealth _auxEnemyHealth = hit.GetComponent<EnemyHealth>();
                FireParticles(position.point);
                _auxEnemyHealth.TakeDamage(_damage, _senderInfo);
                Destroy(transform.parent.gameObject);
            }

            else if (hit.tag == Tags.ONE_WAY_PLATFORM && canDestroy)
            {
                FireParticles(position.point);
                Destroy(transform.parent.gameObject);
            }
        }

        private void FireParticles(Vector3 position)
        {
            GameObject particleGameObject = Instantiate(_particle, position, transform.rotation);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 3f);
        }

        #endregion

    }
}