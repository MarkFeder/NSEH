using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;
using StandardItems = nseh.Utils.Constants.Items.StandardItems;

namespace nseh.Gameplay.Entities.Environment.Items
{
    public enum StandardChestType
    {
        None = 0,
        Health = 1,
        Damage = 2,
        Velocity = 3,
        Jump = 4,
        Defense = 5
    }

    public class StandardChest : Chest
    {
        #region Public Properties

        public StandardChestType chestType;

        public float percent;
        public float time;

        #endregion

        protected override void Activate()
        {
            switch (chestType)
            {
                case StandardChestType.None:

                    Debug.Log("None effect");

                    break;

                case StandardChestType.Health:

                    IncreaseHealth(percent);
                    _spawnItemPoint.DisplayText(_itemText, StandardItems.HEALTH, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, 1.5f, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Damage:

                    IncreaseDamage(percent, time);
                    _spawnItemPoint.DisplayText(_itemText, StandardItems.DAMAGE, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, time, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Velocity:

                    IncreaseVelocity(percent, time);
                    _spawnItemPoint.DisplayText(_itemText, StandardItems.SPEED, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, time, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Jump:

                    IncreaseJump(percent, time);
                    _spawnItemPoint.DisplayText(_itemText, StandardItems.JUMP, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, time, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Defense:

                    SetupDefense(percent, time);
                    _spawnItemPoint.DisplayText(_itemText, StandardItems.DEFENSE, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, time, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                default:

                    Debug.Log("No StandardChestType is detected");

                    break;
            }

            Deactivate();
        }

        protected override void Deactivate()
        {
            Destroy(gameObject, _destructionTime);
        }

        #region Private Methods

        private void SetupDefense(float percent, float time)
        {
            _target.GetComponent<PlayerHealth>().BonificationDefenseForSeconds(percent, time);
        }

        private void IncreaseJump(float percent, float time)
        {
            _target.GetComponent<PlayerMovement>().IncreaseJumpForSeconds(percent, time);
        }

        private void IncreaseVelocity(float percent, float time)
        {
            _target.GetComponent<PlayerMovement>().IncreaseSpeedForSeconds(percent, time);
        }

        private void IncreaseHealth(float percent)
        {
            _target.GetComponent<PlayerHealth>().IncreaseHealth(percent);
        }

        private void IncreaseDamage(float percent)
        {
            _target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().ForEach(act => act.IncreaseDamage(percent));
        }

        private void IncreaseDamage(float percent, float time)
        {
            _target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().ForEach(act => act.IncreaseDamageForSeconds(percent, time));
        }

        #endregion
    }
}
