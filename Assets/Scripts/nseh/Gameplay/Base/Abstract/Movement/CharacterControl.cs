using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Base.Abstract.Movement
{
    [RequireComponent(typeof(CharMovement))]
    public class CharacterControl : MonoBehaviour
    {
        private CharMovement m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        [SerializeField]
        protected int gamepadIndex;
        [SerializeField]
        protected bool useGamepad;

        private void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<CharMovement>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex));
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = (this.useGamepad) ? Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, this.gamepadIndex)) : Input.GetAxis(Inputs.AXIS_HORIZONTAL_KEYBOARD);

            // we use world-relative directions in the case of no main camera
            m_Move = h * Vector3.forward;

            // pass all parameters to the character control script
            m_Character.Move(h, m_Move, false, m_Jump);
            m_Jump = false;
        }
    }
}
