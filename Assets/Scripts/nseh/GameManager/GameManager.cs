using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Utils;

namespace nseh.GameManager
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

        //Properties
        public enum States { MainMenu, Playing, Loading };
        private States _currentState;
        public States _nextState;
        public int _numberPlayers = 0;
        public List<GameObject> _characters;
        public string player1character;
        public string player2character;

        //List of all services (E.g: EventManager, LightManager...) 
        private List<Service> _servicesList;

        #region Initialization

        //Managers should be initialised here
        public void Start()
        {
            Add<MenuManager>();
            Add<LevelManager>();
            Add<LoadingScene>();
            Find<MenuManager>().Activate();
        }


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

        #endregion

        //Here is where the different game services are triggered in a similar way to a state machine
        public void Update()
        {
            //State execution
            foreach (Service thisService in _servicesList)
            {
                if (thisService.IsActivated)
                {
                    thisService.Tick();
                }
            }
            //End of state execution
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
        }

        #endregion

        #region Service Management

        //Finds the specified service in the services list
        public T Find<T>() where T : class
        {
            foreach (Service thisService in _servicesList)
            {
                if (thisService.GetType() == typeof(T))
                    return thisService as T;
            }
            return null;
        }

        //Adds the specified service to the services list
        public void Add<T>() where T : new()
        {
            Service serviceToAdd = new T() as Service;
            serviceToAdd.Setup(this);
            _servicesList.Add(serviceToAdd);
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
                            Find<LoadingScene>().Release();
                            Find<MenuManager>().Activate();
                        }
                        else if (_nextState == States.Playing)
                        {
                            _currentState = _nextState;
                            Find<LoadingScene>().Release();
                            Find<LevelManager>().Activate();
                        }
                        break;

                    case States.Playing:
                        _currentState = States.Loading;
                        _nextState = States.MainMenu;
                        Find<LevelManager>().Release();
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
    }
}