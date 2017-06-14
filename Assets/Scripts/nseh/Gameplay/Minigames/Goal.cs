using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;
using nseh.Managers;

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
                if (Time.time - aux < 0.01)
                {
                    num += 100;
                }
                aux = Time.time;
                num -= 100;
                //yield return new WaitForSeconds(1);
                Destroy(other.gameObject, 1);
            }
        

        }

        #endregion
    }
}