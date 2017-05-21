using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Level;
using UnityEngine.SceneManagement;

namespace nseh.Gameplay.Gameflow
{
    public class LoadingEvent : LevelEvent
    {
        #region Private Properties

        private string _scene;
        private LevelManager.States state;

        #endregion

        #region Public Methods

        public override void ActivateEvent()
        {
            _isActivated = true;

            _scene = SceneManager.GetActiveScene().name;
        }

        public override void EventTick()
        {
            if (_scene != SceneManager.GetActiveScene().name)
            {
                EventRelease();
            }
        }

        public override void EventRelease()
        {
            _isActivated = false;

            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == "Minigame")
            {
                _levelManager.ChangeState(LevelManager.States.Minigame);
            }
            else if (sceneName == "Boss")
            {
                _levelManager.ChangeState(LevelManager.States.Boss);
            }
            else
            {
                _levelManager.ChangeState(LevelManager.States.LevelEvent);
            }
        }

        #endregion
    }
}
