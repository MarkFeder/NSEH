using nseh.Gameplay.Base.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using nseh.Utils;
using Constants = nseh.Utils.Constants.Animations.Movement;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class CharacterMovement : MonoBehaviour, IMovement
    {
        // External References

        private Animator anim;
        private Rigidbody body;

        // Properties

        [SerializeField]
        protected int gamepadIndex;
        [SerializeField]
        protected float walkSpeed = 0.15f;
        [SerializeField]
        protected float runSpeed = 1.0f;
        [SerializeField]
        protected float jumpHeight = 10.0f;

        protected float horizontal;
        protected float speedDampTime = 0.1f;

        protected bool hasJumped = false;
        protected bool facingRight = true;
        protected bool isMoving;

        protected Dictionary<string, int> animParameters;

        private float speed;
        private float distToGround;
        private float offsetToGround;
        private float minimumHeight;

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

        protected virtual void Awake()
        {
            this.anim = GetComponent<Animator>();
            this.body = GetComponent<Rigidbody>();
            this.body.isKinematic = false;

            this.animParameters = this.FillInAnimParameters();
            this.distToGround = GetComponent<Collider>().bounds.extents.y;
            
            this.offsetToGround = 0.1f;
            this.minimumHeight = 5.0f;
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
            dict.Add(Constants.SPEED, Animator.StringToHash(Constants.SPEED));
            dict.Add(Constants.JUMP, Animator.StringToHash(Constants.JUMP));
            dict.Add(Constants.H, Animator.StringToHash(Constants.H));
            dict.Add(Constants.GROUNDED, Animator.StringToHash(Constants.GROUNDED));

            return dict;
        }

        protected virtual void Update()
        {
            this.horizontal = Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL, this.gamepadIndex));
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

        #region Movement Logic

        public virtual void Jump()
        {
            if (this.body.velocity.y < 10)
            {
                this.anim.SetBool(this.animParameters[Constants.JUMP], false);
            }
            if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)) && this.IsGrounded())
            {
                anim.SetBool(this.animParameters[Constants.JUMP], true);
                this.body.velocity = new Vector3(0, this.jumpHeight, 0);                
            }
        }

        public virtual void Move()
        {
            this.MakeCharacterToFlip(horizontal);

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

        public virtual bool IsRaisingUp()
        {
            return this.body.velocity.y > this.minimumHeight;
        }

        public virtual bool IsGrounded()
        {
            return Physics.Raycast(this.transform.position, -Vector3.up, this.distToGround + this.offsetToGround);
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
