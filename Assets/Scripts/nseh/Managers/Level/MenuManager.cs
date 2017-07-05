using nseh.Managers.Main;

namespace nseh.Managers
{
    public class MenuManager : Service
    {

        #region Public Methods

        public override void Setup(GameManager myGame)
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
            MyGame.ChangePlayers(number);
        }

        public void ChangeStates()
        {
            MyGame.ChangeState(GameManager.States.Game);
        }

        public void ExitGame()
        {
            MyGame.ExitGame();
        }

        public override void Tick()
        {
        }

        public void RestartingCharacters()
        {
            MyGame.RestartList();
        }

        public void Adding(string Character)
        {
            MyGame.AddCharacter(Character);
        }

        public override void Release()
        {
            _isActivated = false;
        }

        #endregion

    }
}
