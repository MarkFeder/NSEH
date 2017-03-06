﻿using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Movement;
using DisadvantageItems = nseh.Utils.Constants.Items.DisadvantageItems;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        #region Public Properties

        public DisadvantageChestType chestType;

        public float percent;
        public float seconds;

        #endregion

        protected override void Activate()
        {
            switch (this.chestType)
            {
                case DisadvantageChestType.None:

                    Debug.Log("None effect");

                    break;

                case DisadvantageChestType.ChestBomb:

                    this.ChestBomb(this.percent);
                    StartCoroutine(DisplayText(itemText, DisadvantageItems.BOMBCHEST, 3));
                    break;

                case DisadvantageChestType.PoisonCloud:

                    this.PoisonCloud(this.percent, this.seconds);
                    StartCoroutine(DisplayText(itemText, DisadvantageItems.POISONCLOUD, 3));
                    break;

                case DisadvantageChestType.ConfusedPotion:

                    this.ConfusedPotion(this.seconds);
                    StartCoroutine(DisplayText(itemText, DisadvantageItems.CONFUSION, 3));
                    break;

                default:

                    Debug.Log("No DisadvantageChestType is detected");

                    break;
            }

            this.Deactivate();
        }

        protected override void Deactivate()
        {
            Destroy(this.gameObject, this.destructionTime);
        }

        private void ChestBomb(float percent)
        {
            this.target.GetComponent<CharacterHealth>().DecreaseHealth(percent);
        }

        private void PoisonCloud(float percent, float seconds)
        {
            this.target.GetComponent<CharacterHealth>().DecreaseHealthForEverySecond(percent, seconds);
        }

        private void ConfusedPotion(float time)
        {
            this.target.GetComponent<PlayerMovement>().InvertControl(time);
        }
    }
}
