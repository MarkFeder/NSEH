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

    public void ChangePlayers(int number)
    {
        Game.thisGame.ChangePlayers(number);
    }

    public void ChangeStates()
    {
        Game.thisGame.ChangeState(Game.States.Playing);
    }

    public void ExitGame()
    {
        Game.thisGame.ExitGame();
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
