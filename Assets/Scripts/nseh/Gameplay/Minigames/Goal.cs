using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;
using nseh.Managers;


namespace nseh.Gameplay.Minigames
{


    public class Goal : MonoBehaviour {


        public float speed;
        private Rigidbody myRigidBody;
        public bool started = false;
        public int num;
        // Use this for initialization
        void Start()
        {
            myRigidBody = GetComponent<Rigidbody>();
            num = 400;

        }

        // Update is called once per frame
        void Update()
        {
            if (started == true)
            {
                myRigidBody.velocity = new Vector3(speed, myRigidBody.velocity.y, myRigidBody.velocity.z);

            }
            else
            {
                myRigidBody.velocity = Vector3.zero;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerBody")
            {
                StartCoroutine(DestroyCharacter(other));
               
       
            }
        }
        IEnumerator DestroyCharacter(Collider other)
        {
            other.GetComponent<Minigame>().position = num;
            num -= 100;
            yield return new WaitForSeconds(1);
            Destroy(other.gameObject);
             
            

        }

    }
}