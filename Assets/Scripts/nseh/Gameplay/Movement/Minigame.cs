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
        public int gamepadIndex;
        public float speedVertical;
        public float speedHorizontal;
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
        }


        void Update()
        {
           
            if(started == true)
            {
                Debug.Log(Input.inputString);
                //if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, gamepadIndex)))
                if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, gamepadIndex)))
                {
                    Debug.Log("ss");
                    _myRigidBody.velocity = new Vector3(_myRigidBody.velocity.x, _myRigidBody.velocity.y, speedVertical);
                }

                if (Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, gamepadIndex)) != 0)
                {
                    _myRigidBody.velocity = new Vector3(speedHorizontal * Input.GetAxis("Horizontal"), _myRigidBody.velocity.y, _myRigidBody.velocity.z);

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