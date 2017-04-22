using UnityEngine;

namespace nseh.Managers
{
    public class MenuManager : Service
    {

        public override void Setup(Main.GameManager myGame)
        {
            base.Setup(myGame);
        }

        public override void Activate()
        {
            IsActivated = true;
        }

        public void ChangePlayers(int number)
        {
            RestartingCharacters();
            Main.GameManager.Instance.ChangePlayers(number);

        }

        public void SetPlayerChoice(string choice, int player)
        {
            Main.GameManager.Instance.SetPlayersChoice(choice, player);
        }

        public void ChangeStates()
        {
            Main.GameManager.Instance.ChangeState(Main.GameManager.States.Playing);
        }

        public void ExitGame()
        {
            Main.GameManager.Instance.ExitGame();
        }

        public override void Tick()
        {
        }

        public void RestartingCharacters()
        {
            Main.GameManager.Instance.RestartList();
        }

        public void Adding(GameObject Character)
        {
            Main.GameManager.Instance.AddCharacter(Character);
        }

        public override void Release()
        {
            IsActivated = false;
            //MyGame.nextState = Game.States.Playing;
        }
    } 
}
