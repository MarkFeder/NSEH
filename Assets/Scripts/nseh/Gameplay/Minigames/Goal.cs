using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Managers.Main;
using nseh.Managers.Level;

namespace nseh.Gameplay.Minigames
{

    public class Goal : MonoBehaviour {

        #region Private Properties
        private float aux;
        #endregion

        #region Public Properties
        public int num;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start()
        {
            aux = Time.deltaTime;
            num = 400;
        }

        // Update is called once per frame
        #endregion

        #region Private Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerBody")
            {
                if (Time.time - aux < 0.5)
                {
                    num += 100;
                }
                other.GetComponent<MinigameMovement>().puntuation = num;
                aux = Time.time;
                GameManager.Instance.Find<LevelManager>().Find<MinigameEvent>().AddPuntuation(num, other.GetComponent<MinigameMovement>().gamepadIndex - 1);
                num -= 100;
            }
        

        }

        #endregion
    }
}