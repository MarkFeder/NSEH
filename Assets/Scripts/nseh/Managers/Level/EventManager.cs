using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Gameplay.Base.Interfaces;
using nseh.Managers.Main;

namespace nseh.Managers.Level
{
    public class EventManager : MonoBehaviour
    {

        #region Public Properties

        public List<GameObject> _events;

        #endregion

        #region Private Methods

        void Start()
        {
            GameEvent _gameEvent;
            MinigameEvent _minigameEvent;
            BossEvent _bossEvent;

            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _gameEvent = GameManager.Instance.GameEvent;
                    _gameEvent.Events = CastToIEvent();
                    break;
                case "Minigame":
                    _minigameEvent = GameManager.Instance.MinigameEvent;
                    _minigameEvent.Events = CastToIEvent();
                    break;
                case "Boss":
                    _bossEvent = GameManager.Instance.BossEvent;
                    _bossEvent.Events = CastToIEvent();
                    break;
            }
            Destroy(this);
        }

        private List<IEvent> CastToIEvent()
        {
            List<IEvent> _aux = new List<IEvent>();
            foreach (GameObject eventobject in _events)
            {
                _aux.Add(eventobject.GetComponent<IEvent>());
            }
            return _aux;
        }
        #endregion

    }
}
