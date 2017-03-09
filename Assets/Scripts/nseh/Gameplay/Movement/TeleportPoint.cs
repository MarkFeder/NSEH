using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Gameplay.Movement;

namespace nseh.Gameplay.Movement
{

    public class TeleportPoint : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> TeleportPoints;
        private bool m_isAxisInUse = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {

        }

        private void OnTriggerStay(Collider other)
        {
           
            if ((Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow)) && other.GetComponent<PlayerMovement>().Teletransported == false)
            {
                other.GetComponent<PlayerMovement>().Teletransported = true;
                int randomTeleportPoint = (int)Random.Range(0, TeleportPoints.Count);
                Debug.Log(TeleportPoints[randomTeleportPoint].name);
                other.transform.position = new Vector3(TeleportPoints[randomTeleportPoint].transform.position.x, TeleportPoints[randomTeleportPoint].transform.position.y, other.transform.position.z);    
            }

            
        }

        private void OnTriggerExit(Collider other)
        {
            if (Input.GetAxis("Horizontal") !=0 || other.GetComponent<Rigidbody>().velocity.y>0)
            {
                other.GetComponent<PlayerMovement>().Teletransported = false;

            }
        }
    }
}
