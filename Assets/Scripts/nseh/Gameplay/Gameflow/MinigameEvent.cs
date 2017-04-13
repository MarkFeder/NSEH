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

namespace nseh.Gameplay.Gameflow
{
    public class MinigameEvent : LevelEvent
    {
        private Text _clock;
        private Text _ready;
        private float _timeRemaining;
        private Canvas _canvasClock;
        private GameObject _SpawPoints;
        private GameObject _CubeDeath;
        private bool _stoped;
        private GameObject aux;
        private List<GameObject> _players;
        

        override public void ActivateEvent()
        {

            IsActivated = true;
            _stoped = false;
            _players = new List<GameObject>();
            _SpawPoints = GameObject.Find("SpawnPoints");
            Debug.Log(LvlManager.MyGame._characters.Count);
            for (int i=0; i< LvlManager.MyGame._characters.Count; i++)
            {
                Debug.Log(LvlManager.MyGame._characters[i].name);
                if(LvlManager.MyGame._characters[i].name== "SirProspector")
                {
                    Debug.Log("Creando");
                    aux=Object.Instantiate(Resources.Load("SirProspectorMinigame") as GameObject);
                    Debug.Log(aux);
                    aux.transform.position = _SpawPoints.transform.GetChild(i).transform.position;
                    aux.transform.GetChild(1).GetComponent<TextMinigame>().playerText= i + 1;
                    aux.GetComponent<Minigame>().gamepadIndex = i + 1;
                    _players.Add(aux);
                }
            }
            _CubeDeath = GameObject.Find("Tar/Tar");
            Debug.Log(_CubeDeath);
            _canvasClock = GameObject.Find("CanvasClockHUD").GetComponent<Canvas>();
            Debug.Log(_canvasClock);
            
            _clock = _canvasClock.transform.Find("TextClock").GetComponent<Text>();
            Debug.Log(_clock.text);
            _clock.text = "";
            _ready = _canvasClock.transform.Find("TextReady").GetComponent<Text>();
            StartMinigame(LvlManager.MyGame);
            _timeRemaining = -1;

        }



        override public void EventTick()
        {
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
                    StopMinigame();

                }
            }
            
        }


        override public void EventRelease()
        {
            
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
            _timeRemaining = 60;
            Camera.main.GetComponent<CameraScript>().started = true;
            _CubeDeath.GetComponent<CubeDeath>().started = true;
            foreach(GameObject character in _players)
            {
                character.GetComponent<Minigame>().started = true;
            }
        }

        void StopMinigame()
        {
            _clock.text = "SAFE!";
            Camera.main.GetComponent<CameraScript>().started = false;
            _CubeDeath.GetComponent<CubeDeath>().started = false;
            //LvlManager.ChangeState(LevelManager.States.BossFight);
        }
    }
}