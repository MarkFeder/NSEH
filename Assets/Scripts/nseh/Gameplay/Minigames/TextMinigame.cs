using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;
        public class TextMinigame : MonoBehaviour {


        public int playerText;
	    // Use this for initialization
	    void Start ()
        {
            if (playerText == 1)
            {
                this.gameObject.GetComponent<TextMesh>().color = Color.red;
                this.gameObject.GetComponent<TextMesh>().text = "P1";
            }

            else if (playerText == 2)
            {
                this.gameObject.GetComponent<TextMesh>().color = Color.blue;
                this.gameObject.GetComponent<TextMesh>().text = "P2";
            }

            else if (playerText == 3)
            {
                this.gameObject.GetComponent<TextMesh>().color = Color.green;
                this.gameObject.GetComponent<TextMesh>().text = "P3";
            }

            else if (playerText == 4)
            {
                this.gameObject.GetComponent<TextMesh>().color = Color.yellow;
                this.gameObject.GetComponent<TextMesh>().text = "P4";
            }

        }
    
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
