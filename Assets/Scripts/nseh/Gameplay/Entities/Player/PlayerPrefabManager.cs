using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Managers.Level;
using nseh.Managers.Main;

namespace nseh.Gameplay.Entities.Player
{ 
    public class PlayerPrefabManager : MonoBehaviour
    {

        #region Public Properties

        public List<GameObject> playerPrefabs;

        #endregion

        #region Private Methods

        void Start ()
        {

            GameEvent _gameEvent;
            MinigameEvent _minigameEvent;
            BossEvent _bossEvent;

            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _gameEvent = GameManager.Instance.GameEvent;
                    _gameEvent.PlayerPrefabs = PushToDictionary();
                    break;

                case "Minigame":
                    _minigameEvent = GameManager.Instance.MinigameEvent;
                    _minigameEvent.PlayerPrefabs = PushToDictionary();
                    break;

                case "Boss":
                    _bossEvent = GameManager.Instance.BossEvent;
                    _bossEvent.PlayerPrefabs = PushToDictionary();
                    break;
            }

            Destroy(this.gameObject);
	    }

        Dictionary<string,GameObject> PushToDictionary()
        {
            Dictionary<string, GameObject> playerDictionary = new Dictionary<string, GameObject>();

            foreach(GameObject playeraux in playerPrefabs)
            {
                playerDictionary.Add(playeraux.name, playeraux);
            }

            return playerDictionary;

        }

        #endregion

    }
}
