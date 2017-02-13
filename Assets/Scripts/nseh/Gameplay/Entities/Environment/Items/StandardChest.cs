using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
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

                    this.IncreaseHealth(0.15f);

                    break;

                case StandardChestType.Damage:

                    this.IncreaseDamage(0.25f);

                    break;

                case StandardChestType.Velocity:

                    this.IncreaseVelocity(0.5f, 4.0f);

                    break;

                case StandardChestType.Jump:

                    this.IncreaseJump(0.5f, 4.0f);

                    break;

                case StandardChestType.Defense:

                    this.SetUpDefense(15.0f, 5);

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
            this.target.GetComponent<CharacterMovement>().IncreaseJumpForSeconds(percent, time);
        }

        private void IncreaseVelocity(float percent, float time)
        {
            this.target.GetComponent<CharacterMovement>().IncreaseSpeedForSeconds(percent, time);
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
