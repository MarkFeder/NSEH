using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Enemies;
using nseh.Managers.Level;
using UnityEngine;
using System.Collections;
using nseh.Gameplay.AI;
using UnityEngine.UI;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Gameflow
{
    public class BossEvent : LevelEvent
    {
        #region Private Properties

        private bool _isPaused;
        private GameObject _boss;
        private Animator _animator;
        private Text _ready;

        #endregion

        #region Public Methods

        public override void ActivateEvent()
        {
            _isActivated = true;
            _isPaused = false;
            _ready = GameObject.Find("CanvasProgressHUD/TextReady").GetComponent<Text>();
            _levelManager.CanvasPausedBossManager.DisableCanvas();
            _animator = _boss.GetComponent<Animator>();
            _boss = GameObject.Find("Bava Dongo");
            _boss.GetComponent<EnemyHealth>().MaxHealth = _levelManager.Players.Count * 100;
            _boss.GetComponent<EnemyHealth>().CurrentHealth = _levelManager.Players.Count * 100;
            _boss.GetComponent<BavaDongo_AI>().frenzyHealth = _boss.GetComponent<EnemyHealth>().MaxHealth * _boss.GetComponent<BavaDongo_AI>().percentageFrenzy;
            StartBoss(_levelManager.MyGame);

        }

        public override void EventTick()
        {
           

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Death"))
            {
                StopBoss(_levelManager.MyGame);
                
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

            foreach (PlayerManager character in _levelManager.Players)
            {
                _levelManager.MyGame._score[character.PlayerRunTimeInfo.GamepadIndex - 1, 2] = character.PlayerRunTimeInfo.PlayerScore.Score;
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

        private void StartBoss(MonoBehaviour myMonoBehaviour)
        {
            myMonoBehaviour.StartCoroutine(StartingBoss());
        }


        private IEnumerator StartingBoss()
        {
            _ready.text = "DEFEAT THE BOSS TOGETHER!";
            yield return new WaitForSeconds(3);
            _ready.text = "";

        }

        private void StopBoss(MonoBehaviour myMonoBehaviour)
        {
            myMonoBehaviour.StartCoroutine(StopingBoss());
        }


        private IEnumerator StopingBoss()
        {
            _ready.text = "BAVA DONGO IS DEAD! YOU WIN!";
            yield return new WaitForSeconds(10);
            _ready.text = "";
            EventRelease();
        }

        #endregion
    }
}