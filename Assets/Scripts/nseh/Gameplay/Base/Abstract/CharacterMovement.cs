using nseh.Gameplay.Base.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using Constants = nseh.Utils.Constants;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class CharacterMovement : MonoBehaviour, IMovement
    {
        protected float force = 20.0f;

        protected float walkSpeed = 0.15f;
        protected float runSpeed = 1.0f;
        protected float speedDampTime = 0.1f;
        private float speed;

        protected float jumpHeight = 5.0f;
        protected float jumpCooldown = 1.0f;
        protected float timeToNextJump = 0.0f;

        protected bool facingRight = true;

        private Animator anim;
        // This is placeholder right now
        protected Dictionary<string, int> animParameters;

        private Rigidbody body;
    
        private float distToGround;

        protected float horizontal;
        protected bool run;
        protected bool isMoving;

        protected virtual void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.body = GetComponent<Rigidbody>();
            this.body.isKinematic = false;

            this.animParameters = new Dictionary<string, int>();
            this.FillInAnimParameters();
            this.distToGround = GetComponent<Collider>().bounds.extents.y;
        }

        private void FillInAnimParameters()
        {
            if (this.animParameters.Count == 0)
            {
                this.animParameters.Add(Constants.Animations.Movement.SPEED, Animator.StringToHash(Constants.Animations.Movement.SPEED));
                this.animParameters.Add(Constants.Animations.Movement.JUMP, Animator.StringToHash(Constants.Animations.Movement.JUMP));
                this.animParameters.Add(Constants.Animations.Movement.H, Animator.StringToHash(Constants.Animations.Movement.H));
                this.animParameters.Add(Constants.Animations.Movement.GROUNDED, Animator.StringToHash(Constants.Animations.Movement.GROUNDED));
            }
        }

        protected virtual void Update()
        {
            this.horizontal = Input.GetAxis(Constants.Input.AXIS_HORIZONTAL);
            this.run = Input.GetButton(Constants.Input.BUTTON_RUN);

            this.isMoving = Mathf.Abs(this.horizontal) > 0.1f;
        }

        protected virtual void FixedUpdate()
        {
            this.anim.SetFloat(this.animParameters[Constants.Animations.Movement.H], this.horizontal);

            this.anim.SetBool(this.animParameters[Constants.Animations.Movement.GROUNDED], this.IsGrounded());

            this.Move();

            this.Jump();
        }

        protected virtual void Start()
        {
        }

        #region Movement Logic

        public virtual void Jump()
        {
            if (this.body.velocity.y < 10) // already jumped
            {
                this.anim.SetBool(this.animParameters[Constants.Animations.Movement.JUMP], false);
                if (this.timeToNextJump > 0)
                    this.timeToNextJump -= Time.deltaTime;
            }
            if (Input.GetButtonDown(Constants.Animations.Movement.JUMP))
            {
                anim.SetBool(this.animParameters[Constants.Animations.Movement.JUMP], true);
                if (this.speed > 0 && this.timeToNextJump <= 0)
                {
                    this.body.velocity = new Vector3(0, this.jumpHeight, 0);
                    this.timeToNextJump = this.jumpCooldown;
                }
            }
        }

        public virtual void Move()
        {
            this.MovementManagement(this.horizontal, this.run);
        }

        protected virtual bool IsGrounded()
        {
            return Physics.Raycast(this.transform.position, -Vector3.up, this.distToGround + 0.1f);
        }

        protected virtual void MovementManagement(float horizontal, bool running)
        {
            this.MakeCharacterToFlip(horizontal);

            if (this.isMoving)
            {
                if (running)
                {
                    this.speed = this.runSpeed;
                }
                else
                {
                    this.speed = this.walkSpeed;
                }

                this.anim.SetFloat(this.animParameters[Constants.Animations.Movement.SPEED], this.speed, this.speedDampTime, Time.deltaTime);
            }
            else
            {
                this.speed = 0.0f;
                this.anim.SetFloat(this.animParameters[Constants.Animations.Movement.SPEED], 0.0f);
            }

            if (this.facingRight)
            {
                this.body.AddForce(transform.right * speed);
            }
            else
            {
                this.body.AddForce(-transform.right * speed);
            }
        }

        #endregion

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
            this.facingRight = !this.facingRight;
            Vector3 theScale = transform.localScale;

            theScale.z *= -1;
            this.transform.localScale = theScale;
        }

        #endregion
    }
}
