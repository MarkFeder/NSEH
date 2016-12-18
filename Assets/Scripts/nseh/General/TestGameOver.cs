using UnityEngine;
using System.Collections;
using nseh.GameManager;

namespace nseh.General
{
    public class TestGameOver : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                GetComponent<GameManager_Master>().CallEventTimesUp();
            }
        }
    }
}

