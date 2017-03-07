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
            Debug.Log("dsadas"+ other.GetComponent<PlayerMovement>().teletrasported);
            if (Input.GetAxis("Vertical") > 0 && other.GetComponent<PlayerMovement>().teletrasported == false)
            {
                other.GetComponent<PlayerMovement>().teletrasported = true;
                Debug.Log(Input.GetAxis("Vertical"));
                int randomStandardItem = (int)Random.Range(0, TeleportPoints.Count);
                other.transform.position = new Vector3(TeleportPoints[randomStandardItem].transform.position.x, TeleportPoints[randomStandardItem].transform.position.y, other.transform.position.z);
            }

            
        }

        private void OnTriggerExit(Collider other)
        {
            if (Input.GetAxis("Horizontal") !=0 || other.GetComponent<Rigidbody>().velocity.y!=0)
            {
                other.GetComponent<PlayerMovement>().teletrasported = false;
            }
        }
    }
}
