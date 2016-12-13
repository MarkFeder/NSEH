using UnityEngine;
using System.Collections;

namespace NSEH
{
    public class GameManager_Master : MonoBehaviour {

        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler MenuToogleEvent;
        public event GameManagerEventHandler GoToMenuSceneEvent;
        public event GameManagerEventHandler RestartEvent;
        public event GameManagerEventHandler TimesUpEvent;
        public event GameManagerEventHandler Player1WinsEvent;
        public event GameManagerEventHandler Player2WinsEvent;

        public bool isTimesUp;
        

        public void CallEventMenuToogle()
        {
            if (MenuToogleEvent != null)
            {
                MenuToogleEvent();
            }
        }

       public void CallEventGoToMenuScene()
        {
            if (GoToMenuSceneEvent != null)
            {
               GoToMenuSceneEvent();
            }
        }

        public void CallRestart()
        {
            if (RestartEvent != null)
            {
                RestartEvent();
            }
        }


        public void CallEventTimesUp()
        {
            if (TimesUpEvent != null)
            {
                isTimesUp = true;

                TimesUpEvent();
            }
            /*
            else if (Player1WinsEvent != null)
            {
                isGameOver = true;
                Player1WinsEvent();
            }
            else if (Player2WinsEvent != null)
            {
                isGameOver = true;
                Player2WinsEvent();
            }*/
        }

    }
}


