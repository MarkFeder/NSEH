using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.Minigames
{
    public class DestroyPlatforms : MonoBehaviour
    {

        #region Public Properties
        public GameObject destructionPoint;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start()
        {
            if (this.gameObject.name == "Cube 6(Clone)")
            {
                destructionPoint = GameObject.Find("PlatformDestructionPointWall");
            }

            else if (this.gameObject.name == "InitialPlatforms")
            {
                destructionPoint = GameObject.Find("PlatformDestructionPointWall");
            }
 
            else
            {
                destructionPoint = GameObject.Find("PlatformDestructionPoint");
            }
        }

        // Update is called once per frame
        public void Update()
        {
            if (transform.position.x < destructionPoint.transform.position.x)
            {
                Destroy(gameObject);
            }
        }   
        #endregion

    }
}
