using System;
using System.Collections;
using UnityEngine;
using Layers = nseh.Utils.Constants.Layers;

namespace nseh.Gameplay.Entities.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Private Properties

        private Animator anim;
        private Rigidbody body;
        private PlayerInfo playerInfo;

        private int platformMask;

        private bool facingRight;
        private bool movePressed;
        private bool jumpPressed;
        private bool currentIdleJump = false;
        private bool currentLocoJump = false;
        private bool usedExtraJump = false;

        private float horizontal;
        private float vertical;
        private float gravity;
        [SerializeField]
        private float currentSpeed;
        private float oldSpeed;

        #endregion

        #region Public Properties

        [Range(0, 1)]
        public float dampAir;
        public float jumpAirSpeed;
        public float jumpHeight;
        public float baseSpeed;

        #endregion

        #region Public C# Properties

        public bool IsFacingRight
        {
            get
            {
                return this.transform.root.transform.forward.x > 0.0f;
            }
        }

        public float CurrentSpeed
        {
            get
            {
                return this.currentSpeed;
            }

            set
            {
                this.currentSpeed = value;
            }
        }

        public float BaseSpeed
        {
            get
            {
                return this.baseSpeed;
            }

            set
            {
                this.baseSpeed = value;
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
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.playerInfo.IdleHash;
            }
        }

        private bool IsLocomotionState
        {
            get
            {
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.playerInfo.LocomotionHash;
            }
        }

        private bool IsIdleJumpState
        {
            get
            {
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.playerInfo.IdleJumpHash;
            }
        }

        private bool IsLocomotionJumpState
        {
            get
            {
                return this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.playerInfo.LocomotionJumpHash;
            }
        }

        private bool IsJumpingState
        {
            get
            {
                return this.IsLocomotionJumpState || this.IsIdleJumpState;
            }
        }

        #endregion

        private void Start()
        {
            this.OnSetupPlayerMovement();
        }

        private void Update()
        {
            this.horizontal = this.playerInfo.Horizontal;
            this.vertical = this.playerInfo.Vertical;

            this.movePressed = Mathf.Abs(this.horizontal) > 0.1f;
            this.jumpPressed = this.playerInfo.JumpPressed;

            this.anim.SetFloat(this.playerInfo.HorizontalStateName, this.horizontal);
            this.anim.SetBool(this.playerInfo.GroundedStateName, this.IsGrounded());

            this.OnFlipPlayer(this.horizontal);
            this.Move();
            this.Jump();
        }

        #region Main Logic

        private void ApplyJump()
        {
            // Depending on the current state, jump vector will be different
            if (this.IsIdleState)
            {
                this.body.velocity = Vector3.up * this.jumpHeight;

                this.currentIdleJump = true;
            }
            else if (this.IsLocomotionState)
            {
                var vVelocity = this.body.velocity;
                vVelocity.y = this.jumpHeight;
                this.body.velocity = vVelocity;

                this.currentLocoJump = true;
            }
            else if (this.IsJumpingState)
            {
                // For double jump
                var vVelocity = this.body.velocity;
                vVelocity.y = this.jumpHeight;
                this.body.velocity = vVelocity;
            }

            // Start animator
            this.StartJumpAnimator();
        }

        private void MotionInTheAir()
        {
            // Player is in the air
            // Double jump
            if (!this.usedExtraJump && this.jumpPressed)
            {
                this.usedExtraJump = true;

                this.ApplyJump();
            }
            else
            {
                // Should gravity be updated here to support customization
                // Damp air depends on the type of jump
                if (this.currentIdleJump && this.body.velocity.y <= 0.1f)
                {
                    // If player is moving in the air
                    if (this.movePressed)
                    {
                        Vector3 vLocalDirection = new Vector3(0.0f, this.body.velocity.y, this.jumpAirSpeed);
                        this.body.velocity = this.transform.TransformDirection(vLocalDirection);
                    }
                }
                else if (this.currentLocoJump)
                {
                    Vector3 vLocalDirection = new Vector3(0.0f, this.body.velocity.y, this.jumpAirSpeed);
                    this.body.velocity = this.transform.TransformDirection(vLocalDirection);
                }
            }
        }

        private void Jump()
        {
            // Player's first jump
            if (this.IsGrounded() && this.jumpPressed)
            {
                this.usedExtraJump = false;

                this.ApplyJump();
            }
            else
            {
                this.MotionInTheAir();
            }
        }

        private void Move()
        {
            if (this.IsGrounded())
            {
                // Stop jump animator when grounded
                this.StopJumpAnimator();
                this.currentIdleJump = false;
                this.currentLocoJump = false;

                // TODO: Should our character move like playerController
                // so as to avoid some obstacles ?

                // Check if player is moving
                if (this.movePressed)
                {
                    this.body.velocity = this.transform.forward * this.currentSpeed;
                    this.anim.SetFloat(this.playerInfo.SpeedStateName, this.currentSpeed);
                }
                else
                {
                    this.body.velocity = Vector3.zero;
                    this.anim.SetFloat(this.playerInfo.SpeedStateName, 0.0f);
                }
            }
        }

        #endregion

        #region Helpers

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, 0.35f);
        }

        public bool IsGrounded()
        {
            return Physics.CheckSphere(this.transform.position, 0.35f, this.platformMask) && this.body.velocity.y <= 0.1f;
        }

        private void StopJumpAnimator()
        {
            if (this.anim.GetBool(this.playerInfo.LocomotionJumpStateName))
            {
                this.anim.SetBool(this.playerInfo.LocomotionJumpStateName, false);
            }

            if (this.anim.GetBool(this.playerInfo.IdleJumpStateName))
            {
                this.anim.SetBool(this.playerInfo.IdleJumpStateName, false);
            }
        }

        private void StartJumpAnimator()
        {
            if (IsLocomotionState)
            {
                this.anim.SetBool(this.playerInfo.LocomotionJumpStateName, true);
            }

            if (IsIdleState)
            {
                this.anim.SetBool(this.playerInfo.IdleJumpStateName, true);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Enable player's movement
        /// </summary>
        public void EnableMovement()
        {
            this.enabled = true;
            this.playerInfo.Body.useGravity = true;
            this.playerInfo.Body.isKinematic = false;
        }

        /// <summary>
        /// Disable player's movement
        /// </summary>
        public void DisableMovement()
        {
            this.enabled = false;
            this.playerInfo.Body.useGravity = false;
            this.playerInfo.Body.isKinematic = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when PlayerMovement is enabled. Get all references again
        /// </summary>
        private void OnEnable()
        {
            this.OnSetupPlayerMovement();
        }

        /// <summary>
        /// Called when PlayerMovement component is activated
        /// </summary>
        private void OnSetupPlayerMovement()
        {
            this.playerInfo = GetComponent<PlayerInfo>();
            this.anim = this.playerInfo.Animator;
            this.body = this.playerInfo.Body;

            this.facingRight = (this.transform.localEulerAngles.y == 270.0f) ? true : false;
            this.platformMask = LayerMask.GetMask(Layers.PLATFORM);
            this.currentSpeed = this.baseSpeed;
        }

        #endregion

        #region Flip Logic

        /// <summary>
        /// Check if player can rotate and do it
        /// </summary>
        /// <param name="horizontal"></param>
        private void OnFlipPlayer(float horizontal)
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

        /// <summary>
        /// Flip player's rotation
        /// </summary>
        private void Flip()
        {
            this.facingRight = !this.facingRight;

            var rotation = this.transform.localRotation;
            rotation.y = -rotation.y;
            this.transform.localRotation = rotation;
        }

        #endregion

        #region Public Items Methods

        /// <summary>
        /// Invert input control
        /// </summary>
        /// <param name="seconds"></param>
        public void InvertControl(float seconds)
        {
            var waitSeconds = new WaitForSeconds(seconds);

            this.StartCoroutine(this.InvertControlForSeconds(waitSeconds));
        }

        /// <summary>
        /// Increase jump by percent for a total of seconds
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void IncreaseJumpForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.IncreaseJumpForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase speed by percent for a total of seconds
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void IncreaseSpeedForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.IncreaseSpeedForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase speed by percent
        /// </summary>
        /// <param name="percent"></param>
        public void IncreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {
                oldSpeed = this.currentSpeed;

                this.currentSpeed += (this.baseSpeed * percent / 100.0f);

                Debug.Log(String.Format("Speed of {0} is: {1} and applying {2}% more has changed to: {3}",
                        this.gameObject.name, oldSpeed, percent, this.currentSpeed));
            }
        }

        /// <summary>
        /// Decrease speed by percent 
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {
                oldSpeed = this.currentSpeed;

                this.currentSpeed -= (this.baseSpeed * percent / 100.0f);

                Debug.Log(String.Format("Speed of {0} is: {1} and applying {2}% less has changed to: {3}",
                        this.gameObject.name, oldSpeed, percent, this.currentSpeed));
            }
        }

        /// <summary>
        /// Decrease speed by percent when a player falls into Tar 
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseSpeedTar(float percent)
        {
            if (percent > 0.0f)
            {
                oldSpeed = this.baseSpeed - (this.baseSpeed * percent / 100.0f);

                this.currentSpeed -= (this.baseSpeed * percent / 100.0f);

                Debug.Log(String.Format("Speed of {0} is: {1} and applying {2}% less has changed to: {3}",
                        this.gameObject.name, oldSpeed, percent, this.currentSpeed));
            }
        }

        /// <summary>
        /// Increase jump by percent
        /// </summary>
        /// <param name="percent"></param>
        public void IncreaseJump(float percent)
        {
            if (percent > 0.0f)
            {
                var oldJumpHeight = this.jumpHeight;

                this.jumpHeight += (this.jumpHeight * percent / 100.0f);

                Debug.Log(String.Format("Jump of {0} is: {1} and applying {2}% more has changed to: {3}",
                        this.gameObject.name, oldJumpHeight, percent, this.jumpHeight));
            }
        }

        /// <summary>
        /// Decrease jump by percent
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseJump(float percent)
        {
            if (percent > 0.0f)
            {
                var oldJumpHeight = this.jumpHeight;

                this.jumpHeight -= (this.jumpHeight * percent / 100.0f);

                Debug.Log(String.Format("Jump of {0} is: {1} and applying {2}% less has changed to: {3}",
                        this.gameObject.name, oldJumpHeight, percent, this.jumpHeight));
            }
        }

        /// <summary>
        /// Set current speed to base speed
        /// </summary>
        public void RestoreBaseSpeed()
        {
            this.currentSpeed = this.baseSpeed;
            this.oldSpeed = this.baseSpeed;
        }

        #endregion

        #region Private Item Methods

        private void InvertPlayerRotation()
        {
            Quaternion rotation = this.transform.localRotation;
            rotation.y = -rotation.y;

            this.transform.localRotation = rotation;
        }

        private IEnumerator InvertControlForSeconds(WaitForSeconds waitSeconds)
        {
            Debug.Log("Character " + this.transform.root.name + " control has been inverted");

            this.InvertPlayerRotation();

            yield return waitSeconds;

            this.InvertPlayerRotation();

            Debug.Log("Character " + this.transform.root.name + " control has been restablished");
        }

        private IEnumerator IncreaseJumpForSecondsInternal(float percent, float seconds)
        {
            var oldJumpHeight = this.jumpHeight;

            this.IncreaseJump(percent);

            yield return new WaitForSeconds(seconds);

            Debug.Log(string.Format("Jump of {0} has been restored to: {1}", this.gameObject.name, oldJumpHeight));

            this.jumpHeight = oldJumpHeight;
        }

        private IEnumerator IncreaseSpeedForSecondsInternal(float percent, float seconds)
        {
            oldSpeed = this.currentSpeed;

            this.IncreaseSpeed(percent);

            yield return new WaitForSeconds(seconds);

            Debug.Log(string.Format("Speed of {0} has been restored to: {1}", this.gameObject.name, oldSpeed));

            this.currentSpeed = oldSpeed;
        }

        #endregion
    }
}
