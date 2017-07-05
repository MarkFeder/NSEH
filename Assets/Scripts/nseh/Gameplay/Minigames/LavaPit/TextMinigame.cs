using UnityEngine;

namespace nseh.Gameplay.Minigames
{
    public class TextMinigame : MonoBehaviour
    {

        #region Public Properties

        public int playerText;

        #endregion

        #region Public Methods

        public void Start()
        {
            if (playerText == 1)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(193, 23, 23, 255);
                this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = "P1";
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

    #endregion

}