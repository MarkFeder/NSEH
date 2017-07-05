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

        #endregion

        #region Private Methods

        private void Awake()
        {
            _currentAttack = Attack.None;
        }

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();

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
