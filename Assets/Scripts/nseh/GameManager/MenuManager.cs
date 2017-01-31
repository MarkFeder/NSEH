using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Service
{
  
    public override void Setup(Game myGame)
    {
        base.Setup(myGame);
    }

    public override void Activate()
    {
        IsActivated = true;
    }

    

    public override void Tick()
    {
        

    }

    public override void Release()
    {
        IsActivated = false;
        //MyGame.nextState = Game.States.Playing;
    }
}
