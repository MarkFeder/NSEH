using nseh.Managers.Main;
using UnityEngine.SceneManagement;

namespace nseh.Managers
{
    public class LoadingScene : Service
    {

        #region Private Methods

        private string _scene;
        GameManager.States state;

        #endregion

        #region Public Methods

        public override void Setup(GameManager myGame)
        {
            base.Setup(myGame);
        }

        public override void Activate()
        {
            _isActivated = true;
            state = MyGame._nextState;
            _scene = SceneManager.GetActiveScene().name;
        }

        public override void Tick()
        {
            if (_scene != SceneManager.GetActiveScene().name)
            {
                Release();
            }
        }

        public override void Release()
        {
            if (state == GameManager.States.Game)
            {
                GameManager.Instance.ChangeState(GameManager.States.Game);
            }

            else if (state == GameManager.States.Minigame)
            {
                GameManager.Instance.ChangeState(GameManager.States.Minigame);
            }

            else if (state == GameManager.States.Boss)
            {
                GameManager.Instance.ChangeState(GameManager.States.Boss);
            }

            else if (state == GameManager.States.MainMenu)
            {
                GameManager.Instance.ChangeState(GameManager.States.MainMenu);
            }

            else if (state == GameManager.States.Score)
            {
                GameManager.Instance.ChangeState(GameManager.States.Score);
            }

            _isActivated = false;
        }

        #endregion

    }
}
