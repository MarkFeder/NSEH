using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Managers.Main;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Minigames
{
    public class Goal : MonoBehaviour {

        #region Private Properties

        private float aux;

        #endregion

        #region Public Properties

        public int num;
        public MinigameVolcano eventAux;

        #endregion

        #region Public Methods

        public void Start()
        {
            aux = Time.deltaTime;
            num = GameManager.Instance._numberPlayers * 100;
        }

        #endregion

        #region Private Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Tags.PLAYER_BODY)
            {
                if (Time.time - aux < 0.5)
                {
                    num += 50;
                }
                other.GetComponent<MinigameMovement>().puntuation = num;
                aux = Time.time;
                eventAux.AddPuntuation(num, other.GetComponent<MinigameMovement>().gamepadIndex - 1);
                num -= 50;
            }
        }

        #endregion

    }
}