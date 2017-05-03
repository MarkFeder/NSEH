using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Entities.Player;
using UnityEngine;
using DisadvantageItems = nseh.Utils.Constants.Items.DisadvantageItems;

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
        #region Private Properties

        [SerializeField]
        private DisadvantageChestType _chestType;

        [SerializeField]
        private float _percent;
        [SerializeField]
        private float _seconds;

        #endregion

        protected override void Activate()
        {
            switch (_chestType)
            {
                case DisadvantageChestType.None:

                    Debug.Log("None effect");

                    break;

                case DisadvantageChestType.ChestBomb:

                    ChestBomb(_percent);
                    _spawnItemPoint.DisplayText(_itemText, DisadvantageItems.BOMBCHEST, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, 1.0f, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case DisadvantageChestType.PoisonCloud:

                    PoisonCloud(_percent, _seconds);
                    _spawnItemPoint.DisplayText(_itemText, DisadvantageItems.POISONCLOUD, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, _seconds, _particlesSpawnPoints.ParticleBodyPos);

                    break;

                case DisadvantageChestType.ConfusedPotion:

                    ConfusedPotion(_seconds);
                    _spawnItemPoint.DisplayText(_itemText, DisadvantageItems.CONFUSION, _timeToDisplayText);
                    ParticleAnimation(_particlePrefab, _seconds, _particlesSpawnPoints.ParticleHeadPos);

                    break;

                default:

                    Debug.Log("No DisadvantageChestType is detected");

                    break;
            }

            Deactivate();
        }

        protected override void Deactivate()
        {
            Destroy(gameObject, _destructionTime);
        }

        #region Private Methods

        private void ChestBomb(float percent)
        {
            _target.GetComponent<PlayerHealth>().DecreaseHealth(percent);
        }

        private void PoisonCloud(float percent, float seconds)
        {
            _target.GetComponent<PlayerHealth>().DecreaseHealthForEverySecond(percent, seconds);
        }

        private void ConfusedPotion(float time)
        {
            _target.GetComponent<PlayerMovement>().InvertControl(time);
        }

        #endregion
    }
}
