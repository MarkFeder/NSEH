﻿using UnityEngine;
using System.Collections;

namespace nseh.GameManager
{
    public class GameManager_TogglePause : MonoBehaviour {


        private GameManager_Master gameManagerMaster;
        private bool isPaused;

	    void OnEnable ()
        {
            SetInitialPreferences();
            gameManagerMaster.MenuToogleEvent += TogglePause;
	    }
	
	    void OnDisable()
        {
            gameManagerMaster.MenuToogleEvent -= TogglePause;
        }

        void SetInitialPreferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void TogglePause()
        {
            if (isPaused)
            {
                Time.timeScale = 0;
                isPaused = false;
            }
            else
            {
                Time.timeScale = 1;
                isPaused = true;
            }
        }
    }
}



