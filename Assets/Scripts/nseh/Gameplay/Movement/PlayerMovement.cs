using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants = nseh.Utils.Constants.Animations.Movement;
using Inputs = nseh.Utils.Constants.Input;
using Layers = nseh.Utils.Constants.Layers;

namespace nseh.Gameplay.Movement
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovement : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody body;
        private Dictionary<string, int> animParameters;
        private CapsuleCollider collider;

        private int platformMask;

        private bool facingRight;
        private bool isMoving;
        private bool isJumping;
        private bool currentIdleJump = false;
        private bool currentLocoJump = false;

        private float horizontal;
        private float vertical;
        private float radius;

        public bool useGamepad = false;
        public int gamepadIndex = 1;
        public float speed;
        public float jumpHeight;
        public float gravity;
        [Range(0, 1)]
        public float dampAir;

        // C# Properties 

        #region Public Properties

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

            set
            {
                if (value != 0)
                {
                    this.gamepadIndex = value;
                }
            }
        }

        public float Speed
        {
            get
            {
                return this.speed;
            }

            set
            {
                this.speed = value;
            }
        }

        public bool IsFallingDown
        {
            get
            {
                return this.body.velocity.y < 0.0f;
            }
        }

        #endregion

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

        private void Start()
        {
            this.anim = GetComponent<Animator>();
            this.body = GetComponent<Rigidbody>();
            this.collider = GetComponent<CapsuleCollider>();
            this.body.isKinematic = false;
            this.animParameters = this.FillInAnimParameters();

            this.radius = this.collider.radius;
            this.facingRight = true;

            this.platformMask = LayerMask.GetMask(Layers.PLATFORM);
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

        private void Update()
        {
            this.horizontal = (this.useGamepad) ? Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, this.gamepadIndex)) : Input.GetAxis(Inputs.AXIS_HORIZONTAL_KEYBOARD);
            this.vertical = (this.useGamepad) ? Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, this.gamepadIndex)) : Input.GetAxis(Inputs.AXIS_VERTICAL_KEYBOARD);

            this.anim.applyRootMotion = this.IsGrounded();
            this.isMoving = Mathf.Abs(this.horizontal) > 0.1f;
            this.isJumping = Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex));

            this.anim.SetFloat(this.animParameters[Constants.H], this.horizontal);
            this.anim.SetBool(this.animParameters[Constants.GROUNDED], this.IsGrounded());
        }

        private void FixedUpdate()
        {
            this.Move();

            this.Jump();
        }

        #region Main Logic

        private void Jump()
        {
            if (this.IsGrounded())
            {
                // Check if player has jumped
                if (this.isJumping)
                {
                    if (this.IsIdleState)
                    {
                        this.body.velocity = Vector3.up * this.jumpHeight;

                        this.currentIdleJump = true;
                    }
                    else if (this.IsLocomotionState)
                    {
                        var fVelocity = this.body.velocity;
                        fVelocity.y = this.jumpHeight;
                        this.body.velocity = fVelocity;

                        this.currentLocoJump = true;
                    }

                    this.StartJumpAnimator();
                } 
            }
            else
            {
                // Damp air depends on the type of jump
                if (this.currentIdleJump && this.body.velocity.y <= 1.0f)
                {
                    float velocityY = this.body.velocity.y;

                    // If receiving player's input
                    if (this.isMoving)
                    {
                        var fVelocity = this.transform.forward * this.speed * this.dampAir;
                        fVelocity.y = velocityY;

                        this.body.velocity = fVelocity;
                    }
                }
                else if (this.currentLocoJump)
                {
                    float velocityY = this.body.velocity.y;
                    var fVelocity = this.transform.forward * this.speed * this.dampAir;
                    fVelocity.y = velocityY;

                    this.body.velocity = fVelocity;
                }

                this.StopJumpAnimator();
            }
        }

        private void Move()
        {
            this.FlipCharacter(this.horizontal);

            if (this.IsGrounded())
            {
                this.currentIdleJump = false;
                this.currentLocoJump = false;

                // TODO: Should our character move like playerController
                // so as to avoid some obstacles ?

                // Check if player is moving
                if (this.isMoving)
                {
                    this.body.velocity = this.transform.forward * this.speed;
                    this.anim.SetFloat(this.animParameters[Constants.SPEED], this.speed);
                }
                else
                {
                    this.body.velocity = Vector3.zero;
                    this.anim.SetFloat(this.animParameters[Constants.SPEED], 0.0f);
                }
            }
        }

        #endregion

        #region Helpers

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, 0.2f);
        }

        public bool IsGrounded()
        {
            return Physics.CheckSphere(this.transform.position, 0.2f, this.platformMask);
        }

        public bool CanMove()
        {
            return this.IsGrounded() && (Mathf.Abs(this.horizontal) > 0.1f);
        }

        public bool CanJump()
        {
            return this.IsGrounded() && (Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex)));
        }

        private void StopJumpAnimator()
        {
            if (this.anim.GetBool(this.animParameters[Constants.LOCOMOTION_JUMP]))
            {
                this.anim.SetBool(this.animParameters[Constants.LOCOMOTION_JUMP], false);
            }

            if (this.anim.GetBool(this.animParameters[Constants.IDLE_JUMP]))
            {
                this.anim.SetBool(this.animParameters[Constants.IDLE_JUMP], false);
            }
        }

        private void StartJumpAnimator()
        {
            if (IsLocomotionState)
            {
                this.anim.SetBool(this.animParameters[Constants.LOCOMOTION_JUMP], true);
            }

            if (IsIdleState)
            {
                this.anim.SetBool(this.animParameters[Constants.IDLE_JUMP], true);
            }
        }

        #endregion

        #region Jump Events

        public void OnReduceCollider()
        {
            Debug.Log("OnReduceCollider");

            //Vector3 center = this.playerController.center;
            //center.y += 1.10f;

            //this.playerController.center = center;
            //this.playerController.height = 1.21f;
        }

        public void OnResetCollider()
        {
            Debug.Log("OnResetCollider");

            //Vector3 center = this.playerController.center;
            //center.y = 0.74f;

            //this.playerController.center = center;
            //this.playerController.height = 1.63f;
        }

        #endregion

        #region Flip Logic

        private void FlipCharacter(float horizontal)
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

        private void Flip()
        {
            this.facingRight = !this.facingRight;

            var rotation = this.transform.localRotation;
            rotation.y = -rotation.y;
            this.transform.localRotation = rotation;
        }

        #endregion

        #region Items Events

        public void InvertControl(float seconds)
        {
            var waitSeconds = new WaitForSeconds(seconds);

            this.StartCoroutine(this.InvertControlForSeconds(waitSeconds));
        }

        private IEnumerator InvertControlForSeconds(WaitForSeconds waitSeconds)
        {
            Debug.Log("Character " + this.transform.root.name + " control has been inverted");

            this.TransformLocalRotation();

            yield return waitSeconds;

            this.TransformLocalRotation();

            Debug.Log("Character " + this.transform.root.name + " control has been restablished");
        }

        private void TransformLocalRotation()
        {
            var rotation = this.transform.localRotation;
            rotation.y = -rotation.y;
            this.transform.localRotation = rotation;
        }

        public void IncreaseJumpForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.IncreaseJumpForSecondsInternal(percent, seconds));
        }

        private IEnumerator IncreaseJumpForSecondsInternal(float percent, float seconds)
        {
            int counterSeconds = 0;
            var oldJumpHeight = this.jumpHeight;

            while (counterSeconds < seconds)
            {
                this.IncreaseJump(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }

            Debug.Log("After " + seconds + " seconds, the jump height has been restored to: " + oldJumpHeight);

            this.jumpHeight = oldJumpHeight;
        }

        public void IncreaseJump(float percent)
        {
            if (percent > 0.0f)
            {
                var oldJumpHeight = this.jumpHeight;

                this.jumpHeight += (this.jumpHeight * percent);

                Debug.Log(String.Format("Jump of {0} is: {1} and applying {2}% more has changed to: {3}",
                        this.gameObject.name, oldJumpHeight, percent * 100.0f, this.jumpHeight));
            }
        }

        public void IncreaseSpeedForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.IncreaseSpeedForSecondsInternal(percent, seconds));
        }

        private IEnumerator IncreaseSpeedForSecondsInternal(float percent, float seconds)
        {
            int counterSeconds = 0;
            var oldSpeed = this.speed;

            while (counterSeconds < seconds)
            {
                this.IncreaseSpeed(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }

            Debug.Log("After " + seconds + " seconds, the speed has been restored to: " + oldSpeed);

            this.speed = oldSpeed;
        }

        public void IncreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {
                var oldSpeed = this.speed;

                this.speed += (this.speed * percent);

                Debug.Log(String.Format("Speed of {0} is: {1} and applying {2}% more has changed to: {3}",
                        this.gameObject.name, oldSpeed, percent * 100.0f, this.speed));
            }
        }

        #endregion
    }
}
