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
            IsActivated = true;
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
            IsActivated = false;
            if (SceneManager.GetActiveScene().name=="Minigame")
            {
                LvlManager.ChangeState(LevelManager.States.Minigame);
            }
            else
            {
                LvlManager.ChangeState(LevelManager.States.LevelEvent);
            }  
        }
        #endregion

    }
}
