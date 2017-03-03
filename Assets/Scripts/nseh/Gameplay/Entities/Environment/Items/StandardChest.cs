using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Movement;
using nseh.Utils.Helpers;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

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
        public StandardChestType chestType;

        [Range(0,1)]
        public float percent;
        public float time;

        [Range(0,10)]
        public int hits;

        protected override void Start()
        {
            base.Start();
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

                    break;

                case StandardChestType.Damage:

                    this.IncreaseDamage(this.percent);

                    break;

                case StandardChestType.Velocity:

                    this.IncreaseVelocity(this.percent, this.time);

                    break;

                case StandardChestType.Jump:

                    this.IncreaseJump(this.percent, this.time);

                    break;

                case StandardChestType.Defense:

                    this.SetUpDefense(this.time, this.hits);

                    break;

                default:

                    Debug.Log("No StandardChestType is detected");

                    break;
            }
        }

        #region Private Methods

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
            this.target.GetComponent<CharacterCombat>().Actions.OfType<CharacterAttack>().ForEach(act => act.IncreaseDamage(percent));
        }

        #endregion

        protected override void Deactivate()
        {
            throw new NotImplementedException();
        }
    }
}
