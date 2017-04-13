﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;


namespace nseh.Gameplay.Minigames {

    public class CubeDeath : MonoBehaviour {


        public float speed;
        private Rigidbody myRigidBody;
        public bool started = false;
        // Use this for initialization
        void Start () {
            myRigidBody = GetComponent<Rigidbody>();
        }
	
	    // Update is called once per frame
	    void Update ()
        {
            if(started == true)
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
            StartCoroutine(DestroyCharacter(other));
            //Destroy(other.gameObject);
        }

        IEnumerator DestroyCharacter(Collider other)
        {
            if (other.tag == "PlayerBody")
            {
                other.GetComponent<Minigame>().velocityCube = -10f;
                yield return new WaitForSeconds(0);
                Destroy(other.gameObject);
            }
            
        }
    }
}