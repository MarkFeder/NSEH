using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nseh.Gameplay.Movement;

public class Cube : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="PlayerBody")
            other.GetComponent<Minigame>().velocityCube = -2;
      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerBody")
            other.GetComponent<Minigame>().velocityCube = 0;
    }

}
