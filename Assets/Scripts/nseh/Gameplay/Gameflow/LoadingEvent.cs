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
            Debug.Log("Loading "+ aux+ " "+ SceneManager.GetActiveScene().name);
            if (aux != SceneManager.GetActiveScene().name)
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
    }
}
