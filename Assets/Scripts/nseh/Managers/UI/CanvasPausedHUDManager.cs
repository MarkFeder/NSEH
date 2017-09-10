using nseh.Managers.Main;
using nseh.Managers.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nseh.Managers.UI
{
    public class CanvasPausedHUDManager : MonoBehaviour
    {

        #region Private Methods

        private void Start()
        {
            GameEvent _gameEvent;
            MinigameEvent _minigameEvent;
            BossEvent _bossEvent;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _gameEvent = GameManager.Instance.GameEvent;
                    _gameEvent.CanvasPause = this.gameObject;
                    break;

                case "Minigame":
                    _minigameEvent = GameManager.Instance.MinigameEvent;
                    _minigameEvent.CanvasPause = this.gameObject;
                    break;

                case "Boss":
                    _bossEvent = GameManager.Instance.BossEvent;
                    _bossEvent.CanvasPause = this.gameObject;
                    break;
            }
        }

        #endregion

        #region Public Methods

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }

        public void RestartGame()
        {
            for (int i = 0; i < GameManager.Instance._numberPlayers; i++)
            {
                for (int j = 0; j < GameManager.Instance._numberPlayers; j++)
                {
                    Debug.Log(i + " " + j);
                    Physics.IgnoreLayerCollision(13 + i, 13 + j, false);

                }

                Physics.IgnoreLayerCollision(13 + i, 12, false);
                Physics.IgnoreLayerCollision(13 + i, 8, false);
            }

            if (SceneManager.GetActiveScene().name == "Game")  
                GameManager.Instance.GameEvent.Restart();
            
            else
                GameManager.Instance.ChangeState(GameManager.States.Game);

            GameManager.Instance.TogglePause();
        }

        public void GoToMainMenu()
        {
            for (int i = 0; i < GameManager.Instance._numberPlayers; i++)
            {
                for (int j = 0; j < GameManager.Instance._numberPlayers; j++)
                {
                    Debug.Log(i + " " + j);
                    Physics.IgnoreLayerCollision(13 + i, 13 + j, false);

                }

                Physics.IgnoreLayerCollision(13 + i, 12, false);
                Physics.IgnoreLayerCollision(13 + i, 8, false);
            }

            GameManager.Instance.ChangeState(GameManager.States.MainMenu);
            GameManager.Instance.TogglePause();
        }

        public void Resume()
        {
            GameManager.Instance.TogglePause(this.gameObject);
        }

        #endregion

    }
}
