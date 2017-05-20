using nseh.Managers.Main;
using UnityEngine.SceneManagement;

namespace nseh.Managers
{
    public class LoadingScene : Service
    {

        #region Private Methods

        private string _aux;
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
            _aux = SceneManager.GetActiveScene().name;
        }

        public override void Tick()
        {
            if (_aux != SceneManager.GetActiveScene().name)
            {
                Release();
            }

        }

        public override void Release()
        {
           
            if (state == GameManager.States.Playing)
            {
                GameManager.Instance.ChangeState(GameManager.States.Playing);
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
