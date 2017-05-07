using nseh.Managers;
using nseh.Managers.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nseh.Managers
{
    public class LoadingScene : Service
    {
        string aux;
        nseh.Managers.Main.GameManager.States state;

        // Use this for initialization
        public override void Setup(nseh.Managers.Main.GameManager myGame)
        {
            base.Setup(myGame);
        }

        public override void Activate()
        {
            IsActivated = true;
            state = MyGame._nextState;
            aux = SceneManager.GetActiveScene().name;
        }

        public override void Tick()
        {
            if (aux != SceneManager.GetActiveScene().name)
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
                Debug.Log("dsadsa");
                nseh.Managers.Main.GameManager.Instance.ChangeState(nseh.Managers.Main.GameManager.States.Score);
            }
            IsActivated = false;
        }
    }
}
