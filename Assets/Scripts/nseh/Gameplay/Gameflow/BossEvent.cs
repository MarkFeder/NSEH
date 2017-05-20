using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Level;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Enemies;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Gameflow
{
    public class BossEvent : LevelEvent
    {

        private bool _isPaused;
        private GameObject _boss;
        private float _bossHealth;
        private List<GameObject> _players;
        private GameObject _aux;


        // Use this for initialization
        public override void ActivateEvent()
        {
            _isActivated = true;
            _isPaused = false;
            _players = new List<GameObject>();

            _levelManager.CanvasPausedBossManager.DisableCanvas();
            _boss = GameObject.Find("Bava Dongo");
            _bossHealth = GameObject.Find("Bava Dongo").GetComponent<EnemyHealth>().CurrentHealth;
        }

        public override void EventTick()
        {
            _bossHealth= _boss.GetComponent<EnemyHealth>().CurrentHealth;
            if (_bossHealth <= 0 )
            {
                Debug.Log("Se murió :(");
                _levelManager.GoToMainMenuScore();
            }

            else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))))
            {
                _isPaused = !_isPaused;

                if (_isPaused)
                {
                    _levelManager.CanvasPausedBossManager.EnableCanvas();
                    Time.timeScale = 0;
                }
                else
                {
                    _levelManager.CanvasPausedBossManager.DisableCanvas();
                    Time.timeScale = 1;
                }
            }
        }

        public override void EventRelease()
        {
            _isActivated = false;

        }
    }
}