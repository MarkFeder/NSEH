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
        #region Public Properties
        public bool started = false;
        public bool isGrounded;
        public int position;
        public int gamepadIndex;
        public float speed;
        public float jumpForce;
        public float velocityCube;
        public AudioClip audio;
        #endregion

        #region Private Properties
        private Rigidbody _myRigidBody;
        private Animator _animator;
        #endregion

        #region Public Properties
        // Use this for initialization
        public void Start()
        {
            _myRigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            velocityCube = 0;
            //gamepadIndex = GetComponent<PlayerInfo>().GamepadIndex;
        }

        // Update is called once per frame
        public void Update()
        {
            if(Math.Round(_myRigidBody.velocity.y, 1) == 0)
            {
                isGrounded = true;
            }
            if (started == true)
            {
                _animator.SetBool("Start", true);
                _myRigidBody.velocity = new Vector3(speed + velocityCube, _myRigidBody.velocity.y, _myRigidBody.velocity.z);

                if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)) && isGrounded)
                {
                    isGrounded = false;
                    _myRigidBody.velocity = new Vector3(_myRigidBody.velocity.x, jumpForce, _myRigidBody.velocity.z);
                    AudioSource.PlayClipAtPoint(audio, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0.5f);
                }
            }
            else
            {
                _myRigidBody.velocity = Vector3.zero;
            }
        }

        
        #endregion
    }
}