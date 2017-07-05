using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Managers.Main;
using nseh.Managers.Level;

namespace nseh.Managers.UI
{
    public class CanvasLoadingManager : MonoBehaviour
    {

        #region Private Methods

        void Start()
        {
            GameEvent _gameEvent;
            MinigameEvent _minigameEvent;
            BossEvent _bossEvent;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _gameEvent = GameManager.Instance.Find<GameEvent>();
                    _gameEvent.Loading = this.gameObject;
                    break;

                case "Minigame":
                    _minigameEvent = GameManager.Instance.Find<MinigameEvent>();
                    _minigameEvent.Loading = this.gameObject;
                    break;

                case "Boss":
                    _bossEvent = GameManager.Instance.Find<BossEvent>();
                    _bossEvent.Loading = this.gameObject;
                    break;
            }
        }

        #endregion

    }
}
