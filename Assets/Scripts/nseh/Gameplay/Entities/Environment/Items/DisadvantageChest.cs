using nseh.Gameplay.Base.Abstract.Entities;
using UnityEngine;
using nseh.Managers.Main;

namespace nseh.Gameplay.Entities.Environment.Items
{
    public enum DisadvantageChestType
    {
        None = 0,
        PoisonCloud = 1,
        Vulnerable = 2,
        ConfusedPotion = 3,
        LessEnergy = 4,
        Disarmed = 5
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

                case DisadvantageChestType.PoisonCloud:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(_chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleHeadPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.DecreaseHealthForEverySecond(_percent, _seconds));

                    break;

                case DisadvantageChestType.Vulnerable:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(_chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.VulnerableForSeconds(_seconds));

                    break;

                case DisadvantageChestType.ConfusedPotion:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(_chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleHeadPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.InvertControlForSeconds(_seconds));

                    break;

                case DisadvantageChestType.LessEnergy:

                    _playerInfo.DecreaseEnergy(50);
                    ParticleAnimation(_particlePrefab, _playerInfo.ParticleBodyPos);

                    break;

                case DisadvantageChestType.Disarmed:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(_chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.PlayerCombat.DisarmedForSeconds(_seconds));

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
