using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Managers.Main;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Minigames
{
    public class LavaDeath : MonoBehaviour
    {

        #region Public Properties

        public int num;
        public MinigameVolcano eventAux;

        #endregion

        #region Private Properties

        private float aux;

        #endregion

        #region Public Methods

        public void Start ()
        {
            num = (4 - GameManager.Instance._numberPlayers) * 50;
        }
	
        #endregion

        #region Private Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Tags.PLAYER_BODY)
            {
                if (Time.time - aux < 0.5)
                {
                    num -= 50;
                }

                aux = Time.time;
                other.GetComponent<MinigameMovement>().started = false;
                other.GetComponent<MinigameMovement>().puntuation = num;
                eventAux.AddPuntuation(num, other.GetComponent<MinigameMovement>().gamepadIndex-1);
                num += 50;
            }
        }

        #endregion

    }
}