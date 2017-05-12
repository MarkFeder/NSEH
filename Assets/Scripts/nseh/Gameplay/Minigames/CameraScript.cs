using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.Minigames
{

    public class CameraScript : MonoBehaviour
    {
        #region Public Properties
        public bool starting;
        public bool started = false;
        public int num;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start()
        {
            starting = true;

        }

        // Update is called once per frame
        public void Update()
        {
            if (started == true)
            {
                starting =false;
                transform.position = new Vector3(transform.position.x + Time.deltaTime * 5, transform.position.y, transform.position.z);
            }

            else if (started == false && starting==true)
            {
                if (num == 1)
                {
                    transform.position = new Vector3(0 , 6, -5);
                }
                else if (num == 2)
                {
                    transform.position = new Vector3(2, 5f, -7);
                }
                else if (num == 3)
                {
                    transform.position = new Vector3(2, 3.22f, -10);
                }
                else if (num == 4)
                {
                    transform.position = new Vector3(2, 1.5f, -12);
                }
            }
        }
        #endregion

    }
}
