using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {

    public GameManager MyGame;
    public LevelManager LvlManager;
    public bool IsActivated;

    //Setup the event providing the current game instance. The event is not active here yet.
    virtual public void Setup(GameManager myGame, LevelManager lvlManager)
    {
        MyGame = myGame;
        LvlManager = lvlManager;
    }

    //Activate the event execution.
    abstract public void ActivateEvent();

    //Event execution.
    abstract public void EventTick();

    //Deactivates the event
    abstract public void EventRelease();
}
