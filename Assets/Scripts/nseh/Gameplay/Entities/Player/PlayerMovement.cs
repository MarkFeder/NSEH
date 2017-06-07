using System.Collections;
using nseh.Managers.Audio;
using nseh.Managers.Main;
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

        private Animator _anim;
        private Rigidbody _body;
        private PlayerInfo _playerInfo;
        
        private int _inverted;
        private int _platformMask;

        private bool _facingRight;
        private bool _movePressed;
        private bool _jumpPressed;
        private bool _canUseDoubleJump = false;
        private bool _currentIdleJump = false;
        private bool _currentLocoJump = false;
        private bool _usedExtraJump = false;

        private float _horizontal;
        private float _vertical;
        private float _gravity;

        [SerializeField]
        private float _currentSpeed;
        private float _oldSpeed;
        private float _oldJump;
        private float _timeJump;
        private float _timeSpeed;
        private float _timeConfusion;

        [Range(0,1)]
        [SerializeField]
        private float _dampAir;
        [SerializeField]
        private float _jumpAirSpeed;
        [SerializeField]
        private float _jumpHeight;
        [SerializeField]
        private float _baseSpeed;

        public AudioClip audio;

        #endregion

        #region Public C# Properties

        public bool IsFacingRight
        {
            get { return _facingRight; }
        }

        public float CurrentSpeed
        {
            get { return _currentSpeed; }
            set { _currentSpeed = value; }
        }

        public float BaseSpeed
        {
            get { return _baseSpeed; }
            set { _baseSpeed = value; }
        }

        public bool IsFallingDown
        {
            get { return _body.velocity.y < 0.0f; }
        }

        #endregion

        private void Start()
        {
            OnSetupPlayerMovement();
        }

        private void Update()
        {
            _horizontal = _playerInfo.Horizontal;
            _vertical = _playerInfo.Vertical;

            _movePressed = Mathf.Abs(_horizontal) > 0.1f;
            _jumpPressed = _playerInfo.JumpPressed;

            _anim.SetFloat(_playerInfo.HorizontalStateName, _horizontal);
            _anim.SetBool(_playerInfo.GroundedStateName, IsGrounded());

            OnFlipPlayer(_horizontal);
            Move();
            Jump();
           
        }


        public virtual void OnPlayJumpSound(AnimationEvent animationEvent)
        {
            AudioSource.PlayClipAtPoint(audio, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
        }

        #region Main Logic

        private void Jump()
        {
            if (IsGrounded() && _jumpPressed)
            {
                _body.velocity = new Vector3(_body.velocity.x, _jumpHeight, 0);
                _usedExtraJump = false;
            }
            else if (!IsGrounded() && _jumpPressed && !_usedExtraJump && _canUseDoubleJump)
            {
                _body.velocity = new Vector3(_body.velocity.x, _jumpHeight, 0);
                _usedExtraJump = true;
            }
        }

        private void Move()
        {
            if (_movePressed)
            {
                _body.velocity = new Vector3(_inverted * _horizontal * _currentSpeed, _body.velocity.y, 0);
                _anim.SetFloat(_playerInfo.SpeedStateName, _currentSpeed);
            }
            else
            {
                _body.velocity = new Vector3(0, _body.velocity.y, 0);
                _anim.SetFloat(_playerInfo.SpeedStateName, 0.0f);
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
            return Physics.CheckSphere(transform.position, 0.35f, _platformMask) && _body.velocity.y <= 0.1f;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Enable player's movement.
        /// </summary>
        public void EnableMovement()
        {
            enabled = true;
            _playerInfo.Body.useGravity = true;
            _playerInfo.Body.isKinematic = false;
        }

        /// <summary>
        /// Disable player's movement.
        /// </summary>
        public void DisableMovement(float seconds)
        {
            StartCoroutine(DisableMovementInternal(seconds));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when PlayerMovement is enabled. Get all references again.
        /// </summary>
        private void OnEnable()
        {
            OnSetupPlayerMovement();
        }

        /// <summary>
        /// Called when PlayerMovement component is activated.
        /// </summary>
        private void OnSetupPlayerMovement()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _anim = _playerInfo.Animator;
            _body = _playerInfo.Body;
            _inverted = -1;

            _facingRight = (transform.localEulerAngles.y == 270.0f) ? true : false;
            _platformMask = LayerMask.GetMask(Layers.PLATFORM);
            _currentSpeed = _baseSpeed;
            _oldSpeed = _currentSpeed;
            _oldJump = _jumpHeight;
        }

        #endregion

        #region Flip Logic
        
        /// <summary>
        /// Check if player can rotate and do it.
        /// </summary>
        /// <param name="horizontal"></param>
        private void OnFlipPlayer(float horizontal)
        {
            if (horizontal > 0.0f && !_facingRight)
            {
                Flip();
            }
            else if (horizontal < 0.0f && _facingRight)
            {
                Flip();
            }
        }

        /// <summary>
        /// Flip player's rotation.
        /// </summary>
        private void Flip()
        {
            _facingRight = !_facingRight;
            var rotation = transform.localRotation;
            rotation.y = -rotation.y;
            transform.localRotation = rotation;
        }
        
        #endregion

        #region Public Items Methods

        /// <summary>
        /// Invert input control.
        /// </summary>
        /// <param name="seconds"></param>
        public void InvertControl(float seconds)
        {
            StartCoroutine(InvertControlForSeconds(seconds));
        }

        /// <summary>
        /// Increase jump by percent for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void IncreaseJumpForSeconds(float percent, float seconds)
        {
            StartCoroutine(IncreaseJumpForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase speed by percent for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void IncreaseSpeedForSeconds(float percent, float seconds)
        {
            StartCoroutine(IncreaseSpeedForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Decrease speed by percent for a total of seconds.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="seconds"></param>
        public void DecreaseSpeedForSeconds(float percent, float seconds)
        {
            StartCoroutine(DecreaseSpeedForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase speed by percent.
        /// </summary>
        /// <param name="percent"></param>
        public void IncreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {
                _timeSpeed = Time.time;

                _currentSpeed = _oldSpeed;

                _currentSpeed += (_baseSpeed * percent / 100.0f);

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

                _currentSpeed -= (_baseSpeed * percent / 100.0f);

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
                _oldSpeed = _baseSpeed - (_baseSpeed * percent / 100.0f);

                _currentSpeed -= (_baseSpeed * percent / 100.0f);

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
                _timeJump = Time.time;

                _jumpHeight = _oldJump;

                _jumpHeight += (_jumpHeight * percent / 100.0f);


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

                _jumpHeight -= (_jumpHeight * percent / 100.0f);


            }
        }

        /// <summary>
        /// Set current speed to base speed.
        /// </summary>
        public void RestoreBaseSpeed()
        {
            _currentSpeed = _baseSpeed;
            _oldSpeed = _baseSpeed;
        }

        #endregion

        #region Private Item Methods

        private void InvertPlayerRotation()
        {
            if(_inverted == -1)
            {
                Quaternion rotation = transform.localRotation;
                rotation.y = -rotation.y;
                transform.localRotation = rotation;
            }

            _timeConfusion = Time.time;
            _inverted = 1;
                    
        }

		private IEnumerator DisableMovementInternal(float seconds)
		{
			yield return new WaitForSeconds(seconds);

			enabled = false;

			if (!IsGrounded())
			{
				_playerInfo.Body.useGravity = true;
			}

			_playerInfo.Body.isKinematic = true;
		}

        private IEnumerator InvertControlForSeconds(float seconds)
        {

            InvertPlayerRotation();

            yield return new WaitForSeconds(seconds);

            if (Time.time >= _timeConfusion + seconds)
            {
                _inverted = -1;
                Quaternion rotation = transform.localRotation;
                rotation.y = -rotation.y;
                transform.localRotation = rotation;
            }
               

        }

        private IEnumerator IncreaseJumpForSecondsInternal(float percent, float seconds)
        {

            //IncreaseJump(percent);

            _canUseDoubleJump = true;
            _timeJump = Time.time;

            yield return new WaitForSeconds(seconds);

            if (Time.time >= _timeJump+seconds)
            {
				_canUseDoubleJump = false;
			}
        }

        private IEnumerator IncreaseSpeedForSecondsInternal(float percent, float seconds)
        {
            IncreaseSpeed(percent);

            yield return new WaitForSeconds(seconds);

            if (Time.time >= _timeSpeed + seconds)
            {
				_currentSpeed = _oldSpeed;
			}
        }

        private IEnumerator DecreaseSpeedForSecondsInternal(float percent, float seconds)
        {

            DecreaseSpeed(percent);

            yield return new WaitForSeconds(seconds);
        }

        #endregion
    }
}
