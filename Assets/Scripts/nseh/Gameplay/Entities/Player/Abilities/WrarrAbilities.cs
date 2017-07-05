﻿using nseh.Gameplay.Entities.Player;
using UnityEngine;
using nseh.Gameplay.Combat.Special.Wrarr;

namespace nseh.Gameplay.Player.Abilities
{
    public class WrarrAbilities : MonoBehaviour
    {

        #region Private Properties

        [Header("Particles")]
        [SerializeField]
        private GameObject _particleAbility;
        [SerializeField]
        private GameObject _particleDefinitive;
        [Space(10)]

        [Header("GameObjects")]
        [SerializeField]
        private GameObject _wave;
        [SerializeField]
        private GameObject _rock;
        [Space(10)]

        [Header("Definitive Parameters")]
        [SerializeField]
        private GameObject _boneRock;
        [SerializeField]
        private float _rockForce;

        private PlayerInfo _playerInfo;
        private Rigidbody _body;
        private GameObject _rockAux;
        private Rigidbody _rockRigidBody;
        private Collider _rockCollider;

        #endregion

        #region Private Methods

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _body = GetComponent<Rigidbody>();
        }

        #endregion

        #region Animation Events

        public virtual void OnPlayAttackAbilityParticle(AnimationEvent animationEvent)
        {
            GameObject particleGameObject = Instantiate(_particleAbility, _playerInfo.ParticleFootPos.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }
            Destroy(particleGameObject, 3f);
        }

        public virtual void OnActivateWave(AnimationEvent animationEvent)
        {
            _body.isKinematic = true;
            _wave.GetComponent<WaveComponent>().enabled = true;
        }

        public virtual void OnDeactivateWave(AnimationEvent animationEvent)
        {
            _body.isKinematic = false;
            _wave.GetComponent<WaveComponent>().enabled = false;
        }

        public virtual void OnInstanceRock(AnimationEvent animationEvent)
        {
            _rockAux = Instantiate(_rock, _boneRock.transform.position, Quaternion.identity, _boneRock.transform);
            _rockRigidBody = _rockAux.transform.GetChild(0).GetComponent<Rigidbody>();
            _rockCollider = _rockAux.transform.GetChild(0).GetComponent<Collider>();
            _rockCollider.enabled = false;
        }

        public virtual void OnLaunchRock(AnimationEvent animationEvent)
        {
            _rockAux.transform.parent = null;
            _rockRigidBody.isKinematic = false;
            _rockRigidBody.useGravity = true;
            _rockCollider.enabled = true;

            Vector3 vForward = transform.TransformDirection(Vector3.forward);
            _rockRigidBody.AddForce(vForward * _rockForce, ForceMode.Force);
        }

        #endregion

    }
}
