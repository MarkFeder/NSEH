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
        public bool started = false;
        private Rigidbody myRigidBody;
        private Animator animator;
        public bool isGrounded;
        public int position;

        public int gamepadIndex;

        // Use this for initialization
        void Start()
        {
            myRigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            velocityCube = 0;
            //gamepadIndex = GetComponent<PlayerInfo>().GamepadIndex;
        }



        // Update is called once per frame
        void Update()
        {
            if(Math.Round(myRigidBody.velocity.y, 1) == 0)
            {
                isGrounded = true;
            }
            if (started == true)
            {
                animator.SetBool("Start", true);
                myRigidBody.velocity = new Vector3(speed + velocityCube, myRigidBody.velocity.y, myRigidBody.velocity.z);

                if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)) && isGrounded)
                {
                    isGrounded = false;
                    myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, jumpForce, myRigidBody.velocity.z);
                }
            }
            else
            {
                myRigidBody.velocity = Vector3.zero;
            }





        }
    }
}