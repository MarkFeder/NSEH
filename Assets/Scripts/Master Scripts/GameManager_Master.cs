using UnityEngine;
using System.Collections;

namespace NSEH
{
    public class GameManager_Master : MonoBehaviour {

        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler MenuToogleEvent;
        public event GameManagerEventHandler GoToMenuEvent;
        public event GameManagerEventHandler GameOverEvent;

        public bool isGameOver;
        public bool isMenuOn;

       public void CallEventMenuToogle()
        {
            if (MenuToogleEvent != null)
            {
                MenuToogleEvent();
            }
        }

       public void CallEventGoToMenuScene()
        {
            if (GoToMenuEvent != null)
            {
               GoToMenuEvent();
            }
        }

       public void CallEventGameOver()
        {
            if (GameOverEvent != null)
            {
                isGameOver = true;
                GameOverEvent();
            }
        }
    }
}


