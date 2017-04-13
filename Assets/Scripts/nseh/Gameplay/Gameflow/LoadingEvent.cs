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
        string aux;
        nseh.Managers.Level.LevelManager.States state;


        // Use this for initialization


        public override void ActivateEvent()
        {
            IsActivated = true;
            aux = SceneManager.GetActiveScene().name;
        }

        public override void EventTick()
        {
            Debug.Log("Laoding "+ aux+ " "+ SceneManager.GetActiveScene().name);
            if (aux != SceneManager.GetActiveScene().name)
            {
                EventRelease();
            }

        }

        public override void EventRelease()
        {
            IsActivated = false;
            LvlManager.ChangeState(LevelManager.States.Minigame);
       
            
        }
    }
}
