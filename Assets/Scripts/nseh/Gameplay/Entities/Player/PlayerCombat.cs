using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Main;
using Damage = nseh.Utils.Constants.PlayerInfo;
using nseh.Gameplay.Combat.Weapon;

namespace nseh.Gameplay.Entities.Player
{
    public class PlayerCombat : MonoBehaviour
    {

        #region Private Properties
        private PlayerInfo _playerInfo;

        [Header("Weapons")]
        [SerializeField]
        private List<Collider> _weaponList;
        [Space(10)]

        private GameObject _particle;
        private GameObject _particleCritical;

        [Header("Clips")]
        [SerializeField]
        private List<AudioClip> _audioAttack;

        #endregion

        #region Public Properties

        public enum Attack
        {
            //DICCIONARIO!
            None = 0,
            A1 = Damage.A1,
            A2 = Damage.A2,
            A3 = Damage.A3,
            B1 = Damage.B1,
            B2 = Damage.B2
        }
        /*
        [Serializable]
        public struct NameDamageParticleSoundPosition
        {
            public Attack attack;
            public int damage;
            public GameObject particle;
            public AudioClip sound;
            public Transform position;
        }

        public List<NameDamageParticleSoundPosition> Attacks;*/

        [Header("Attack State")]
        public Attack _currentAttack;
        public int criticalAttacks;
        private int _criticalIncrement;

        public int CriticalIncrement
        {
            get
            {
                return _criticalIncrement;
            }

        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            _currentAttack = Attack.None;
        }

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _criticalIncrement = 1;
            criticalAttacks = 0;
        }

        public void CriticManagement()
        {
            if(criticalAttacks > 0)
            {
                criticalAttacks--;

                if (criticalAttacks == 0)
                {
                    _criticalIncrement = 1;
                    Destroy(_particleCritical);
                }
            }
        }

        public void AddCriticalAttacks(int attacks, GameObject particle, Transform particlesPos)
        {
            if(criticalAttacks == 0)
            {
                criticalAttacks = attacks;
                _criticalIncrement = 2;
                _particleCritical = Instantiate(particle, particlesPos.position, particlesPos.rotation, particlesPos.transform);

                foreach (ParticleSystem particle_aux in _particleCritical.GetComponentsInChildren<ParticleSystem>())
                {
                    particle_aux.Play();
                }
            }

            else if(criticalAttacks < attacks)
            {
                criticalAttacks = attacks;
            }
        }

        #endregion

        #region Animation Events
        
        public void ActivateCollider(int index)
        {
            _weaponList[index].enabled = true;
            _weaponList[index].GetComponent<WeaponCollision>().enabled = true;
        }

        public void DeactivateCollider(int index)
        {
            _weaponList[index].enabled = false;
            _weaponList[index].GetComponent<WeaponCollision>().enabled = false;
        }

        public void DesactivateAllColliders()
        {
            foreach(Collider weapon in _weaponList)
            {
                weapon.enabled = false;
                weapon.GetComponent<WeaponCollision>().enabled = false;
            }
        }

        public virtual void OnPlayAttackSound(int index)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(_audioAttack[index], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        #endregion

    }
}
