using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Movement;
using System;
using System.Collections;
using UnityEngine;

namespace nseh.Gameplay.Entities.Environment.Items
{
    public enum DisadvantageChestType
    {
        None = 0,
        ChestBomb = 1,
        PoisonCloud = 2,
        ConfusedPotion = 3
    }

    public class DisadvantageChest : Chest
    {
        public DisadvantageChestType chestType;

        [Range(0,1)]
        public float percent;
        public float seconds;

        protected override void Activate()
        {
            switch (this.chestType)
            {
                case DisadvantageChestType.None:

                    Debug.Log("None effect");

                    break;

                case DisadvantageChestType.ChestBomb:

                    this.ChestBomb(this.percent);

                    break;

                case DisadvantageChestType.PoisonCloud:

                    this.PoisonCloud(this.percent, this.seconds);

                    break;

                case DisadvantageChestType.ConfusedPotion:

                    this.ConfusedPotion(this.seconds);

                    break;

                default:

                    Debug.Log("No StandardChestType is detected");

                    break;
            }
        }

        private void ChestBomb(float percent)
        {
            this.target.GetComponent<CharacterHealth>().DecreaseHealth(percent);
        }

        private void PoisonCloud(float percent, float seconds)
        {
            this.target.GetComponent<CharacterHealth>().DecreaseHealthForSeconds(percent, seconds);
        }

        private void ConfusedPotion(float time)
        {
            this.target.GetComponent<PlayerMovement>().InvertControl(time);
        }

        protected override void Deactivate()
        {
            throw new NotImplementedException();
        }
    }
}
