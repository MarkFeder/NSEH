using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Level;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Enemies;
using nseh.Gameplay.Entities.Player;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Gameflow
{
    public class BossEvent : LevelEvent
    {

        private bool _isPaused;
        private GameObject _boss;
        private float _bossHealth;
        private List<PlayerManager> _players;
        private GameObject _aux;


        // Use this for initialization
        public override void ActivateEvent()
        {
            _isActivated = true;
            _isPaused = false;
            _players = _levelManager.Players;
            _levelManager.CanvasPausedBossManager.DisableCanvas();
            _boss = GameObject.Find("Bava Dongo");
            _boss.GetComponent<EnemyHealth>().MaxHealth = _players.Count * 200;
            _boss.GetComponent<EnemyHealth>().CurrentHealth = _players.Count * 200;

        }

        public override void EventTick()
        {
           
            _bossHealth= _boss.GetComponent<EnemyHealth>().CurrentHealth;
            Debug.Log("tick" + _bossHealth);
            if (_bossHealth<=0)
            {
                Debug.Log("Se murió :(");
                EventRelease();
                //_levelManager.GoToMainMenuScore();
                //_levelManager.MyGame.StartCoroutine("EventRelease");

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
            Debug.Log("rELEASE"+ _players.Count);
            foreach (PlayerManager character in _players)
            {
                _levelManager.MyGame._score[character.PlayerRunTimeInfo.GamepadIndex - 1, 2] = character.PlayerRunTimeInfo.Score;
                Debug.Log(_levelManager.MyGame._score[character.PlayerRunTimeInfo.GamepadIndex - 1, 2]);
                //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+character.PlayerRunTimeInfo.Score);
            }
            
           _levelManager.GoToMainMenuScore();
           

        }
    }
}