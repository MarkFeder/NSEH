using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine;
using System.Collections.Generic;
using nseh.Gameplay.Gameflow;

namespace nseh.Managers.UI
{
    public class CanvasPausedMinigameHUDManager : MonoBehaviour
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
            /*
            _levelManager.ChangeState(LevelManager.States.Restart);
            _levelManager.MyGame.ChangeState(GameManager.States.Loading);
            _levelManager.MyGame.ChangeState(GameManager.States.Playing);
            _levelManager.ChangeState(LevelManager.States.LevelEvent);
            */

        }


        public void GoToMainMenu()
        {
            _levelManager.Find<MinigameEvent>().EventRelease();
            _levelManager.MyGame._characters= new List<GameObject>();
            _levelManager.GoToMainMenu();
        }

        public void Resume()
        {
            Time.timeScale = 1;
            DisableCanvas();
        }

        #endregion
    }
}
