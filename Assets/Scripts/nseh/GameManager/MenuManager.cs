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
            GameManager.Instance.ChangePlayers(number);
        }

        public void SetPlayerChoice(string choice, int player)
        {
            GameManager.Instance.SetPlayersChoice(choice, player);
        }

        public void ChangeStates()
        {
            GameManager.Instance.ChangeState(GameManager.States.Playing);
        }

        public void ExitGame()
        {
            GameManager.Instance.ExitGame();
        }

        public override void Tick()
        {


        }

        public void RestartingCharacters()
        {
            GameManager.Instance.RestartList();
        }

        public void Adding(GameObject Character)
        {
            GameManager.Instance.AddCharacter(Character);
        }

        public override void Release()
        {
            IsActivated = false;
            //MyGame.nextState = Game.States.Playing;
        }
    } 
}
