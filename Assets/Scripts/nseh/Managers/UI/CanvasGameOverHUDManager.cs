using nseh.Managers.Level;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasGameOverHUDManager : MonoBehaviour
    {
        #region Public Properties

        public Text gameOverText;
        public Button mainMenuButton;
        public Button restartButton;

        private LevelManager levelManager;

        #endregion

        #region Public C# Properties

        public Text GameOverText
        {
            get { return gameOverText; }
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

            levelManager = nseh.Managers.Main.GameManager.Instance.Find<LevelManager>();
        }

        #region Public Methods

        public bool ValidateGameOverText()
        {
            return gameOverText && mainMenuButton && restartButton;
        }

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }

        public void ChangeGameOverText(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                gameOverText.text = text;
            }
        }

        #endregion

        #region Public Event Methods

        public void RestartGame()
        {
            levelManager.Restart();
        }

        public void GoToMainMenu()
        {
            levelManager.GoToMainMenu();
        }

        #endregion
    }
}
