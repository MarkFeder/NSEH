using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Misc
{
    public class PlayerText : MonoBehaviour
    {
        Quaternion rotation;
        int playerText;

        private void Start()
        {
            playerText = this.transform.parent.gameObject.GetComponent<PlayerInfo>().Player;

            if (playerText == 1)
            {
                rotation = new Quaternion(0, -1, 0, 0);
                this.gameObject.GetComponent<TextMesh>().color = Color.red;
                this.gameObject.GetComponent<TextMesh>().text = "P1";
            }

            else if (playerText == 2)
            {
                rotation = new Quaternion(0, 1, 0, 0);
                this.gameObject.GetComponent<TextMesh>().color = Color.blue;
                this.gameObject.GetComponent<TextMesh>().text = "P2";
            }

            else if (playerText == 3)
            {
                rotation = new Quaternion(0, -1, 0, 0);
                this.gameObject.GetComponent<TextMesh>().color = Color.green;
                this.gameObject.GetComponent<TextMesh>().text = "P3";
            }

            else if (playerText == 4)
            {
                rotation = new Quaternion(0, 1, 0, 0);
                this.gameObject.GetComponent<TextMesh>().color = Color.yellow;
                this.gameObject.GetComponent<TextMesh>().text = "P4";
            }
        }

        private void LateUpdate()
        {
            transform.rotation = rotation;
        }
    }
}
