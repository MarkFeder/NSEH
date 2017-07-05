using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Managers.Main;

namespace nseh.Managers.Level
{
    public class SpawnPointManager : MonoBehaviour
    {

        #region Public Properties

        public List<GameObject> _playerSpawnPoints;

        #endregion

        #region Private Methods

        void Start()
        {
            GameEvent _gameEvent;
            MinigameEvent _minigameEvent;
            BossEvent _bossEvent;

            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _gameEvent = GameManager.Instance.GameEvent;
                    _gameEvent.SpawnPoints = _playerSpawnPoints;
                    break;
                case "Minigame":
                    _minigameEvent = GameManager.Instance.MinigameEvent;
                    _minigameEvent.SpawnPoints = _playerSpawnPoints;
                    break;

                case "Boss":
                    _bossEvent = GameManager.Instance.BossEvent;
                    _bossEvent.SpawnPoints = _playerSpawnPoints;
                    break;
            }

            Destroy(this);
        }

        #endregion

    }
}
