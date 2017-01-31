using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Service {

    //Properties
    //Level game flow
    public enum States { LevelEvent, Minigame };
    public States _currentState;
    private States nextState;
    private bool _isPaused;
    private Canvas _canvasIsPaused;
    private Canvas _canvasGameOver;
    private Text _Clock;
    private float timeRemaining = 1;
    private bool _firstTick;

    //List of all events (E.g: EventManager, LightManager...) 
    private List<Event> _eventsList;

    //Adds the specified event to the events list
    void Add<T>() where T : new()
    {
        //Debug.Log("Estoy añadiendo :D");
        Event serviceToAdd = new T() as Event;
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
        foreach (Event thisEvent in _eventsList)
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
        _eventsList = new List<Event>();
        _currentState = States.LevelEvent;
        _isPaused = false;
        _firstTick = true;



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
            Text gameOver = GameObject.Find("CanvasGameOver/PanelGameOver/TextGameOver").GetComponent<Text>();
            gameOver.text = "Time's Up";

        }
    }
    //This is where the different events are triggered in a similar way to a state machine. This method is very similar to MonoBehaviour.Update()
    override public void Tick()
    {
        if (_firstTick && SceneManager.GetActiveScene().name == "Game")
        {
            _canvasIsPaused = GameObject.Find("CanvasPaused").GetComponent<Canvas>();
            _Clock = GameObject.Find("CanvasClock/TextClock").GetComponent<Text>();
            _canvasIsPaused.gameObject.SetActive(false);
            _canvasGameOver= GameObject.Find("CanvasGameOver").GetComponent<Canvas>();
            _canvasGameOver.gameObject.SetActive(false);
            _firstTick = false;
        }
        else if (_firstTick && SceneManager.GetActiveScene().name != "Game")
        {
            Debug.Log("Loading");
        }
        else
        {
            Clock();
            //
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }

            if (!_isPaused)
            {
                //State execution
                foreach (Event thisEvent in _eventsList)
                {
                    if (thisEvent.IsActivated)
                        thisEvent.EventTick();
                }
                //End of state execution
                //MyGame.ChangeState(Game.States.MainMenu);
            }
        }
    }

    override public void Release()
    {
        IsActivated = false;
    }
}
