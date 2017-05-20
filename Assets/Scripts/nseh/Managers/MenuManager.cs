using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Managers
{
    public class MenuManager : Service
    {
        #region Public Methods
        public override void Setup(Main.GameManager myGame)
        {
            base.Setup(myGame);
        }

        public override void Activate()
        {
            _isActivated = true;
        }

        public void ChangePlayers(int number)
        {
            RestartingCharacters();
            GameManager.Instance.ChangePlayers(number);
        }

        public void SetPlayerChoice(string choice, int player)
        {
            GameManager.Instance.SetPlayersChoice(choice, player);
        }

        public void ChangeStates()
        {
            GameManager.Instance.ChangeState(Main.GameManager.States.Playing);
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
            _isActivated = false;
        }

        #endregion
    }
}
