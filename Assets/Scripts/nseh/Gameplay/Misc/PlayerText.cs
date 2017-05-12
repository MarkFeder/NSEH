using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Misc
{
    public class PlayerText : MonoBehaviour
    {
        #region Private Properties
        private int _playerText;
        #endregion

        #region Public Methods
        public void Start()
        {
            _playerText = this.transform.parent.gameObject.GetComponent<PlayerInfo>().Player;

            if (_playerText == 1)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(193, 23, 23, 255);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P1";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P1";
            }
            
            else if (_playerText == 2)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32 (7, 116, 222, 255);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P2";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P2";
            }

            else if (_playerText == 3)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32 (129, 63, 153, 255);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P3";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P3";
            }

            else if (_playerText == 4)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(229, 124, 22, 255);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P4";
                this.gameObject.transform.GetChild(1).GetComponent<TextMesh>().text = "P4";
            }
        }
        #endregion

    }
}
