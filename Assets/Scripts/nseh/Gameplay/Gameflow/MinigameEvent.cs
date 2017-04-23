
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nseh.Managers.UI;
using nseh.Gameplay.Minigames;
using nseh.Gameplay.Movement;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Level;
using LevelHUDConstants = nseh.Utils.Constants.InLevelHUD;
using Inputs = nseh.Utils.Constants.Input;
using InputUE = UnityEngine.Input;

namespace nseh.Gameplay.Gameflow
{
    public class MinigameEvent : LevelEvent
    {
        private Text _clock;
        private Text _ready;
        private float _timeRemaining;
        private Canvas _canvasClock;
        private Canvas _canvasPause;
        private Canvas _canvasGameOver;
        private GameObject _SpawPoints;
        private GameObject _CubeDeath;
        private GameObject _Goal;
        private bool _stoped;
        private GameObject aux;
        private GameObject _platformGenerators;
        private List<GameObject> _players;
        private bool _isPaused;
        

        override public void ActivateEvent()
        {

            IsActivated = true;
            _stoped = false;
            _isPaused = false;
            _players = new List<GameObject>();
            _SpawPoints = GameObject.Find("SpawnPoints");
            _platformGenerators = GameObject.Find("PlatformGenerators");
            Debug.Log(LvlManager.MyGame._characters.Count);

            for (int i=0; i< LvlManager.MyGame._characters.Count; i++)
            {
                Debug.Log(LvlManager.MyGame._characters[i].name);
                if(LvlManager.MyGame._characters[i].name== "SirProspector")
                {
                    Debug.Log("Creando");
                    aux= UnityEngine.Object.Instantiate(Resources.Load("SirProspectorMinigame") as GameObject);
                    Debug.Log(aux);
                    aux.transform.position = _SpawPoints.transform.GetChild(i).transform.position;
                    aux.transform.GetChild(1).GetComponent<TextMinigame>().playerText= i + 1;
                    aux.GetComponent<Minigame>().gamepadIndex = i + 1;
                    _platformGenerators.transform.GetChild(i).gameObject.SetActive(true);
                    _players.Add(aux);
                }
                else if (LvlManager.MyGame._characters[i].name == "Wrarr")
                {
                    aux = UnityEngine.Object.Instantiate(Resources.Load("WrarrMinigame") as GameObject);
                    Debug.Log(aux);
                    aux.transform.position = _SpawPoints.transform.GetChild(i).transform.position;
                    aux.transform.GetChild(1).GetComponent<TextMinigame>().playerText = i + 1;
                    aux.GetComponent<Minigame>().gamepadIndex = i + 1;
                    _platformGenerators.transform.GetChild(i).gameObject.SetActive(true);
                    _players.Add(aux);
                }
            }
            _CubeDeath = GameObject.Find("Tar/Tar");
            _CubeDeath.GetComponent<CubeDeath>().num = 50+ (4 -LvlManager.MyGame._numberPlayers)*50;
            _Goal = GameObject.Find("Goal");
            Debug.Log(_CubeDeath);
            /*
            _canvasClock = GameObject.Find("CanvasClockHUD").GetComponent<Canvas>();
            Debug.Log(_canvasClock);
            _canvasPause = GameObject.Find("CanvasPausedHUD").GetComponent<Canvas>();
            Debug.Log(_canvasPause);
            _canvasPause.gameObject.SetActive(false);
            
            _canvasGameOver = GameObject.Find("CanvasGameOverHUD").GetComponent<Canvas>();
            _canvasGameOver.enabled = false;
            */
            _clock = LvlManager.CanvasClockMinigameManager._clockText;
               
            
            _clock.text = "";
            _ready = LvlManager.CanvasClockMinigameManager._readyText;
            LvlManager.CanvasPausedMinigameManager.DisableCanvas();
            LvlManager.CanvasGameOverMinigameManager.DisableCanvas();
            //_canvasPausedManager.DisableCanvas();
            StartMinigame(LvlManager.MyGame);
            _timeRemaining = -1;

        }



        override public void EventTick()
        {
            Debug.Log(Input.GetButton(System.String.Format("{0}{1}", Inputs.OPTIONS, 1)));
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))) && _timeRemaining>0)
            {
                _isPaused = !_isPaused;

                if (_isPaused)
                {
                    LvlManager.CanvasPausedMinigameManager.EnableCanvas();
                    Time.timeScale = 0;
                }
                else
                {
                    LvlManager.CanvasPausedMinigameManager.DisableCanvas();
                    Time.timeScale = 1;
                }
            }


            if(_timeRemaining != -1)
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
                    LvlManager.MyGame.StartCoroutine(StopMinigame());

                }
            }
            
        }


        override public void EventRelease()
        {
            _players = new List<GameObject>();
            IsActivated = false;

        }


        void StartMinigame(MonoBehaviour myMonoBehaviour)
        {/*
            switch (LvlManager.numPlayers)
            {
                case 1:
                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(-0.35f, 4.77f, 0.02f)
                    };

                    break;

                case 2:
                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(-0.35f, 4.77f, 0.02f),
                        new Vector3(-0.35f, 1.6f, 0.02f)
                    };

                    break;

            }
            */
            //COLOCAR LOS PERSONAJES Y SUS INDICADORES
            Camera.main.GetComponent<CameraScript>().num = LvlManager.numPlayers;  
            myMonoBehaviour.StartCoroutine(CountDown());
            
        }



        IEnumerator CountDown()
        {
            _ready.text = "READY";
            yield return new WaitForSeconds(1);
            _ready.text = "STEADY";
            yield return new WaitForSeconds(1);
            _ready.text = "RUUUUUUN!!!";
            yield return new WaitForSeconds(1);
            _ready.text = "";
            _timeRemaining = 30;
            Camera.main.GetComponent<CameraScript>().started = true;
            _CubeDeath.GetComponent<CubeDeath>().started = true;
            _Goal.GetComponent<Goal>().started = true;
            foreach (GameObject character in _players)
            {
                character.GetComponent<Minigame>().started = true;
            }
        }

        IEnumerator StopMinigame()
        {
            _clock.text = "SAFE!";
            Camera.main.GetComponent<CameraScript>().started = false;
            _CubeDeath.GetComponent<CubeDeath>().started = false;
            _Goal.GetComponent<Goal>().started = false;
            yield return new WaitForSeconds(3);
            foreach (GameObject character in _players)
            {
                LvlManager.MyGame._score[character.GetComponent<Minigame>().gamepadIndex, 1]= character.GetComponent<Minigame>().position;
            }
            LvlManager.CanvasGameOverMinigameManager.EnableCanvas();
            //LvlManager.ChangeState(LevelManager.States.BossFight);
        
        }
    }
}