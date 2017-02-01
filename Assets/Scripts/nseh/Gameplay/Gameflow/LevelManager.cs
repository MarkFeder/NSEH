using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Service {

    //Properties
    //Level game flow
    public enum States { LevelEvent, Minigame };
    public States _currentState;
    private States nextState;

    //List of all events (E.g: EventManager, LightManager...) 
    private List<Event> _eventsList;

    //Adds the specified event to the events list
    void Add<T>() where T : new()
    {
        //Debug.Log("Estoy añadiendo :D");
        Event serviceToAdd = new T() as Event;
        serviceToAdd.Setup(MyGame, this);
        _eventsList.Add(serviceToAdd);

    }

    //Finds the specified event in the events list
    public T Find<T>() where T : class
    {
        foreach (Event thisEvent in _eventsList)
        {
            if (thisEvent.GetType() == typeof(T))
                return thisEvent as T;
        }
        return null;
    }

    //Events should be initialised here. This method is very similar to MonoBehaviour.Start()
    override public void Setup(Game myGame)
    {
        base.Setup(myGame);
        _eventsList = new List<Event>();
        _currentState = States.LevelEvent;
    }

    override public void Activate()
    {
        IsActivated = true;
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
                    if (_currentState == States.Minigame)
                    {
                        Find<Tar_Event>().ActivateEvent();
                    }
                    _currentState = nextState;
                    break;
                case States.Minigame:
                    if (_currentState == States.LevelEvent)
                    {
                        Find<Tar_Event>().EventRelease();
                    }
                    _currentState = nextState;
                    break;
            }
        }
        //End of transitions
    }

    //This is where the different events are triggered in a similar way to a state machine. This method is very similar to MonoBehaviour.Update()
    override public void Tick()
    {
        

        //State execution
        foreach (Event thisEvent in _eventsList)
        {
            if (thisEvent.IsActivated)
                thisEvent.EventTick();
        }
        //End of state execution
        MyGame.ChangeState(Game.States.MainMenu);
    }

    override public void Release()
    {
        IsActivated = false;
    }
}
