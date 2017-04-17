using nseh.Managers.Level;
using nseh.Managers.Main;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using nseh.Gameplay.Gameflow;


namespace nseh.Managers.UI
{
    public class CanvasGameOverMinigameHUDManager : MonoBehaviour
    {
        #region Public Properties

        public Text _gameOverText;
        public Button _mainMenuButton;
        public Button _restartButton;

        private LevelManager _levelManager;

        #endregion

        #region Public C# Properties

        public Text GameOverText
        {
            get { return _gameOverText; }
        }

        #endregion

        private void Start()
        {
            if (!ValidateGameOverText())
            {
                Debug.Log("The game over text is null");
                enabled = false;
                return;
            }

            _levelManager = GameManager.Instance.Find<LevelManager>();
        }

        #region Public Methods

        public bool ValidateGameOverText()
        {
            return _gameOverText && _mainMenuButton && _restartButton;
        }

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
            _levelManager.Find<MinigameEvent>().EventRelease();
            _levelManager.MyGame._characters = new List<GameObject>();
            _levelManager.GoToMainMenu();
        }

        #endregion
    }
}
