using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System.Collections;
using System.Collections.Generic;
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

        [Range(0,10)]
        public int hits;

        #endregion

        protected override void Activate()
        {
            switch (this.chestType)
            {
                case StandardChestType.None:

                    Debug.Log("None effect");

                    break;

                case StandardChestType.Health:

                    this.IncreaseHealth(this.percent);
                    this.spawnItemPoint.DisplayText(this.itemText, StandardItems.HEALTH, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, 1.5f, this.particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Damage:

                    this.IncreaseDamage(this.percent, this.time);
                    this.spawnItemPoint.DisplayText(this.itemText, StandardItems.DAMAGE, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, this.time, this.particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Velocity:

                    this.IncreaseVelocity(this.percent, this.time);
                    this.spawnItemPoint.DisplayText(this.itemText, StandardItems.SPEED, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, this.time, this.particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Jump:

                    this.IncreaseJump(this.percent, this.time);
                    this.spawnItemPoint.DisplayText(this.itemText, StandardItems.JUMP, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, this.time, this.particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Defense:

                    this.SetupDefense(this.time, this.hits);
                    this.spawnItemPoint.DisplayText(this.itemText, StandardItems.DEFENSE, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, this.time, this.particlesSpawnPoints.ParticleBodyPos);

                    break;

                default:

                    Debug.Log("No StandardChestType is detected");

                    break;
            }

            this.Deactivate();
        }

        protected override void Deactivate()
        {
            Destroy(this.gameObject, this.destructionTime);
        }

        #region Private Methods

        private IEnumerator SetupDefense(float time, int hits)
        {
            // TODO: think how to set this thing up with our combat system

            yield return new WaitForSeconds(time);
        }

        private void IncreaseJump(float percent, float time)
        {
            this.target.GetComponent<PlayerMovement>().IncreaseJumpForSeconds(percent, time);
        }

        private void IncreaseVelocity(float percent, float time)
        {
            this.target.GetComponent<PlayerMovement>().IncreaseSpeedForSeconds(percent, time);
        }

        private void IncreaseHealth(float percent)
        {
            this.target.GetComponent<PlayerHealth>().IncreaseHealth(percent);
        }

        private void IncreaseDamage(float percent)
        {
            this.target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().ForEach(act => act.IncreaseDamage(percent));
        }

        private void IncreaseDamage(float percent, float time)
        {
            this.target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().ForEach(act => act.IncreaseDamageForSeconds(percent, time));
        }

        #endregion
    }
}
