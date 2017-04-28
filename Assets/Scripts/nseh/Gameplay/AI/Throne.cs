using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.AI
{

    public class Throne : MonoBehaviour {

        public int players_throne;

        // Use this for initialization
        void Start() {
            players_throne = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            players_throne++;
        }


        private void OnTriggerExit(Collider other)
        {
            players_throne--;
        }



        // Update is called once per frame
        void Update() {

        }
    } 
}
