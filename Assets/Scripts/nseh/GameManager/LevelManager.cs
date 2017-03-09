using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Gameflow;
using nseh.GameManager.General;
using nseh.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nseh.Gameplay.Movement;

namespace nseh.GameManager
{
    public class LevelManager : Service
    {

        //Properties
        //Level game flow
        public enum States { LevelEvent, Minigame };
        public States _currentState;

        private States _nextState;
        private bool _isPaused;
        private Canvas _canvasIsPaused = null;
        private Canvas _canvasGameOver = null;
        private Text _Clock = null;
        private Text _gameOverText = null;
        private float _timeRemaining;
        private GameObject _player1;
        private GameObject _player2;
        private Image _player1_HUD;
        private Image _player2_HUD;
        private Image _player1_portrait;
        private Image _player2_portrait;
        private bool _isGameOver;

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
            Add<CameraManager>();
            Add<ItemSpawn_Event>();
        }

        override public void Activate()
        {
            _isGameOver = false;
            _timeRemaining = Constants.LevelManager.TIME_REMAINING;
            _canvasIsPaused = GameObject.Find("CanvasPaused").GetComponent<Canvas>();
            _Clock = GameObject.Find("CanvasClock/TextClock").GetComponent<Text>();
            _canvasIsPaused.gameObject.SetActive(false);

            _canvasGameOver = GameObject.Find("CanvasGameOver").GetComponent<Canvas>();
            _canvasGameOver.gameObject.SetActive(false);

            //Encapsular en metodo
            _player1_HUD = GameObject.Find("CanvasPlayersHUD/p1_mark_hud").GetComponent<Image>();
            _player2_HUD = GameObject.Find("CanvasPlayersHUD/p2_mark_hud").GetComponent<Image>();

            _player1_portrait = GameObject.Find("CanvasPlayersHUD/p1_mark_hud/portrait_folio/portrait").GetComponent<Image>();
            _player2_portrait = GameObject.Find("CanvasPlayersHUD/p2_mark_hud/portrait_folio/portrait").GetComponent<Image>();

            IsActivated = true;
            Time.timeScale = 1;

            //Initial event
            Find<Tar_Event>().ActivateEvent();
            Find<ItemSpawn_Event>().ActivateEvent();
            Find<CameraManager>().ActivateEvent();

            Debug.Log("dsasad "+GameManager.Instance._numberPlayers+" "+ (GameManager.Instance._characters[0].name));

            switch (GameManager.Instance._numberPlayers)
            {
                case 1:
                    _player1 = GameManager.Instance.InstantiateCharacter(GameManager.Instance._characters[0], new Vector3(0, 1, 2), new Vector3(0, -90, 0));
                    //INTERFAZ
                    _player1_HUD.gameObject.SetActive(true);
                    Debug.Log(_player1_HUD.transform.GetChild(1).GetComponent<BarComponent>());
                    _player1.GetComponent<CharacterHealth>().HealthBar = _player1_HUD.transform.GetChild(1).GetComponent<BarComponent>();
                    //Encapsular en metodo
                    if (GameManager.Instance.player1character == "Paladin" || GameManager.Instance.player1character == "SirProspector")
                    {
                        Debug.Log("Deberia mostrar a prospector");
                        //_player1_HUD.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("Sprites/sirprospector_portrait") as Sprite;
                        _player1_portrait.sprite = Resources.Load<Sprite>("sirprospector_portrait");
                        Debug.Log(_player1_portrait.sprite);
                    }
                    else if (GameManager.Instance.player1character == "Demon")
                    {
                        Debug.Log("Deberia mostrar a wrarr");
                        //_player1_HUD.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("Sprites/wrarr_portrait") as Sprite;
                        _player1_portrait.sprite = Resources.Load<Sprite>("wrarr_portrait");
                    }
                    _player2_HUD.gameObject.SetActive(false);
                    //CAMBIAR 
                    _player1.GetComponent<PlayerMovement>().GamepadIndex = 1;
                    break;

                case 2:
                    _player1 = GameManager.Instance.InstantiateCharacter(GameManager.Instance._characters[0], new Vector3(12, 1, 2), new Vector3(0, -90, 0));

                    _player1_HUD.gameObject.SetActive(true);
                    _player1.GetComponent<CharacterHealth>().HealthBar = _player1_HUD.transform.GetChild(1).GetComponent<BarComponent>();

                    _player2 = GameManager.Instance.InstantiateCharacter(GameManager.Instance._characters[1], new Vector3(-12, 1, 2), new Vector3(0, 90, 0));

                    _player2_HUD.gameObject.SetActive(true);
                    _player2.GetComponent<CharacterHealth>().HealthBar = _player2_HUD.transform.GetChild(1).GetComponent<BarComponent>();



                    _player1.GetComponent<PlayerMovement>().GamepadIndex = 1;

                    _player2.GetComponent<PlayerMovement>().GamepadIndex = 2;
                    
                    //INTERFAZ
                    //Encapsular en metodo
                    if (GameManager.Instance.player1character == "Paladin" || GameManager.Instance.player1character == "SirProspector")
                    {
                        Debug.Log("Deberia mostrar a prospector");
                        //_player1_HUD.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("Sprites/sirprospector_portrait") as Sprite;
                        _player1_portrait.sprite = Resources.Load<Sprite>("sirprospector_portrait");
                    }
                    else if (GameManager.Instance.player1character == "Demon")
                    {
                        Debug.Log("Deberia mostrar a wrarr");
                        //_player1_HUD.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("Sprites/wrarr_portrait") as Sprite;
                        _player1_portrait.sprite = Resources.Load<Sprite>("wrarr_portrait");
                    }

                    if (GameManager.Instance.player2character == "Paladin" || GameManager.Instance.player2character == "SirProspector")
                    {
                        Debug.Log("Deberia mostrar a prospector");
                        //_player2_HUD.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("Sprites/sirprospector_portrait") as Sprite;
                        _player2_portrait.sprite = Resources.Load<Sprite>("sirprospector_portrait");

                    }
                    else if (GameManager.Instance.player2character == "Demon")
                    {
                        Debug.Log("Deberia mostrar a wrarr");
                        //_player2_HUD.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("Sprites/wrarr_portrait") as Sprite;
                        _player2_portrait.sprite = Resources.Load<Sprite>("wrarr_portrait");
                    }
                    break;
            }
        }



