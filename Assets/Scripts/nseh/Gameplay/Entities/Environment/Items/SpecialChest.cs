﻿using nseh.Gameplay.Base.Abstract.Entities;
using UnityEngine;
using nseh.Managers.Main;

namespace nseh.Gameplay.Entities.Environment.Items
{
    public enum SpecialChestType
    {
        None = 0,
        Invulnerability = 1,
        CriticalDamage = 2,
        UnlockDefinitiveMode = 3
    }

    public class SpecialChest : Chest
    {

        #region Public Properties

        public SpecialChestType chestType;

        [SerializeField]
        private float _percent;
        [SerializeField]
        private float _seconds;
        [SerializeField]
        private int _points;

        #endregion

        #region Protected Methods

        protected override void Activate()
        {
            switch (chestType)
            {
                case SpecialChestType.None:

                    Debug.Log("None effect");

                    break;

                case SpecialChestType.Invulnerability:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(chestType.ToString(), _seconds));
                    GameManager.Instance.StartCoroutine(_playerInfo.InvulnerabilityModeForSeconds(_seconds));
                    ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos);

                    break;

                case SpecialChestType.CriticalDamage:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(chestType.ToString(), _seconds));

                    GameManager.Instance.StartCoroutine(_playerInfo.BonificationAttackForSeconds(_points, _seconds));
                    ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos);

                    break;

                case SpecialChestType.UnlockDefinitiveMode:

                    _playerInfo.IncreaseEnergy(100);
                    ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos);

                    break;

                default:

                    Debug.Log("No SpecialChestType has been detected");

                    break;
            }

            Deactivate();
        }

        protected override void Deactivate()
        {
            Destroy(gameObject, _destructionTime);
        }

        #endregion

    }
}
