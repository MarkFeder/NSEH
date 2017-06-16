using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nseh.Gameplay.Minigames;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Level;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Gameflow
{
    public class MinigameEvent : LevelEvent
    {

        #region Private Properties
        private bool _stoped;
        private bool _isPaused;
        private Text _ready;
        private Canvas _canvasPause;
        private Canvas _canvasGameOver;
        private GameObject _SpawPoints;
        private GameObject _CubeDeath;
        private GameObject _Goal;
        private GameObject _aux;
        private GameObject _fireGenerators;
        private List<int> _puntuation;
        private List<GameObject> _players;
        private Vector3 _gravity;
        private int _auxCount;
        private bool _started;

        #endregion

        #region Public Methods
        public override void ActivateEvent()
        {
            _isActivated = true;
            _stoped = false;
            _isPaused = false;
            _started = false;
            _auxCount = 0;
            _players = new List<GameObject>();
            _puntuation = new List<int>();
            for (int i = 0; i < _levelManager.MyGame._numberPlayers; i++)
                _puntuation.Add(0);

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
            _fireGenerators = GameObject.Find("Main Camera/FireBallGenerators");
            _CubeDeath.GetComponent<CubeDeath>().num = 50 + (4 - _levelManager.MyGame._numberPlayers) * 50;
            _Goal = GameObject.Find("Goal");
            _Goal.GetComponent<Goal>().num = _levelManager.MyGame._numberPlayers * 100;
            _ready = _levelManager.CanvasClockMinigameManager._readyText;
            _levelManager.CanvasPausedMinigameManager.DisableCanvas();
            //_canvasPausedManager.DisableCanvas();
            StartMinigame(_levelManager.MyGame);
            _gravity = Physics.gravity;

        }

        override public void EventTick()
        {
            Debug.Log("TICK");
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))) && _started)
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
            
        }


        override public void EventRelease()
        {
            _puntuation = new List<int>(_levelManager.MyGame._numberPlayers);
            _players = new List<GameObject>();
            _auxCount = 0;
            Physics.gravity = _gravity;
            _isActivated = false;
        }

        #endregion

        #region Private Methods
        private void StartMinigame(MonoBehaviour myMonoBehaviour)
        {
            myMonoBehaviour.StartCoroutine(CountDown());
        }

        private void FinishMinigame(MonoBehaviour myMonoBehaviour)
        {
            foreach (GameObject character in _players)
            {
                _levelManager.MyGame._score[(character.GetComponent<MinigameMovement>().gamepadIndex) - 1, 1] = character.GetComponent<MinigameMovement>().puntuation;     
            }
            myMonoBehaviour.StartCoroutine(ChangeStage());
        }

        private IEnumerator ChangeStage()
        {
            yield return new WaitForSeconds(3);
            Physics.gravity = _gravity;
            EventRelease();
            _levelManager.ChangeState(LevelManager.States.LoadingBoss);
        }


        private IEnumerator CountDown()
        {
            _ready.text = "MASH X BUTTON FOR AVOID LAVA AND FIREBALLS!";
            yield return new WaitForSeconds(3);
            _ready.text = "READY";
            yield return new WaitForSeconds(1);
            _ready.text = "STEADY";
            yield return new WaitForSeconds(1);
            _ready.text = "RUUUUUUN!!!";
            yield return new WaitForSeconds(1);
            _ready.text = "";
            _started = true;
            Physics.gravity = new Vector3(0, 0, -10);
            Camera.main.GetComponent<CameraScript>().started = true;
            _fireGenerators.GetComponent<FireballsGenerator>().started = true;
            foreach (GameObject character in _players)
            {
                character.GetComponent<MinigameMovement>().started = true;
                character.GetComponent<Animator>().SetBool("Start", true);
            }
        }

        public void StopMinigame()
        {
            _ready.text = "SAFE!";
            Camera.main.GetComponent<CameraScript>().started = false;
            _fireGenerators.GetComponent<FireballsGenerator>().started = false;
          
        }


        private void Puntuations()
        {
            foreach (int puntuationAux in _puntuation)
            {     
                    _levelManager.MyGame._score[_puntuation.IndexOf(puntuationAux), 1] = puntuationAux;
            }        
        }


        public void AddPuntuation(int amount, int index)
        {

            Debug.Log("Holis "+ index + " "+amount+" "+_puntuation.Count);
            _puntuation[index] = amount;
            _auxCount++;
            if (_auxCount == _levelManager.MyGame._characters.Count && Camera.main.GetComponent<CameraScript>().started == true)
            {
                _ready.text = "THIS NEVER HAPPENED...";
                Camera.main.GetComponent<CameraScript>().started = false;
                _fireGenerators.GetComponent<FireballsGenerator>().started = false;
                FinishMinigame(_levelManager.MyGame);
            }
            else if (_auxCount == _levelManager.MyGame._characters.Count && Camera.main.GetComponent<CameraScript>().started == false)
            {
                FinishMinigame(_levelManager.MyGame);
            }
        }
        #endregion
    }
}