using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.AI
{

    public class Throne : MonoBehaviour {

        #region Public Properties
        public int players_throne;
        #endregion

        #region Public Methods
        public void Start() {
            players_throne = 0;
        }
        #endregion

        #region Private Methods
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
                players_throne++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
                players_throne--;
        }
        #endregion

    }
}
