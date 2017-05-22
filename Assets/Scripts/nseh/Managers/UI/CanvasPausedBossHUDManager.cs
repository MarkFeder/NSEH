using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine;
using System.Collections.Generic;
using nseh.Gameplay.Gameflow;

namespace nseh.Managers.UI
{
    public class CanvasPausedBossHUDManager : MonoBehaviour 
    {
        #region Private Properties

        private LevelManager _levelManager;

        #endregion

        private void Start()
        {
            _levelManager = GameManager.Instance.Find<LevelManager>();
        }

        #region Public Methods

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Public Event Methods

        public void RestartGame()
        {
            _levelManager.Restart();
        }

        public void GoToMainMenu()
        {
            _levelManager.Find<BossEvent>().EventRelease();
            _levelManager.MyGame.RestartList();
        }

        public void Resume()
        {
            Time.timeScale = 1;
            DisableCanvas();
        }

        #endregion
    }
}
