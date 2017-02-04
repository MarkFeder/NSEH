using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using nseh.Gameplay.Gameflow;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Utils;

namespace nseh.GameManager
{
    public class LevelManager : Service
    {

        //Properties
        //Level game flow
        public enum States { LevelEvent, Minigame };
        public States _currentState;
        private States nextState;
        private bool _isPaused;
        private Canvas _canvasIsPaused = null;
        private Canvas _canvasGameOver = null;
        private Text _Clock = null;
        private Text _gameOverText = null;
        private float timeRemaining;
        private GameObject _player1;
        private GameObject _player2;

        //List of all events (E.g: EventManager, LightManager...) 
        private List<LevelEvent> _eventsList;

        //Adds the specified event to the events list
        void Add<T>() where T : new()
        {
            LevelEvent serviceToAdd = new T() as LevelEvent;
            serviceToAdd.Setup(this);
            _eventsList.Add(serviceToAdd);

        }

        public void PauseGame()
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {

                _canvasIsPaused.gameObject.SetActive(true);
                Time.timeScale = 0;

            }
            else
            {
                _canvasIsPaused.gameObject.SetActive(false);
                Time.timeScale = 1;

            }
        }

        //Finds the specified event in the events list
        public T Find<T>() where T : class
        {
            foreach (LevelEvent thisEvent in _eventsList)
            {
                if (thisEvent.GetType() == typeof(T))
                    return thisEvent as T;
            }
            return null;
        }

        //Events should be initialised here. This method is very similar to MonoBehaviour.Start()
        override public void Setup(GameManager myGame)
        {
            base.Setup(myGame);
            _eventsList = new List<LevelEvent>();
            _currentState = States.LevelEvent;
            _isPaused = false;
            Add<Tar_Event>();


        }

        override public void Activate()
        {
            timeRemaining = Constants.LevelManager.TIME_REMAINING;
            _canvasIsPaused = GameObject.Find("CanvasPaused").GetComponent<Canvas>();
            _Clock = GameObject.Find("CanvasClock/TextClock").GetComponent<Text>();
            _canvasIsPaused.gameObject.SetActive(false);
            _canvasGameOver = GameObject.Find("CanvasGameOver").GetComponent<Canvas>();
            _canvasGameOver.gameObject.SetActive(false);
            IsActivated = true;
            Time.timeScale = 1;
            //Initial event
            Find<Tar_Event>().ActivateEvent();
            switch (GameManager.thisGame.numberPlayers)
            {
                case 1:
                    //_player1 = GameManager.thisGame.Instantiate(GameManager.thisGame.characters[0], new Vector3(0, 1, 2), new Vector3(0, 90, 0));
                    //INTERFAZ
                    break;

                case 2:
                    //_player1 = GameManager.thisGame.Instantiate(GameManager.thisGame.characters[0], new Vector3(-10, 1, 2), new Vector3(0,90,0));
                    //_player2 = GameManager.thisGame.Instantiate(GameManager.thisGame.characters[1], new Vector3(10, 1, 2), new Vector3(0, -90, 0));
                    //INTERFAZ
                    break;
            }
           


        }



        public void ChangeState(States newState)
        {

            nextState = newState;
            //Transitions
            if (nextState != _currentState)
            {
                switch (nextState)
                {
                    case States.LevelEvent:
                        Find<Tar_Event>().ActivateEvent();
                        _currentState = nextState;
                        break;
                    case States.Minigame:
                        Find<Tar_Event>().EventRelease();
                        _currentState = nextState;
                        break;
                }
            }
            //End of transitions
        }

        public void Clock()
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining > 0 && timeRemaining > 10)
            {
                _Clock.text = timeRemaining.ToString("f0");
            }

            else if (timeRemaining > 0 && timeRemaining < 10)
            {
                _Clock.text = timeRemaining.ToString("f2");
            }
            else
            {
                _Clock.text = "";
                Time.timeScale = 0;
                _canvasGameOver.gameObject.SetActive(true);
                _gameOverText = GameObject.Find("CanvasGameOver/PanelGameOver/TextGameOver").GetComponent<Text>();
                _gameOverText.text = "Time's Up";

            }
        }


        public void Restart()
        {
            timeRemaining = Constants.LevelManager.TIME_REMAINING;
            _canvasGameOver.gameObject.SetActive(false);
            _canvasIsPaused.gameObject.SetActive(false);
            Time.timeScale = 1;
            Find<Tar_Event>().ActivateEvent();
        }

        public void GoToMainMenu()
        {
            MyGame.ChangeState(GameManager.States.MainMenu);

        }

        //This is where the different events are triggered in a similar way to a state machine. This method is very similar to MonoBehaviour.Update()
        override public void Tick()
        {
       
                if (timeRemaining > 0)
                {
                    Clock();
                }
                //
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGame();
                }

                if (!_isPaused)
                {
                    //State execution
                    foreach (LevelEvent thisEvent in _eventsList)
                    {
                        if (thisEvent.IsActivated)
                            thisEvent.EventTick();
                    }
                    //End of state execution
                }
            
        }

        override public void Release()
        {
            IsActivated = false;
            _canvasGameOver.gameObject.SetActive(false);
            _canvasIsPaused.gameObject.SetActive(false);
        }
    } 
}
