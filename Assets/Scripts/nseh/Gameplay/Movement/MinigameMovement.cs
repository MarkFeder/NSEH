using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Inputs = nseh.Utils.Constants.Input;

public class MinigameMovement : MonoBehaviour {
    public float speedVertical;

    public float speedHorizontal;
    public int gamepadIndex;
    public bool started;
    private Rigidbody _body;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        started = false;
    }

    void Update()
    {
        if (started)
        {
            if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, gamepadIndex)))
            {
                //_body.AddForce(transform.forward * speedVertical);
                _body.velocity = new Vector3(_body.velocity.x, _body.velocity.y, speedVertical);
            }

            if (Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, gamepadIndex)) != 0)
            {
                _body.velocity = new Vector3(speedHorizontal * Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, gamepadIndex)), _body.velocity.y, _body.velocity.z);

            }
        }
        
    }
}