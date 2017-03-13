using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Inputs = nseh.Utils.Constants.Input;
using nseh.Gameplay.Movement;

namespace nseh.Gameplay.Movement
{

    public class Minigame : MonoBehaviour
    {

        public float speed;
        public float jumpForce;
        public float velocityCube;
        private Rigidbody myRigidBody;

        public int gamepadIndex;

        // Use this for initialization
        void Start()
        {
            myRigidBody = GetComponent<Rigidbody>();
            gamepadIndex = 1;
            velocityCube = 0;
            //gamepadIndex = GetComponent<PlayerInfo>().GamepadIndex;
        }

        // Update is called once per frame
        void Update()
        {
            
                myRigidBody.velocity = new Vector3(speed + velocityCube, myRigidBody.velocity.y, myRigidBody.velocity.z);
            
            
           
            if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)))
            {
                myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, jumpForce, myRigidBody.velocity.z);
            }

        }
    }
}