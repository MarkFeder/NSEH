using nseh.Managers;
using nseh.Managers.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nseh.Managers
{
    public class LoadingScene : Service
    {

        #region Private Methods
        private string _aux;
        nseh.Managers.Main.GameManager.States state;
        #endregion

        #region Public Methods
        // Use this for initialization
        public override void Setup(nseh.Managers.Main.GameManager myGame)
        {
            base.Setup(myGame);
        }

        public override void Activate()
        {
            IsActivated = true;
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
           
            if (state == nseh.Managers.Main.GameManager.States.Playing)
            {
                nseh.Managers.Main.GameManager.Instance.ChangeState(nseh.Managers.Main.GameManager.States.Playing);
            }
            else if (state == nseh.Managers.Main.GameManager.States.MainMenu)
            {
                nseh.Managers.Main.GameManager.Instance.ChangeState(nseh.Managers.Main.GameManager.States.MainMenu);
            }

            else if (state == nseh.Managers.Main.GameManager.States.Score)
            {
                nseh.Managers.Main.GameManager.Instance.ChangeState(nseh.Managers.Main.GameManager.States.Score);
            }
            IsActivated = false;
        }
        #endregion

    }
}
