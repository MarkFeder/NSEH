using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using nseh.Managers.Level;
using nseh.Managers.Main;

namespace nseh.Managers.UI
{
    public class CanvasClockHUDManager : MonoBehaviour
    {

        #region Public Properties

        public Text clockText;
        public Text readyText;

        #endregion

        #region Private Methods

        private void Start()
        {
            GameEvent _gameEvent;
            MinigameEvent _minigameEvent;
            BossEvent _bossEvent;

            if (!ValidateCanvasClock())
            {
                Debug.Log("Fields in canvas clock are null");
                enabled = false;
                return;
            }
            else
            {
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Game":
                        _gameEvent = GameManager.Instance.GameEvent;
                        _gameEvent.Clock = clockText;
                        _gameEvent.Ready = readyText;
                        break;

                    case "Minigame":
                        _minigameEvent = GameManager.Instance.MinigameEvent;
                        _minigameEvent.Ready = readyText;
                        break;

                    case "Boss":
                        _bossEvent = GameManager.Instance.BossEvent;
                        _bossEvent.Ready = readyText;
                        break;
                }
            }
        }

        private bool ValidateCanvasClock()
        {
            return clockText && readyText;
        }

        #endregion

    }
}
