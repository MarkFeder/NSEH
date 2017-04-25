using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.Minigames
{
    public class DestroyPlatforms : MonoBehaviour
    {

        public GameObject destructionPoint;

        // Use this for initialization
        void Start()
        {
            if (this.gameObject.name == "Cube 6(Clone)")
            {
                destructionPoint = GameObject.Find("PlatformDestructionPointWall");
            }else if (this.gameObject.name == "InitialPlatforms")
            {
                destructionPoint = GameObject.Find("PlatformDestructionPointWall");
            }

          
            else
            {
                destructionPoint = GameObject.Find("PlatformDestructionPoint");
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.x < destructionPoint.transform.position.x)
            {
                Destroy(gameObject);
            }
        }
    } 
}
