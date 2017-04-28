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

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(193, 23, 23, 255);
            this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P1";
                this.gameObject.transform.GetChild(0).transform.GetChild(0).transform.position = new Vector3(-0.9f, 2.2f, -0.8f);
            }

            else if (playerText == 2)
            {

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(7, 116, 222, 255);
            this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P2";
            }

            else if (playerText == 3)
            {

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(129, 63, 153, 255);
            this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P3";
            }

            else if (playerText == 4)
            {

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(229, 124, 22, 255);
            this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P4";

            }


         
	    }
    }
