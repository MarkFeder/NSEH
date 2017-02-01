using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Service
{
  
    public override void Setup(GameManager myGame)
    {
        base.Setup(myGame);
    }

    public override void Activate()
    {
        IsActivated = true;
    }

    public void ChangePlayers(int number)
    {
        GameManager.thisGame.ChangePlayers(number);
    }

    public void ChangeStates()
    {
        GameManager.thisGame.ChangeState(GameManager.States.Playing);
    }

    public void ExitGame()
    {
        GameManager.thisGame.ExitGame();
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
