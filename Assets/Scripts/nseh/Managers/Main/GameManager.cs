using System.Collections.Generic;
using nseh.Managers.Audio;
using nseh.Managers.Level;
using nseh.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nseh.Managers.Main
{
    public class GameManager : MonoBehaviour
    {

        #region Singleton Pattern

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                return null;
            }
        }

        #endregion

        #region Private Properties

        private States _currentState;

		private List<Service> _servicesList;
        private GameEvent _gameEvent;
        private BossEvent _bossEvent;
        private MinigameEvent _minigameEvent;
        private SoundManager _soundManager;

        #endregion

        #region Cached Managers

        public GameEvent GameEvent
        {
            get
            {
                return _gameEvent;
            }
        }

        public BossEvent BossEvent
        {
            get
            {
                return _bossEvent;
            }
        }

        public MinigameEvent MinigameEvent
        {
            get
            {
                return _minigameEvent;
            }
        }


        public SoundManager SoundManager
        {
            get
            {
                return _soundManager;
            }
        }

        #endregion

        #region Public Properties

        public enum States { MainMenu, Game, Minigame, Boss, Loading , Score};
        public States _nextState;

        public int _numberPlayers = 0;
        public List<string> _characters;
        public int [,] _score;
        public bool isPaused;

        #endregion

        #region Initialization

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
                //Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.Locked;

                _servicesList = new List<Service>();
                _characters = new List<string>();
                _currentState = States.MainMenu;

                Add<SoundManager>();
				Add<MenuManager>();
				Add<GameEvent>();
                Add<MinigameEvent>();
                Add<BossEvent>();
                Add<LoadingScene>();

                _gameEvent = Find<GameEvent>();
                _minigameEvent = Find<MinigameEvent>();
                _bossEvent = Find<BossEvent>();
                _soundManager = Find<SoundManager>();

                _soundManager.Activate();
				Find<MenuManager>().Activate();
                isPaused = false;
            }
        }

        #endregion

        #region Public Methods

        public void Update()
        {
            if (!isPaused)
            {
                foreach (Service thisService in _servicesList)
                {
                    if (thisService.IsActivated)
                    {
                        thisService.Tick();
                    }
                }
            } 
        }

        public void TogglePause()
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0;
            }

            else
            {
                Time.timeScale = 1;
            }
        }

        public void TogglePause(GameObject canvas)
        {
            isPaused = !isPaused;

            if (isPaused)
            {    
                Time.timeScale = 0;
                canvas.SetActive(true);
            }

            else
            {   
                Time.timeScale = 1;
                canvas.SetActive(false);
            }
        }

        #endregion

        #region Characters Management

        public void AddCharacter(string character)
        {
            _characters.Add(character);
        }

        public void RestartList()
        {
            _characters.Clear();
        }

        public void ChangePlayers(int number)
        {
            _numberPlayers = number;
            _score = new int[number, 3];
        }

        #endregion

        #region Service Management

        public T Find<T>() where T : class
        {
            foreach (Service thisService in _servicesList)
            {
                if (thisService.GetType() == typeof(T))
                    return thisService as T;
            }

            return null;
        }

        public void Add<T>() where T : new()
        {
            Service serviceToAdd = new T() as Service;
            serviceToAdd.Setup(this);
            _servicesList.Add(serviceToAdd);
        }

        #endregion

        #region State Management

        public void ChangeState(States newState)
        {
            _nextState = newState;
            switch (_currentState)
            {
                case States.MainMenu:
                    _currentState = States.Loading;
                    _nextState = States.Game;
                    Find<MenuManager>().Release();
                    SceneManager.LoadScene(1);
                    Find<LoadingScene>().Activate();

                    break;

                case States.Loading:
                        
                    if (_nextState == States.MainMenu)
                    {
                        Time.timeScale = 1;
                        _currentState = _nextState;
                        Find<MenuManager>().Activate();
                    }

                    else if (_nextState == States.Game)
                    {
                        _currentState = _nextState;
                        GameEvent.Activate();
                    }

                    else if (_nextState == States.Minigame)
                    {
                        _currentState = _nextState;
                        MinigameEvent.Activate();
                    }

                    else if (_nextState == States.Boss)
                    {
                        _currentState = _nextState;
                        BossEvent.Activate();
                    }

                    else if (_nextState == States.Score)
                    {
                        Time.timeScale = 1;
                        _currentState = _nextState;
                        Find<MenuManager>().Activate();
                    }

                    break;

                case States.Game:

                    if(_nextState== States.MainMenu)
                    {
                        _currentState = States.Loading;
                        GameEvent.Release();
                        SceneManager.LoadScene(0);
                        Find<LoadingScene>().Activate();
                    }

                    else if (_nextState == States.Minigame)
                    {
                        _currentState = States.Loading;
                        SceneManager.LoadScene(2);
                        Find<LoadingScene>().Activate();
                    }

                    break;

                case States.Minigame:

                    if (_nextState == States.MainMenu)
                    {
                        _currentState = States.Loading;
                        MinigameEvent.Release();
                        SceneManager.LoadScene(0);
                        Find<LoadingScene>().Activate();
                    }

                    else if (_nextState == States.Game)
                    {
                        _currentState = States.Loading;
                        MinigameEvent.Release();
                        SceneManager.LoadScene(1);
                        Find<LoadingScene>().Activate();
                    }

                    else if (_nextState == States.Boss)
                    {
                        _currentState = States.Loading;
                        SceneManager.LoadScene(3);
                        Find<LoadingScene>().Activate();
                    }

                    break;

                case States.Boss:

                    if (_nextState == States.MainMenu)
                    {
                        BossEvent.Release();
                        _currentState = States.Loading;
                        SceneManager.LoadScene(0);
                        Find<LoadingScene>().Activate();
                        
                    }

                    else if (_nextState == States.Game)
                    {
                        _currentState = States.Loading;
                        BossEvent.Release();
                        SceneManager.LoadScene(1);
                        Find<LoadingScene>().Activate();
                    }

                    else if (_nextState == States.Score)
                    {
                        _currentState = States.Loading;
                        SceneManager.LoadScene(4);
                        Find<LoadingScene>().Activate();
                    }

                    break;

                case States.Score:
                    _currentState = States.Loading;
                    Find<MenuManager>().Release();
                    SceneManager.LoadScene(0);
                    Find<LoadingScene>().Activate();

                    break;               
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        #endregion

    }
}