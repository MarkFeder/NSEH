using nseh.Gameplay.Base.Interfaces;
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
    public abstract class CharacterMovement : MonoBehaviour, IMovement
    {
        // External References

        private Animator anim;
        private Rigidbody body;
        private CapsuleCollider collider;

        // Properties

        [SerializeField]
        public bool useGamepad = false;

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
        protected bool isJumping;

        protected Dictionary<string, int> animParameters;

        private float speed;
        private float distToGround;
        private float offsetToGround;
        private float minimumHeight;
        private float radius;
        private int layerMask;

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
            dict.Add(Constants.SPEED, Animator.StringToHash(Constants.SPEED));
            dict.Add(Constants.JUMP, Animator.StringToHash(Constants.JUMP));
            dict.Add(Constants.H, Animator.StringToHash(Constants.H));
            dict.Add(Constants.GROUNDED, Animator.StringToHash(Constants.GROUNDED));

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

        #region Movement Logic

        public virtual void Jump()
        {
            if (this.body.velocity.y < 10)
            {
                this.anim.SetBool(this.animParameters[Constants.JUMP], false);
            }

            if (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)) && this.IsGrounded())
            {
                this.anim.SetBool(this.animParameters[Constants.JUMP], true);
                this.body.velocity = new Vector3(this.body.velocity.x, this.jumpHeight, this.body.velocity.z);
            }
        }

        public virtual void Move()
        {
            this.MakeCharacterToFlip(horizontal);

            if (this.isMoving)
            {
                this.speed = this.runSpeed;
                this.anim.SetFloat(this.animParameters[Constants.SPEED], this.speed, this.speedDampTime, Time.deltaTime);

                this.body.velocity = this.transform.forward * this.speed * Time.deltaTime;
            }
            else
            {
                this.speed = 0.0f;
                this.anim.SetFloat(this.animParameters[Constants.SPEED], 0.0f);
                this.body.velocity = Vector3.zero;
            }
        }

        public virtual bool IsRaisingUp()
        {
            return this.body.velocity.y > this.minimumHeight;
        }

        public virtual bool IsGrounded()
        {
            return Physics.CheckSphere(this.transform.position, this.radius, this.layerMask);
            //return Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down);


            //RaycastHit hit; 
            //if (Physics.Raycast(this.transform.position, -Vector3.up, out hit, this.distToGround + this.offsetToGround))
            //{
            //    return hit.transform.CompareTag(SceneObjects.PLATFORM);
            //}

            //return false;
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
