using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Managers.General
{
    public class PausedComponent : MonoBehaviour
    {

        LevelManager _LevelManager;

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
        void Start()
        {
            _LevelManager = GameManager.Instance.Find<LevelManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    } 
}