using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.Minigames
{

    public class CameraScript : MonoBehaviour
    {
        #region Public Properties
        public bool started = false;
        public float speed;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start()
        {
        }

        // Update is called once per frame
        public void Update()
        {
            if (started == true)
            {
                transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + Time.deltaTime * speed);
            }
        }
        #endregion

    }
}
