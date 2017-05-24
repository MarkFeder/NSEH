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
        
        private int inverted;
        private int platformMask;

        private bool facingRight;
        private bool movePressed;
        private bool jumpPressed;
        private bool canUseDoubleJump = false;
        private bool currentIdleJump = false;
        private bool currentLocoJump = false;
        private bool usedExtraJump = false;

        private float horizontal;
        private float vertical;
        private float gravity;
        [SerializeField]
        private float currentSpeed;
        private float oldSpeed;
        private float oldJump;
        private float timeJump;
        private float timeSpeed;
        private float timeConfusion;

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
                return this.facingRight;
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

        private void Jump()
        {
            if (this.IsGrounded() && this.jumpPressed)
            {
                this.body.velocity = new Vector3(this.body.velocity.x, this.jumpHeight, 0);
                this.usedExtraJump = false;
            }
            else if (!this.IsGrounded() && this.jumpPressed && this.usedExtraJump == false && this.canUseDoubleJump)
            {
                this.body.velocity = new Vector3(this.body.velocity.x, this.jumpHeight, 0);
                this.usedExtraJump = true;
            }
           
        }

        private void Move()
        {
            if (this.movePressed)
            {
                this.body.velocity = new Vector3(inverted*this.horizontal*this.currentSpeed, this.body.velocity.y, 0);
                this.anim.SetFloat(this.playerInfo.SpeedStateName, this.currentSpeed);
            }
            else
            {
                this.body.velocity = new Vector3(0, this.body.velocity.y, 0);
                this.anim.SetFloat(this.playerInfo.SpeedStateName, 0.0f);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Check if the player is on the ground.
        /// </summary>
        /// <returns></returns>
        public bool IsGrounded()
        {
            return Physics.CheckSphere(this.transform.position, 0.35f, this.platformMask) && this.body.velocity.y <= 0.1f;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Enable player's movement.
        /// </summary>
        public void EnableMovement()
        {
            this.enabled = true;
            this.playerInfo.Body.useGravity = true;
            this.playerInfo.Body.isKinematic = false;
        }

        /// <summary>
        /// Disable player's movement.
        /// </summary>
        public void DisableMovement(float seconds)
        {
            StartCoroutine(CorutineDisable(seconds));
        }

        public IEnumerator CorutineDisable(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            this.enabled = false;
            if(!this.IsGrounded())
                this.playerInfo.Body.useGravity = true;
            this.playerInfo.Body.isKinematic = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when PlayerMovement is enabled. Get all references again.
        /// </summary>
        private void OnEnable()
        {
            this.OnSetupPlayerMovement();
        }

        /// <summary>
        /// Called when PlayerMovement component is activated.
        /// </summary>
        private void OnSetupPlayerMovement()
        {
            this.playerInfo = GetComponent<PlayerInfo>();
            this.anim = this.playerInfo.Animator;
            this.body = this.playerInfo.Body;
            this.inverted = -1;

            this.facingRight = (this.transform.localEulerAngles.y == 270.0f) ? true : false;
            this.platformMask = LayerMask.GetMask(Layers.PLATFORM);
            this.currentSpeed = this.baseSpeed;
            this.oldSpeed = this.currentSpeed;
            this.oldJump = jumpHeight;
        }

        #endregion

        #region Flip Logic
        
        /// <summary>
        /// Check if player can rotate and do it.
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
        /// Flip player's rotation.
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
        /// Invert input control.
        /// </summary>
        /// <param name="seconds"></param>
        public void InvertControl(float seconds)
        {
            this.StartCoroutine(this.InvertControlForSeconds(seconds));
        }

        /// <summary>
        /// Increase jump by percent for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void IncreaseJumpForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.IncreaseJumpForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase speed by percent for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void IncreaseSpeedForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.IncreaseSpeedForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Decrease speed by percent for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void DecreaseSpeedForSeconds(float percent, float seconds)
        {
            StartCoroutine(this.DecreaseSpeedForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase speed by percent.
        /// </summary>
        /// <param name="percent"></param>
        public void IncreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {
                timeSpeed = Time.time;

                this.currentSpeed = this.oldSpeed;

                this.currentSpeed += (this.baseSpeed * percent / 100.0f);

            }
        }

        /// <summary>
        /// Decrease speed by percent.
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {

                this.currentSpeed -= (this.baseSpeed * percent / 100.0f);

            }
        }

        /// <summary>
        /// Decrease speed by percent when a player falls into Tar.
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseSpeedTar(float percent)
        {
            if (percent > 0.0f)
            {
                oldSpeed = this.baseSpeed - (this.baseSpeed * percent / 100.0f);

                this.currentSpeed -= (this.baseSpeed * percent / 100.0f);

            }
        }

        /// <summary>
        /// Increase jump by percent.
        /// </summary>
        /// <param name="percent"></param>
        public void IncreaseJump(float percent)
        {

            if (percent > 0.0f)
            {
                timeJump = Time.time;

                this.jumpHeight = this.oldJump;

                this.jumpHeight += (this.jumpHeight * percent / 100.0f);


            }
        }

        /// <summary>
        /// Decrease jump by percent.
        /// </summary>
        /// <param name="percent"></param>
        public void DecreaseJump(float percent)
        {
            if (percent > 0.0f)
            {

                this.jumpHeight -= (this.jumpHeight * percent / 100.0f);


            }
        }

        /// <summary>
        /// Set current speed to base speed.
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
            if(inverted == -1)
            {
                Quaternion rotation = this.transform.localRotation;
                rotation.y = -rotation.y;
                this.transform.localRotation = rotation;
            }

            timeConfusion = Time.time;
            inverted = 1;
                    
        }


        private IEnumerator InvertControlForSeconds(float seconds)
        {

            this.InvertPlayerRotation();

            yield return new WaitForSeconds(seconds);

            if (Time.time >= timeConfusion + seconds)
            {
                inverted = -1;
                Quaternion rotation = this.transform.localRotation;
                rotation.y = -rotation.y;
                this.transform.localRotation = rotation;
            }
               

        }

        private IEnumerator IncreaseJumpForSecondsInternal(float percent, float seconds)
        {

            //this.IncreaseJump(percent);

            this.canUseDoubleJump = true;
            timeJump = Time.time;

            yield return new WaitForSeconds(seconds);

            if (Time.time >= timeJump+seconds)
                this.canUseDoubleJump = false;


        }

        private IEnumerator IncreaseSpeedForSecondsInternal(float percent, float seconds)
        {
            this.IncreaseSpeed(percent);

            yield return new WaitForSeconds(seconds);

            if (Time.time >= timeSpeed + seconds)
                this.currentSpeed = oldSpeed;


        }

        private IEnumerator DecreaseSpeedForSecondsInternal(float percent, float seconds)
        {

            this.DecreaseSpeed(percent);

            yield return new WaitForSeconds(seconds);


        }

        #endregion
    }
}
