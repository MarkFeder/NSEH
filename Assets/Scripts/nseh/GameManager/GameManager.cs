using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Utils;

namespace nseh.GameManager
{
    public class GameManager : MonoBehaviour
    {

        static public GameManager thisGame;

        //Properties
        public enum States { MainMenu, Playing, Loading };
        private States _currentState;
        public States nextState;
        public int numberPlayers = 0;
        public List<GameObject> characters;

        //List of all services (E.g: EventManager, LightManager...) 
        private List<Service> _servicesList;

        //Adds the specified service to the services list
        void Add<T>() where T : new()
        {

            Service serviceToAdd = new T() as Service;
            serviceToAdd.Setup(this);
            _servicesList.Add(serviceToAdd);

        }

       public void AddCharacter(GameObject character)
        {
            characters.Add(character);

        }
        public GameObject Instantiate(GameObject Object, Vector3 pos, Vector3 rot)
        {
            return Instantiate(Object, pos, rot);
        }

        public void RestartList()
        {
            characters = new List<GameObject>();
        }

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

        public void ChangePlayers(int number)
        {
            numberPlayers = number;

        }

        public void ChangeState(States newState)
        {

            nextState = newState;

            if (nextState != _currentState)
            {
                switch (_currentState)
                {
                    case States.MainMenu:
                        _currentState = States.Loading;
                        nextState = States.Playing;
                        Find<MenuManager>().Release();
                        SceneManager.LoadScene(Constants.Scenes.SCENE_01);
                        Find<LoadingScene>().Activate();
                        break;

                    case States.Loading:
                        if (nextState == States.MainMenu)
                        {
                            Time.timeScale = 1;
                            _currentState = nextState;
                            Find<LoadingScene>().Release();
                            Find<MenuManager>().Activate();
                        }
                        else if (nextState == States.Playing)
                        {
                            _currentState = nextState;
                            Find<LoadingScene>().Release();
                            Find<LevelManager>().Activate();
                        }
                        break;

                    case States.Playing:
                        _currentState = States.Loading;
                        nextState = States.MainMenu;
                        Find<LevelManager>().Release();
                        SceneManager.LoadScene(Constants.Scenes.SCENE_MAIN_MENU);
                        Find<LoadingScene>().Activate();
                        break;
                }
            }
        }

        //Managers should be initialised here
        void Start()
        {
            Add<MenuManager>();
            Add<LevelManager>();
            Add<LoadingScene>();
            Find<MenuManager>().Activate();
        }


        void Awake()
        {
            thisGame = this;
            DontDestroyOnLoad(this);
            _servicesList = new List<Service>();
            characters = new List<GameObject>();
            _currentState = States.MainMenu;


        }

        

        //Here is where the different game services are triggered in a similar way to a state machine
        void Update()
        {

            //State execution
            foreach (Service thisService in _servicesList)
            {
                if (thisService.IsActivated)
                    thisService.Tick();
            }
            //End of state execution
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }


}