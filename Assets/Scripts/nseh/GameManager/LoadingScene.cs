using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nseh.GameManager
{

    public class LoadingScene : Service
    {
        string aux;
        GameManager.States state;

        // Use this for initialization
        public override void Setup(GameManager myGame)
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
            Debug.Log("Laoding");
            if (aux != SceneManager.GetActiveScene().name)
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
            IsActivated = false;
        }
    }
}
