﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Misc
{
    public class PlayerText : MonoBehaviour
    {

        int playerText;

        void Start()
        {
            playerText = this.transform.parent.gameObject.GetComponent<PlayerInfo>().player;

            if (playerText == 1)
            {

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P1";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P1";
            }

            else if (playerText == 2)
            {

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.25f, 1, 1);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P2";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P2";
                //this.gameObject.GetComponent<TextMesh>().text = "P2";
            }

            else if (playerText == 3)
            {

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 1, 1);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P3";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P3";
                //this.gameObject.GetComponent<TextMesh>().text = "P3";
            }

            else if (playerText == 4)
            {

                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.25f, 0, 1);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P4";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P4";
                //this.gameObject.GetComponent<TextMesh>().text = "P4";
            }

        }  
    }
}
