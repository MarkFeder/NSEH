using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.Minigames
{
    public class Camera : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(transform.position.x + Time.deltaTime * 5, transform.position.y, transform.position.z);
        }
    } 
}
