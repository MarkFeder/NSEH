using nseh.Managers.General;
using nseh.Managers.Level;
using nseh.Utils;
using System.Collections;
using System.Collections.Generic;
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

        #region Public Properties

        public enum States { MainMenu, Playing, Loading , Score};
        public States _nextState;

        public int _numberPlayers = 0;
        public List<GameObject> _characters;
        public int [,] _score;
        public string player1character;
        public string player2character;

        #endregion

        #region Private Properties

        private States _currentState;
        private LevelManager _levelManager;

        //List of all services (E.g: EventManager, LightManager...) 
        private List<Service> _servicesList;

        #endregion

        #region Cached Managers

        public LevelManager LevelManager
        {
            get { return _levelManager; }
        }

        #endregion

        #region Initialization

        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);

                // Initiate other properties here
                _servicesList = new List<Service>();
                _characters = new List<GameObject>();
                _currentState = States.MainMenu;
            }
        }

        public void Start()
        {
            // Add managers to the list
            Add<MenuManager>();
            Add<LevelManager>();
            Add<LoadingScene>();

            // Cache some managers
            _levelManager = Find<LevelManager>();

            // Find managers and activate them
            Find<MenuManager>().Activate();

           
        }

        #endregion

        /// <summary>
        /// Here is where the different game services are triggered in 
        /// a similar way to a state machine.
        /// </summary>
        public void Update()
        {
            foreach (Service thisService in _servicesList)
            {
                if (thisService.IsActivated)
                {
                    thisService.Tick();
                }
            }
        }

        #region Characters Management

        public void AddCharacter(GameObject character)
        {
            _characters.Add(character);
        }

        public void SetPlayersChoice(string choice, int player)
        {
            switch (player)
            {
                case 1:
                    player1character = choice;
                    break;
                case 2:
                    player2character = choice;
                    break;
            }
        }

        public GameObject InstantiateCharacter(GameObject Object,Vector3 pos, Vector3 rot)
        {
            return Instantiate(Object, pos, Quaternion.Euler(rot));
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

        /// <summary>
        /// Finds the specified service in the services list.
        /// </summary>
        /// <typeparam name="T">The manager itself of type T</typeparam>
        /// <returns></returns>
        public T Find<T>() where T : class
        {
            foreach (Service thisService in _servicesList)
            {
                if (thisService.GetType() == typeof(T))
                    return thisService as T;
            }
            return null;
        }

        /// <summary>
        /// Adds the specified service to the services list.
        /// </summary>
        /// <typeparam name="T">The manager of type T to be added</typeparam>
        public void Add<T>() where T : new()
        {
            Service serviceToAdd = new T() as Service;
            serviceToAdd.Setup(this);
            _servicesList.Add(serviceToAdd);
        }

        /// <summary>
        /// Setup the specified service from the services list.
        /// </summary>
        /// <typeparam name="T">The type of the service to be set up</typeparam>
        public void SetupService<T>() where T : class
        {
            var enumerator = _servicesList.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Service serv = enumerator.Current;

                if (serv.GetType() == typeof(T))
                {
                    serv.Setup(this);
                }
            }
        }

        /// <summary>
        /// Setup all services from the services list.
        /// </summary>
        public void SetupAllServices()
        {
            var enumerator = _servicesList.GetEnumerator();

            while(enumerator.MoveNext())
            {
                enumerator.Current.Setup(this);
            }
        }

        #endregion

        #region Components Management

        public static MainMenuComponent CreateComponent(GameObject where, int parameter)
        {
            MainMenuComponent myC = where.AddComponent<MainMenuComponent>();

            return myC;
        }

        #endregion

        #region State Management

        public void ChangeState(States newState)
        {
            _nextState = newState;
            if (_nextState != _currentState)
            {
                switch (_currentState)
                {
                    case States.MainMenu:
                        _currentState = States.Loading;
                        _nextState = States.Playing;
                        Find<MenuManager>().Release();
                        SceneManager.LoadScene(Constants.Scenes.SCENE_01);
                        Find<LoadingScene>().Activate();

                        break;

                    case States.Loading:
                        if (_nextState == States.MainMenu)
                        {
                            Time.timeScale = 1;
                            _currentState = _nextState;
                            Find<MenuManager>().Activate();
                        }
                        else if (_nextState == States.Playing)
                        {
                            _currentState = _nextState;
                            //DEBUG SOMEDAY
                            // Find<LevelManager>().Setup(this);
                            Find<LevelManager>().Activate();

                        }

                        else if (_nextState == States.Score)
                        {
                            Time.timeScale = 1;
                            _currentState = _nextState;
                                           
                        }
                        break;

                    case States.Playing:
                        //_currentState = States.Loading;
                        //_nextState = States.MainMenu;
                        if(_nextState== States.MainMenu)
                        {
                            _currentState = States.Loading;
                            //_nextState = States.MainMenu;
                            Find<LevelManager>().Release();
                            SceneManager.LoadScene(Constants.Scenes.SCENE_MAIN_MENU);
                            Find<LoadingScene>().Activate();
                        }
                        else if (_nextState == States.Score)
                        {
                            _currentState = States.Loading;
                            Find<LevelManager>().Release();
                            SceneManager.LoadScene("Score");
                            Find<LoadingScene>().Activate();
                        }

                        break;

                    case States.Score:

                        _currentState = States.Loading;
                        //_nextState = States.MainMenu;
                        Debug.Log("scs");
                        SceneManager.LoadScene(Constants.Scenes.SCENE_MAIN_MENU);
                        Find<LoadingScene>().Activate();

                        break;
                }
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        #endregion

        #region Utils Methods

        /// <summary>
        /// Function for use in the States that have no access to Unity functions. 
        /// Call an IEnumerator through this GameObject.
        /// </summary>
        /// <param name="_coroutine">IEnumerator object.</param>
        public void StartChildCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        /// <summary>
        /// Function for use in the States that have no access to Unity functions. 
        /// Call an IEnumerator through this GameObject.
        /// </summary>
        /// <param name="_coroutine">IEnumerator object.</param>
        public void StopChildCoroutine(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }

        /// <summary>
        /// Function for use in the States that have no access to Unity functions. 
        /// Call an IEnumerator through this GameObject.
        /// </summary>
        /// <param name="_coroutine">The name of the method.</param>
        public void StopChildCoroutine(string methodName)
        {
            StopCoroutine(methodName);
        }

        #endregion
    }
}