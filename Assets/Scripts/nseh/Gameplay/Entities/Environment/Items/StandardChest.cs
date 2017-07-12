using nseh.Gameplay.Base.Abstract.Entities;
using UnityEngine;
using nseh.Managers.Main;

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

        [SerializeField]
        private float _percent;
        [SerializeField]
        private float _seconds;
        [SerializeField]
        private int _points;

        #endregion

        #region Protected Properties

        protected override void Activate()
        {
            
            switch (chestType)
            {
                case StandardChestType.None:

                    Debug.Log("None effect");

                    break;

                case StandardChestType.Health:

                    _playerInfo.IncreaseHealth(_percent);
                    ParticleAnimation(_particlePrefab, 1.5f, _playerInfo.ParticleBodyPos);

                    break;

                case StandardChestType.Damage:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.BonificationAttackForSeconds(_points, _seconds));

                    break;

                case StandardChestType.Velocity:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.BonificationAgilityForSeconds(_points, _seconds));

                    break;

                case StandardChestType.Jump:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.DoubleJumpForSeconds(_seconds));

                    break;

                case StandardChestType.Defense:

                    GameManager.Instance.StartCoroutine(_playerInfo.AddItem(chestType.ToString(), _seconds, ParticleAnimation(_particlePrefab, _seconds, _playerInfo.ParticleBodyPos)));
                    GameManager.Instance.StartCoroutine(_playerInfo.BonificationDefenseForSeconds(_points, _seconds));

                    break;

                default:

                    Debug.Log("No StandardChestType is detected");

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
