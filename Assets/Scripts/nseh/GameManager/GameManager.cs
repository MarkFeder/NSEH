using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static public GameManager thisGame;

    //Properties
    public enum States { MainMenu, Playing };
    public States _currentState;
    private States nextState;
    private int numberPlayers = 0;

    //List of all services (E.g: EventManager, LightManager...) 
    private List<Service> _servicesList;

    //Adds the specified service to the services list
    void Add<T>() where T : new()
    {

        Service serviceToAdd = new T() as Service;
        serviceToAdd.Setup(this);
        _servicesList.Add(serviceToAdd);

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
                    _currentState = nextState;
                    Find<MenuManager>().Release();
                    SceneManager.LoadSceneAsync("Game");
                    Find<LevelManager>().Activate();
                    
                    break;

                case States.Playing:
                    _currentState = nextState;
                    nextState = States.MainMenu;
                    Find<LevelManager>().Release();
                    Find<MenuManager>().Activate();
                    SceneManager.LoadScene("MainMenu");
                    break;
            }
        }
    }

    //Managers should be initialised here
    void Start()
    {
        Add<MenuManager>();
        Add<LevelManager>();
        Find<MenuManager>().Activate();
    }


    void Awake()
    {
        thisGame = this;
        DontDestroyOnLoad(this);
        _servicesList = new List<Service>();
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

