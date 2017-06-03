using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Enemies;
using nseh.Managers.Level;
using UnityEngine;
using System.Collections;
using nseh.Gameplay.AI;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Gameflow
{
    public class BossEvent : LevelEvent
    {
        #region Private Properties

        private bool _isPaused;
        private GameObject _boss;
        private float _bossHealth;

        #endregion

        #region Public Methods

        public override void ActivateEvent()
        {
            _isActivated = true;
            _isPaused = false;

            _levelManager.CanvasPausedBossManager.DisableCanvas();

            _boss = GameObject.Find("Bava Dongo");
            _boss.GetComponent<EnemyHealth>().MaxHealth = _levelManager.Players.Count * 100;
            _boss.GetComponent<EnemyHealth>().CurrentHealth = _levelManager.Players.Count * 100;
            _boss.GetComponent<BavaDongo_AI>().frenzyHealth = _boss.GetComponent<EnemyHealth>().MaxHealth * _boss.GetComponent<BavaDongo_AI>().percentageFrenzy;
        }

        public override void EventTick()
        {
            _bossHealth = _boss.GetComponent<EnemyHealth>().CurrentHealth;
            Debug.Log("Bava Dongo Health is: " + _bossHealth);

            if (_bossHealth <= 0)
            {
                Debug.Log("BavaDongo died!");
                EventRelease();
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
            Debug.Log("RELEASE " + _levelManager.Players.Count);

            foreach (PlayerManager character in _levelManager.Players)
            {
                _levelManager.MyGame._score[character.PlayerRunTimeInfo.GamepadIndex - 1, 2] = character.PlayerRunTimeInfo.PlayerScore.Score;
                Debug.Log(_levelManager.MyGame._score[character.PlayerRunTimeInfo.GamepadIndex - 1, 2]);
            }

            GoToScoreMenu(_levelManager.MyGame);
            


        }

        private void GoToScoreMenu(MonoBehaviour myMonoBehaviour)
        {
            myMonoBehaviour.StartCoroutine(ScoreMenu());
        }


        private IEnumerator ScoreMenu()
        {
            yield return new WaitForSeconds(5);
            _levelManager.GoToMainMenuScore();
        }

        #endregion
    }
}