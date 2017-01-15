using UnityEngine;
using System.Collections;
using nseh.Utils.Helpers;

namespace nseh.Gameplay.Entities.Environment.Platforms
{
    public class OneWayPlatform : MonoBehaviour
    {
        private BoxCollider parentCollider;

        void Start()
        {
            this.parentCollider = this.transform.parent.GetComponent<BoxCollider>();   
        }

        void Update() { }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("OnTriggerEnter");

                var body = other.gameObject.GetComponent<Rigidbody>();

                if (body.velocity.y > 0)
                {
                    Debug.Log("y > 0 -> Trigger collider");
                    other.isTrigger = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("OnTriggerExit");
                other.isTrigger = false;
            }
        }
    } 
}
