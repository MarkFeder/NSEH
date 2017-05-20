using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Level;
using nseh.Gameplay.Minigames;
using nseh.Gameplay.Base.Abstract.Gameflow;
using UnityEngine.SceneManagement;

namespace nseh.Gameplay.Gameflow
{
    public class LoadingEvent : LevelEvent
    {

        #region Private Properties
        private string _aux;
        nseh.Managers.Level.LevelManager.States state;
        #endregion

        #region Public Methods
        public override void ActivateEvent()
        {
            _isActivated = true;
            _aux = SceneManager.GetActiveScene().name;
        }

        public override void EventTick()
        {
            if (_aux != SceneManager.GetActiveScene().name)
            {
                EventRelease();
            }

        }

        public override void EventRelease()
        {
            _isActivated = false;
            if (SceneManager.GetActiveScene().name=="Minigame")
            {
                _levelManager.ChangeState(LevelManager.States.Minigame);
            }
            else
            {
                _levelManager.ChangeState(LevelManager.States.LevelEvent);
            }  
        }
        #endregion

    }
}
