using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Service
{
    public Game MyGame;
    public bool IsActivated;
    
    //Setup the Service providing the current game instance. The Service is not active here yet.
    virtual public void Setup(Game myGame)
    {
        MyGame = myGame;
    }

    //Activate the Service execution.
    abstract public void Activate();

    //Service execution.
    abstract public void Tick();

    //Deactivates the Service
    abstract public void Release();
}
