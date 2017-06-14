using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nseh.Managers.UI;
using nseh.Gameplay.Minigames;
using nseh.Gameplay.Movement;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Level;
using nseh.Utils;
using LevelHUDConstants = nseh.Utils.Constants.InLevelHUD;
using Inputs = nseh.Utils.Constants.Input;
using InputUE = UnityEngine.Input;

namespace nseh.Gameplay.Gameflow
{
    public class MinigameEvent : LevelEvent
    {

        #region Private Properties
        private bool _stoped;
        private bool _isPaused;
        private float _timeRemaining;
        private Text _clock;
        private Text _ready;
        private Canvas _canvasClock;
        private Canvas _canvasPause;
        private Canvas _canvasGameOver;
        private GameObject _SpawPoints;
        private GameObject _CubeDeath;
        private GameObject _Goal;
        private GameObject _aux;
        private GameObject _platformGenerators;
        private List<GameObject> _players;

        #endregion

        #region Public Methods
        public override void ActivateEvent()
        {
            _isActivated = true;
            _stoped = false;
            _isPaused = false;
            _players = new List<GameObject>();
            _SpawPoints = GameObject.Find("SpawnPoints");

            for (int i = 0; i < _levelManager.MyGame._characters.Count; i++)
            {
                Debug.Log(_levelManager.MyGame._characters[i].name);
                if (_levelManager.MyGame._characters[i].name == "SirProspector")
                {
                    _aux = UnityEngine.Object.Instantiate(Resources.Load("SirProspectorMinigame") as GameObject);
                    _aux.transform.position = _SpawPoints.transform.GetChild(i).transform.position;
                    _aux.transform.GetChild(1).GetComponent<TextMinigame>().playerText = i + 1;
                    _aux.GetComponent<MinigameMovement>().gamepadIndex = i + 1;
                    _players.Add(_aux);
                }
                else if (_levelManager.MyGame._characters[i].name == "Wrarr")
                {
                    _aux = UnityEngine.Object.Instantiate(Resources.Load("WrarrMinigame") as GameObject);
                    _aux.transform.position = _SpawPoints.transform.GetChild(i).transform.position;
                    _aux.transform.GetChild(1).GetComponent<TextMinigame>().playerText = i + 1;
                    _aux.GetComponent<MinigameMovement>().gamepadIndex = i + 1;
                    _players.Add(_aux);
                }
            }
            _CubeDeath = GameObject.Find("Main Camera/lava");
            _CubeDeath.GetComponent<CubeDeath>().num = 50 + (4 - _levelManager.MyGame._numberPlayers) * 50;
            _Goal = GameObject.Find("Goal");
            _clock = _levelManager.CanvasClockMinigameManager._clockText;
            _clock.text = "";
            _ready = _levelManager.CanvasClockMinigameManager._readyText;
            _levelManager.CanvasPausedMinigameManager.DisableCanvas();
            _levelManager.CanvasGameOverMinigameManager.DisableCanvas();
            //_canvasPausedManager.DisableCanvas();
            StartMinigame(_levelManager.MyGame);
            _timeRemaining = -1;

        }

        override public void EventTick()
        {
            Debug.Log("TICK");
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))) && _timeRemaining > 0)
            {
                _isPaused = !_isPaused;

                if (_isPaused)
                {
                    _levelManager.CanvasPausedMinigameManager.EnableCanvas();
                    Time.timeScale = 0;
                }
                else
                {
                    _levelManager.CanvasPausedMinigameManager.DisableCanvas();
                    Time.timeScale = 1;
                }
            }

            if (_timeRemaining != -1)
            {
                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining > 0 && _timeRemaining > 10)
                {
                    _clock.text = _timeRemaining.ToString("f0");
                }

                else if (_timeRemaining > 0 && _timeRemaining < 10)
                {
                    _clock.text = _timeRemaining.ToString("f2");
                }
                else if (_stoped == false)
                {
                    _stoped = true;
                    _levelManager.MyGame.StartCoroutine(StopMinigame());
                }
            }
            
        }


        override public void EventRelease()
        {
            _players = new List<GameObject>();

            _isActivated = false;
        }

        #endregion

        #region Private Methods
        private void StartMinigame(MonoBehaviour myMonoBehaviour)
        {
            //Camera.main.GetComponent<CameraScript>().num = _levelManager.numPlayers;
            //GameObject.Find("Camera").GetComponent<Camera>().GetComponent<CameraScript>().num = _levelManager.numPlayers;
            myMonoBehaviour.StartCoroutine(CountDown());
        }

        private IEnumerator CountDown()
        {
            _ready.text = "READY";
            yield return new WaitForSeconds(1);
            _ready.text = "STEADY";
            yield return new WaitForSeconds(1);
            _ready.text = "RUUUUUUN!!!";
            yield return new WaitForSeconds(1);
            _ready.text = "";
            _timeRemaining = Constants.LevelManager.MINIGAME_TIME;
            Camera.main.GetComponent<CameraScript>().started = true;
            //GameObject.Find("Camera").GetComponent<Camera>().GetComponent<CameraScript>().started = true;
            foreach (GameObject character in _players)
            {
                character.GetComponent<MinigameMovement>().started = true;
                character.GetComponent<Animator>().SetBool("Start", true);
            }
        }

        private IEnumerator StopMinigame()
        {
            _clock.text = "SAFE!";
            Camera.main.GetComponent<CameraScript>().started = false;
            //GameObject.Find("Camera").GetComponent<Camera>().GetComponent<CameraScript>().started = false;
            yield return new WaitForSeconds(5);
            /*
            foreach (GameObject character in _players)
            {
                if (character.GetComponent<Minigame>().position != 0)
                {
                    _levelManager.MyGame._score[(character.GetComponent<Minigame>().gamepadIndex) - 1, 1] = character.GetComponent<Minigame>().position;
                }
            }*/
            EventRelease();
            //LvlManager.GoToMainMenuScore();
            //LvlManager.CanvasGameOverMinigameManager.EnableCanvas();         
            _levelManager.ChangeState(LevelManager.States.LoadingBoss);
        }
        #endregion
    }
}