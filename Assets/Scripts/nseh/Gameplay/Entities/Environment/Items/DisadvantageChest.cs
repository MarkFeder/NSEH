using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Entities;
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
        protected DisadvantageChestType chestType;

        protected override void Activate()
        {
            switch (this.chestType)
            {
                case DisadvantageChestType.None:

                    Debug.Log("None effect");

                    break;

                case DisadvantageChestType.ChestBomb:

                    this.ChestBomb(0.20f);

                    break;

                case DisadvantageChestType.PoisonCloud:

                    this.PoisonCloud(0.1f, 20.0f);

                    break;

                case DisadvantageChestType.ConfusedPotion:

                    StartCoroutine(this.ConfusedPotion(20.0f));

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

        private IEnumerator ConfusedPotion(float time)
        {
            // TODO: invert character control

            yield return new WaitForSeconds(time);

            // TODO: restore character control
        }

        protected override void Deactivate()
        {
            throw new NotImplementedException();
        }
    }
}
