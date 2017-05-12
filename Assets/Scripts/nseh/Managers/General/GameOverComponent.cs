using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Managers.General
{
    public class GameOverComponent : MonoBehaviour
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


        // Use this for initialization
        void Start()
        {
            _LevelManager = GameManager.Instance.Find<LevelManager>();
        }
        #endregion
    }
}
