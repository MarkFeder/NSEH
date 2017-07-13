using nseh.Gameplay.Base.Abstract.Entities;
using UnityEngine;
using nseh.Managers.Main;

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

        #region Protected Methods

        protected override void Activate()
        {
            switch (_chestType)
            {
                case DisadvantageChestType.None:

                    Debug.Log("None effect");

                    break;

                case DisadvantageChestType.ChestBomb:

                    _playerInfo.DecreaseHealth(_percent);
                    ParticleAnimation(_particlePrefab, _playerInfo.ParticleBodyPos);

                    break;

                case DisadvantageChestType.PoisonCloud:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(_chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.DecreaseHealthForEverySecond(_percent, _seconds));

                    break;

                case DisadvantageChestType.ConfusedPotion:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(_chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.InvertControlForSeconds(_seconds));

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

        #endregion

    }
}