        public void ChangeState(States newState)
        {
            _nextState = newState;
            //Transitions
            if (_nextState != _currentState)
            {
                switch (_nextState)
                {
                    case States.LevelEvent:
                        Find<Tar_Event>().ActivateEvent();
                        Find<CameraManager>().ActivateEvent();
                        Find<ItemSpawn_Event>().ActivateEvent();
                        _currentState = _nextState;
                        break;
                    case States.Minigame:
                        Find<Tar_Event>().EventRelease();
                        Find<CameraManager>().EventRelease();
                        Find<ItemSpawn_Event>().EventRelease();
                        _currentState = _nextState;
                        break;
                }
            }
            //End of transitions
        }

        public void Clock()
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining > 0 && _timeRemaining > 10)
            {
                _Clock.text = _timeRemaining.ToString("f0");
            }

            else if (_timeRemaining > 0 && _timeRemaining < 10)
            {
                _Clock.text = _timeRemaining.ToString("f2");
            }
            else
            {
                _Clock.text = "";
                Time.timeScale = 0;
                _canvasGameOver.gameObject.SetActive(true);
                _gameOverText = GameObject.Find("CanvasGameOver/ImageGameOver/TextGameOver").GetComponent<Text>();
                _gameOverText.text = "Time's Up";
                _isGameOver = true;

            }
        }


        public void Restart()
        {
            _isGameOver = false;
            _isPaused = false;
            _timeRemaining = Constants.LevelManager.TIME_REMAINING;
            _canvasGameOver.gameObject.SetActive(false);
            _canvasIsPaused.gameObject.SetActive(false);

            Time.timeScale = 1;

            Find<Tar_Event>().ActivateEvent();
            //Find<ItemSpawn_Event>().EventRelease();
            Find<ItemSpawn_Event>().ActivateEvent();
            Find<CameraManager>().EventRelease();

            switch (GameManager.Instance._numberPlayers)
            {
                case 1:

                    GameObject.Destroy(_player1);
                    _player1 = GameManager.Instance.InstantiateCharacter(GameManager.Instance._characters[0], new Vector3(0, 1, 2), new Vector3(0, -90, 0));
                    
                    //INTERFAZ
                    _player1_HUD.gameObject.SetActive(true);
                    _player1.GetComponent<CharacterHealth>().HealthBar = _player1_HUD.transform.GetChild(1).GetComponent<BarComponent>();

                    _player2_HUD.gameObject.SetActive(false);
                    
                    //CAMBIAR 
                    _player1.GetComponent<PlayerMovement>().GamepadIndex = 1;
                    break;

                case 2:
                    GameObject.Destroy(_player1);
                    GameObject.Destroy(_player2);

                    _player1 = GameManager.Instance.InstantiateCharacter(GameManager.Instance._characters[0], new Vector3(10, 1, 2), new Vector3(0, -90, 0));

                    _player1_HUD.gameObject.SetActive(true);
                    _player1.GetComponent<CharacterHealth>().HealthBar = _player1_HUD.transform.GetChild(1).GetComponent<BarComponent>();

                    _player2 = GameManager.Instance.InstantiateCharacter(GameManager.Instance._characters[1], new Vector3(-10, 1, 2), new Vector3(0, 90, 0));

                    _player2_HUD.gameObject.SetActive(true);
                    _player2.GetComponent<CharacterHealth>().HealthBar = _player2_HUD.transform.GetChild(1).GetComponent<BarComponent>();


                    _player1.GetComponent<PlayerMovement>().GamepadIndex = 1;
                    _player2.GetComponent<PlayerMovement>().GamepadIndex = 2;
                    
                    //INTERFAZ
                    
                    
                    break;
            }
            Find<CameraManager>().ActivateEvent();
        }

        public void GoToMainMenu()
        {
            _isPaused = false;
            MyGame.ChangeState(GameManager.States.MainMenu);
        }

        public GameObject getPlayer1()
        {
            return _player1;
        }

        public GameObject getPlayer2()
        {
            return _player2;
        }

        //This is where the different events are triggered in a similar way to a state machine. This method is very similar to MonoBehaviour.Update()
        override public void Tick()
        {
       
                if (_timeRemaining > 0)
                {
                    Clock();
                }
                
                //
                if (Input.GetKeyDown(KeyCode.Escape) && !_isGameOver)
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
            Find<ItemSpawn_Event>().EventRelease();
            _canvasGameOver.gameObject.SetActive(false);
            _canvasIsPaused.gameObject.SetActive(false);
        }
    } 
}
