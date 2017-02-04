using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.GameManager
{
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

        public void RestartingCharacters()
        {
            GameManager.thisGame.RestartList();
        }

        public void Adding(GameObject Character)
        {
            GameManager.thisGame.AddCharacter(Character);
        }

        public override void Release()
        {
            IsActivated = false;
            //MyGame.nextState = Game.States.Playing;
        }
    } 
}
