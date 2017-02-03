using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Inputs = nseh.Utils.Constants.Input;
using Constants = nseh.Utils.Constants.Animations.Movement;

namespace nseh.Gameplay.Base.Abstract.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class CharMovement : MonoBehaviour
    {
        [SerializeField]
        float m_MovingTurnSpeed = 360;
        [SerializeField]
        float m_StationaryTurnSpeed = 180;
        [SerializeField]
        float m_JumpPower = 12f;
        [Range(1f, 4f)]
        [SerializeField]
        float m_GravityMultiplier = 2f;
        [SerializeField]
        float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField]
        float m_MoveSpeedMultiplier = 1f;
        [SerializeField]
        float m_AnimSpeedMultiplier = 1f;
        [SerializeField]
        float m_GroundCheckDistance = 0.1f;
        [SerializeField]
        int gamepadIndex = 1;

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        bool m_IsGrounded;
        bool facingRight = true;
        float m_minimumHeight = 5.0f;
        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        float m_TurnAmount;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        bool m_Crouching;

        public bool IsFacingRight
        {
            get
            {
                return this.transform.root.transform.forward.x > 0.0f;
            }
        }

        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;

            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;
        }

        public bool IsRaisingUp()
        {
            return this.m_Rigidbody.velocity.y > this.m_minimumHeight;
        }

        public void Move(float horizontal, Vector3 move, bool crouch, bool jump)
        {
            MakeCharacterToFlip(horizontal);

            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (move.magnitude > 1f) move.Normalize();
            //move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_ForwardAmount = move.z;

            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(crouch, jump);
            }
            else
            {
                HandleAirborneMovement();
            }

            // send input and other state parameters to the animator
            UpdateAnimator(move);
        }

        void UpdateAnimator(Vector3 move)
        {
            // update the animator parameters
            m_Animator.SetFloat(Constants.H, m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat(Constants.SPEED, Mathf.Abs(m_ForwardAmount), 0.1f, Time.deltaTime);
            m_Animator.SetBool(Constants.GROUNDED, m_IsGrounded);

            if (!m_IsGrounded && Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)))
            {
                m_Animator.SetBool(Constants.JUMP, true);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                m_Animator.speed = 1;
            }
        }


        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);

            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
        }


        void HandleGroundedMovement(bool crouch, bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.1f;
            }
        }

        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (m_IsGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }


        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;
                m_Animator.applyRootMotion = true;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                m_Animator.applyRootMotion = false;
            }
        }

        #region Flip Logic

        protected virtual void MakeCharacterToFlip(float horizontal)
        {
            if (horizontal > 0.0f && !facingRight)
            {
                this.Flip();
            }
            else if (horizontal < 0.0f && facingRight)
            {
                this.Flip();
            }
        }

        protected virtual void Flip()
        {
            //this.facingRight = !this.facingRight;
            //Vector3 theScale = transform.localScale;

            //theScale.z *= -1;
            //this.transform.localScale = theScale;

            this.facingRight = !this.facingRight;

            var rotation = this.transform.localRotation;
            rotation.y = -rotation.y;
            this.transform.localRotation = rotation;

        }

        #endregion
    }
}
