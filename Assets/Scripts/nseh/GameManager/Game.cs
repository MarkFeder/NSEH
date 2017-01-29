using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    static public Game thisGame;

    //Properties
    public enum States { MainMenu, Playing };
    private States _currentState;
    public States nextState;

    //List of all services (E.g: EventManager, LightManager...) 
    private List<Service> _servicesList;

    //Adds the specified service to the services list
    void Add<T>() where T : new()
    {
        Debug.Log("Estoy añadiendo :D");
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

    //Managers should be initialised here
    void Start()
    {
        //Add<MenuManager>();
        //Add<Lights>();
        Add<LevelManager>();
        //Find<MenuManager>().Activate();
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
        //Transitions
        if (nextState != _currentState)
        {
            switch (nextState)
            {
                case States.MainMenu:
                    if (_currentState == States.Playing)
                    {
                        Find<LevelManager>().Release();
                        //Find<MenuManager>().Activate();
                    }
                    _currentState = nextState;
                    break;
                case States.Playing:
                    if (_currentState == States.MainMenu)
                    {
                        //Find<MenuManager>().Release();
                        Find<LevelManager>().Activate();
                    }
                    _currentState = nextState;
                    break;
            }
        }
        //End of transitions

        //State execution
        foreach (Service thisService in _servicesList)
        {
            if (thisService.IsActivated)
                thisService.Tick();
        }
        //End of state execution
    }
}

