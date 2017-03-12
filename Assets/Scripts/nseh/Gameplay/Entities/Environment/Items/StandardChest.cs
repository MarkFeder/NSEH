using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Movement;
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

        protected override void Start()
        {
            base.Start();

            this.CheckTimes();
        }

        protected override void Activate()
        {
            switch (this.chestType)
            {
                case StandardChestType.None:

                    Debug.Log("None effect");

                    break;

                case StandardChestType.Health:

                    this.IncreaseHealth(this.percent);
                    StartCoroutine(this.DisplayText(this.itemText, StandardItems.HEALTH));
                    this.ParticleAnimation(particlePrefab, 1.5f, particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Damage:

                    this.IncreaseDamage(this.percent, this.time);
                    StartCoroutine(this.DisplayText(this.itemText, StandardItems.DAMAGE));
                    this.ParticleAnimation(particlePrefab, this.time, particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Velocity:

                    this.IncreaseVelocity(this.percent, this.time);
                    StartCoroutine(this.DisplayText(this.itemText, StandardItems.SPEED));
                    this.ParticleAnimation(particlePrefab, this.time, particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Jump:

                    this.IncreaseJump(this.percent, this.time);
                    StartCoroutine(this.DisplayText(this.itemText, StandardItems.JUMP));
                    this.ParticleAnimation(particlePrefab, this.time, particlesSpawnPoints.ParticleBodyPos);

                    break;

                case StandardChestType.Defense:

                    this.SetUpDefense(this.time, this.hits);
                    StartCoroutine(this.DisplayText(this.itemText, StandardItems.DEFENSE));
                    this.ParticleAnimation(particlePrefab, this.time, particlesSpawnPoints.ParticleBodyPos);

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
        
        private void CheckTimes()
        {
            if (this.chestType == StandardChestType.Damage && this.time >= this.destructionTime)
            {
                Debug.LogError("Error, time must be less than destruction time");
            }
        }

        private IEnumerator SetUpDefense(float time, int hits)
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
            this.target.GetComponent<CharacterHealth>().IncreaseHealth(percent);
        }

        private void IncreaseDamage(float percent)
        {
            this.target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().ForEach(act => act.IncreaseDamage(percent));
        }

        private void IncreaseDamage(float percent, float time)
        {
            StartCoroutine(this.IncreaseDamageInternal(percent, time));
        }

        private IEnumerator IncreaseDamageInternal(float percent, float time)
        {
            // Save old values

            Dictionary<AttackType, float> oldDamages = new Dictionary<AttackType, float>();
            var actions = this.target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>();

            actions.ForEach(act =>
            {
                oldDamages.Add(act.AttackType, act.CurrentDamage);
                act.IncreaseDamage(percent);
            });

            yield return new WaitForSeconds(time);

            // Restore old values

            actions.ForEach(act =>
            {
                act.CurrentDamage = oldDamages[act.AttackType];
                Debug.Log(string.Format("[{0}] damage of {1} has been restored to: {2}", act.AttackType.ToString(), act.Animator.name, oldDamages[act.AttackType]));
            });
        }

        #endregion
    }
}
