﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.GameManager;

namespace nseh.Gameplay.Gameflow
{
    public class Tar_Event : LevelEvent
    {

        //List<EventComponent> _tarComponents;
        float eventDuration = 10.0f;
        //bool eventFinished = false;
        public float elapsedTime;
        bool isUp = false;
        //Event components should suscribe their movement functions here to be handled by event
        public delegate bool TarHandler(float gameTime);
        public static event TarHandler TarUp;
        public static event TarHandler TarDown;

        //Resets all event component positions
        public delegate void TarReset();
        public static event TarReset ResetTarComponents;
        //Setup the event providing the current game instance. The event is not active here yet.
        override public void Setup(LevelManager lvlManager)
        {
            base.Setup(lvlManager);
            //_tarComponents = new List<EventComponent>();
        }

        //Activate the event execution.
        override public void ActivateEvent()
        {
            IsActivated = true;
            ResetTarComponents();
            eventDuration = 10.0f;
            elapsedTime = 0;
            isUp = false;
        }


        //Event execution.
        override public void EventTick()
        {
            elapsedTime += Time.deltaTime;
            //Controls when the tar should go up
            if (elapsedTime >= 5.0f && elapsedTime < (5.0f + eventDuration) && !isUp)
            {
                //foreach(EventComponent tarComponent in _tarComponents)
                //{
                isUp = TarUp(elapsedTime);
                //}

            }
            //Controls when the tar should go down
            else if (elapsedTime >= (5.0f + eventDuration) && isUp)
            {
                isUp = TarDown(elapsedTime);
            }
            //Controls when the event cycle is completed and resets the involved variables
            else if (elapsedTime >= (5.0f + eventDuration) && !isUp)
            {
                if (eventDuration != 45.0f)
                {
                    eventDuration += 5.0f;
                }
                elapsedTime = 0;
                Debug.Log("Variables are reset and tar will remain up next time: " + eventDuration + " seconds.");
            }
            //LvlManager.ChangeState(LevelManager.States.LevelEvent);
        }

        //Deactivates the event
        override public void EventRelease()
        {
            ResetTarComponents();
            eventDuration = 10f;
            elapsedTime = 0;
            isUp = false;
            IsActivated = false;
        }
        /*
            public void RegisterLight(MonoBehaviour componentToRegister)
            {
                _tarComponents.Add(componentToRegister);
            }
        */
    } 
}
