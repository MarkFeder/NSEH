using UnityEngine;
using nseh.Gameplay.Movement;


namespace nseh.Gameplay.Minigames {

    public class CubeDeath : MonoBehaviour {

        #region Public Properties
        public int num;
        #endregion

        #region Private Properties
        private float aux;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start () {
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
                    num -= 50;
                }
                aux = Time.time;
                //other.GetComponent<Minigame>().position = num;
                num += 50;

                //yield return new WaitForSeconds(1);
                Destroy(other.gameObject, 1);
            }
        }

        #endregion

    }
}