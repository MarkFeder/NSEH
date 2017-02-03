﻿using nseh.Gameplay.Base.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using nseh.Utils;
using Constants = nseh.Utils.Constants.Animations.Movement;
using SceneObjects = nseh.Utils.Constants.Scenes;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class CharacterMovement : MonoBehaviour, IMovement
    {
        // External References

        private Animator anim;
        private Rigidbody body;
        private CapsuleCollider collider;

        // Properties

        public bool useGamepad = false;
        public int gamepadIndex = 1;
        public float runSpeed = 1.0f;
        public float jumpHeight = 10.0f;
        public float jumpInAirForce = 1.5f;

        protected float horizontal;
        protected float speedDampTime = 0.1f;

        protected bool facingRight = true;
        protected bool isMoving;

        protected Dictionary<string, int> animParameters;

        private float speed;
        private float distToGround;
        private float offsetToGround;
        private float minimumHeight;
        private float radius;
        private int layerMask;


        // C# Properties 

        public bool IsFacingRight
        {
            get
            {
                return this.transform.root.transform.forward.x > 0.0f;
            }
        }

        public int GamepadIndex
        {
            get
            {
                return this.gamepadIndex;

            }
        }

        #region State Properties

        private bool IsIdleState
        {
            get
            {
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.animParameters[Constants.IDLE];
            }
        }

        private bool IsLocomotionState
        {
            get
            {
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.animParameters[Constants.LOCOMOTION];
            }
        }

        private bool IsIdleJumpState
        {
            get
            {
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.animParameters[Constants.IDLE_JUMP];
            }
        }

        private bool IsLocomotionJumpState
        {
            get
            {
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.animParameters[Constants.LOCOMOTION_JUMP];
            }
        }

        #endregion

        protected virtual void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.body = GetComponent<Rigidbody>();
            this.body.isKinematic = false;

            this.animParameters = this.FillInAnimParameters();
            this.distToGround = GetComponent<Collider>().bounds.extents.y;
            this.collider = GetComponent<CapsuleCollider>();
            
            this.offsetToGround = 0.1f;
            this.minimumHeight = 5.0f;
            this.radius = this.collider.radius * 0.9f;
            this.layerMask = LayerMask.GetMask("Platform");
        }

        protected void Start()
        {
            if (this.gamepadIndex == 0)
            {
                Debug.LogError("GamepadIndex is 0 when it should be > 0");
            }
        }

        private Dictionary<string, int> FillInAnimParameters()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            dict.Add(Constants.H, Animator.StringToHash(Constants.H));
            dict.Add(Constants.GROUNDED, Animator.StringToHash(Constants.GROUNDED));
            dict.Add(Constants.SPEED, Animator.StringToHash(Constants.SPEED));
            dict.Add(Constants.LOCOMOTION, Animator.StringToHash(Constants.LOCOMOTION));
            dict.Add(Constants.IDLE, Animator.StringToHash(Constants.IDLE));

            dict.Add(Constants.IDLE_JUMP, Animator.StringToHash(Constants.IDLE_JUMP));
            dict.Add(Constants.LOCOMOTION_JUMP, Animator.StringToHash(Constants.LOCOMOTION_JUMP));

            return dict;
        }

        protected virtual void Update()
        {
            this.horizontal = (this.useGamepad) ? Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, this.gamepadIndex)) : Input.GetAxis(Inputs.AXIS_HORIZONTAL_KEYBOARD);
            this.isMoving = Mathf.Abs(this.horizontal) > 0.1f;

            // Fix to let the character moves on the ground
            // See: http://answers.unity3d.com/questions/468709/no-gravity-with-mecanim.html for more details
            this.anim.applyRootMotion = this.IsGrounded();

            this.anim.SetFloat(this.animParameters[Constants.H], this.horizontal);
            this.anim.SetBool(this.animParameters[Constants.GROUNDED], this.IsGrounded());
        }

        protected virtual void FixedUpdate()
        {
            this.Move();

            this.Jump();
        }

        #region Jump Logic

        public virtual void Jump()
        {
            if (this.body.velocity.y < 10)
            {
                this.StopJumpAnimator();
            }

            if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)) 
                && this.IsGrounded())
            {
                this.StartJumpAnimator();
                this.body.velocity = new Vector3(this.body.velocity.x, this.jumpHeight, this.body.velocity.z);
            }
        }

        private void StopJumpAnimator()
        {
            if (IsIdleState)
            {
                this.anim.SetBool(this.animParameters[Constants.IDLE_JUMP], false);
            }
            else if (IsLocomotionState)
            {
                this.anim.SetBool(this.animParameters[Constants.LOCOMOTION_JUMP], false);
            }
        }

        private void StartJumpAnimator()
        {
            if (IsIdleState)
            {
                this.anim.SetBool(this.animParameters[Constants.IDLE_JUMP], true);
            }
            else if (IsLocomotionState)
            {
                this.anim.SetBool(this.animParameters[Constants.LOCOMOTION_JUMP], true);
            }
        }

        #endregion

        #region Movement Logic

        public virtual void Move()
        {
            this.FlipCharacter(horizontal);

            if (this.IsGrounded())
            {
                if (this.isMoving)
                {
                    this.speed = this.runSpeed;
                    this.anim.SetFloat(this.animParameters[Constants.SPEED], this.speed, this.speedDampTime, Time.deltaTime);
                }
                else
                {
                    this.speed = 0.0f;
                    this.anim.SetFloat(this.animParameters[Constants.SPEED], 0.0f);
                }
            }
            else
            {
                this.ApplyJumpForceInAir();
            }
        }

        private void ApplyJumpForceInAir()
        {
            if (this.IsIdleJumpState)
            {
                this.body.velocity = new Vector3(this.horizontal * this.jumpInAirForce, this.body.velocity.y, this.body.velocity.z);
            }
        }

        public virtual bool IsRaisingUp()
        {
            return this.body.velocity.y > this.minimumHeight;
        }

        public virtual bool IsGrounded()
        {
            return Physics.CheckSphere(this.transform.position, this.radius, this.layerMask);
        }

        #endregion

        #region Flip Logic

        protected virtual void FlipCharacter(float horizontal)
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
            this.facingRight = !this.facingRight;

            var rotation = this.transform.localRotation;
            rotation.y = -rotation.y;
            this.transform.localRotation = rotation;
        }

        #endregion
    }
}
