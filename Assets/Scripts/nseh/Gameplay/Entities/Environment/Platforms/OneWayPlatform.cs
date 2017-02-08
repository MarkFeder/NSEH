using UnityEngine;
using System.Collections;
using nseh.Utils.Helpers;
using nseh.Gameplay.Base.Abstract;
using Tags = nseh.Utils.Constants.Tags;

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
            if (other.CompareTag(Tags.PLAYER) && !other.isTrigger)
            {
                Debug.Log("OnTriggerEnter");

                var body = other.gameObject.GetComponent<Rigidbody>();
                var movement = other.gameObject.GetSafeComponent<CharacterMovement>();

                if (movement != null && !movement.IsGrounded())
                {
                    Debug.Log("y > 0 -> Trigger collider");
                    other.isTrigger = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.PLAYER) && other.isTrigger)
            {
                Debug.Log("OnTriggerExit");
                other.isTrigger = false;
            }
        }
    } 
}
