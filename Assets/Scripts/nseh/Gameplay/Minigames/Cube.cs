using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;

namespace nseh.Gameplay.Minigames
{
  
    public class Cube : MonoBehaviour
    {
        #region Private Methods
        private void OnTriggerStay(Collider other)
        {
            if(other.tag=="PlayerBody")
                other.GetComponent<Minigame>().velocityCube = -3;
      
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "PlayerBody")
                other.GetComponent<Minigame>().velocityCube = 0;
        }
        #endregion

    }
}

