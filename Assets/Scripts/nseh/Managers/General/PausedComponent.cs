using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Managers.General
{
    public class PausedComponent : MonoBehaviour
    {
        #region Private Properties
        private LevelManager _LevelManager;
        #endregion

        #region Public Methods
        public void RestartGame()
        {
            _LevelManager.Restart();
        }

        public void GoToMainMenu()
        {
            _LevelManager.GoToMainMenu();
        }

        public void Resume()
        {
            _LevelManager.PauseGame();
        }

        // Use this for initialization
        public void Start()
        {
            _LevelManager = GameManager.Instance.Find<LevelManager>();
        }
        #endregion

    }
}