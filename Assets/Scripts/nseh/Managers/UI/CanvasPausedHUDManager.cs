using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasPausedHUDManager : MonoBehaviour
    {
        #region Private Properties

        private LevelManager levelManager;

        #endregion

        private void Start()
        {
            levelManager = GameManager.Instance.Find<LevelManager>();
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
            levelManager.Restart();
        }

        public void GoToMainMenu()
        {
            levelManager.GoToMainMenu();
        }

        public void Resume()
        {
            levelManager.PauseGame();
        }

        #endregion
    }
}
