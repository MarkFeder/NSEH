using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;
using SpecialItems = nseh.Utils.Constants.Items.SpecialItems;

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

        public float seconds;
        public int percent; 

        #endregion

        protected override void Activate()
        {
            switch (chestType)
            {
                case SpecialChestType.None:

                    Debug.Log("None effect");

                    break;

                case SpecialChestType.Invulnerability:

                    Invulnerability(seconds);
                    _spawnItemPoint.DisplayText(_itemText, SpecialItems.INVULNERABILITY, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, seconds, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case SpecialChestType.CriticalDamage:

                    CriticalDamage(percent, seconds);
                    _spawnItemPoint.DisplayText(_itemText, SpecialItems.CRITICAL, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, 3.0f, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case SpecialChestType.UnlockDefinitiveMode:

                    UnlockDefinitiveMode();
                    _spawnItemPoint.DisplayText(_itemText, SpecialItems.ULTIMATE, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, 1.0f, _particlesSpawnPoints.ParticleBodyPos);

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

        #region Private Methods

        private void Invulnerability(float time)
        {
            _target.GetComponent<PlayerHealth>().InvulnerabilityModeForSeconds(time);
        }

        private void CriticalDamage(float percent, float seconds)
        {
            _target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().ForEach(act => act.IncreaseDamageForSeconds(percent, seconds));
        }

        private void UnlockDefinitiveMode()
        {
            CharacterAttack definitiveAttack = _target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().Where(act => act.AttackType == AttackType.CharacterDefinitive).FirstOrDefault();

            if (definitiveAttack != null)
            {
                definitiveAttack.IsEnabled = true;
            }
        }

        #endregion
    }

}
