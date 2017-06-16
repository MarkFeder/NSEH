using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Managers.Main;
using nseh.Managers.Level;

namespace nseh.Gameplay.Minigames {

    public class CubeDeath : MonoBehaviour {

        #region Public Properties
        public int num;
        #endregion

        #region Private Properties
        private float aux;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start ()

        {
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
                    num -= 50;
                }
                aux = Time.time;
                other.GetComponent<MinigameMovement>().started = false;
                other.GetComponent<MinigameMovement>().puntuation = num;
                Debug.Log(other.GetComponent<MinigameMovement>().gamepadIndex);
                GameManager.Instance.Find<LevelManager>().Find<MinigameEvent>().AddPuntuation(num, other.GetComponent<MinigameMovement>().gamepadIndex-1);
                num += 50;
            }
        }

        #endregion

    }
}