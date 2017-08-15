using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveAnimParameters = nseh.Utils.Constants.Animations.Movement;
using nseh.Managers.Main;
using BaseParameters = nseh.Utils.Constants.PlayerInfo;

namespace nseh.Gameplay.Entities.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]

    public class PlayerMovement : MonoBehaviour
    {

        #region Private Properties

        private Animator _anim;
        private Rigidbody _body;
        private PlayerInfo _playerInfo;     
        private int _inverted;
        private bool _facingRight;
        private bool _canUseDoubleJump;
        private bool _usedExtraJump;

        [Header("Speed")]
        [SerializeField]
        private float _currentSpeed;
        private float _oldSpeed;
        private float _oldJump;
        private float _timeJump;
        private float _timeSpeed;
        private float _timeConfusion;
        

        [Space(10)]
        private float _jumpHeight;
        private float _baseSpeed;
        private bool _enableMovement;

        #endregion

        #region Public Properties

        [Header("Audio Clips")]
        public AudioClip audioJump;
        public AudioClip audioLoopJump;

        public List<AudioClip> audioSteps;
        public bool grounded;

        #endregion

        #region Public C# Properties

        public bool IsFacingRight
        {
            get { return _facingRight; }

            set { _facingRight = value; }
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

        #region Private Methods

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _anim = _playerInfo.Animator;
            _body = _playerInfo.Body;
            _inverted = -1;
            _jumpHeight = BaseParameters.JUMPHEIGHT;
            _baseSpeed = BaseParameters.BASESPEED;
            if(GameManager.Instance.CurrentState == GameManager.States.Boss)
            {
                _jumpHeight = BaseParameters.JUMPHEIGHT/1.5f;
                _baseSpeed = BaseParameters.BASESPEED/1.5f;
            }

            _facingRight = (transform.localEulerAngles.y == 270.0f) ? true : false;
            _canUseDoubleJump = false;
            _currentSpeed = _baseSpeed;
            _usedExtraJump = false;
            _enableMovement = true;
            grounded = true;
        }

        private void Update()
        {
            if (!GameManager.Instance.isPaused && _enableMovement)
            {
                Move();               
                if (_playerInfo.JumpPressed)
                    Jump();               
            }

            _anim.SetBool(MoveAnimParameters.GROUNDED, grounded);
        }

        private void Jump()
        {
            if (grounded)
            {
                GameManager.Instance.SoundManager.PlayAudioFX(audioJump, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
                _body.velocity = new Vector3(_body.velocity.x, _jumpHeight, 0);
                _usedExtraJump = false;

            }
            else if (!grounded && _playerInfo.JumpPressed && !_usedExtraJump && _canUseDoubleJump)
            {
                GameManager.Instance.SoundManager.PlayAudioFX(audioJump, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
                _body.velocity = new Vector3(_body.velocity.x, _jumpHeight, 0);
                _usedExtraJump = true;
            }
        }

        private void Move()
        {   
            if (Mathf.Abs(_playerInfo.Horizontal) > 0.1)
            {
                _body.velocity = new Vector3(_inverted * Mathf.Round(_playerInfo.Horizontal) * (_currentSpeed + (float)(_currentSpeed * 0.05 * _playerInfo.CurrentAgility)), _body.velocity.y, 0);
                OnFlipPlayer(_playerInfo.Horizontal);
                _anim.SetBool(MoveAnimParameters.SPEED, true);
            }
                
            else
            {
                _body.velocity = new Vector3(0, _body.velocity.y, 0);
                _anim.SetBool(MoveAnimParameters.SPEED, false);
            }                          
        }

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

        private void Flip()
        {
            _facingRight = !_facingRight;
            var rotation = transform.localRotation;
            rotation.y = -rotation.y;
            transform.localRotation = rotation;
        }

        private void IncreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {
                _timeSpeed = Time.time;

                _currentSpeed = _baseSpeed;

                _currentSpeed += (_currentSpeed * percent / 100.0f);
            }
        }

        private void DecreaseSpeed(float percent)
        {
            if (percent > 0.0f)
            {
                _timeSpeed = Time.time;

                _currentSpeed = _baseSpeed;

                _currentSpeed -= (_currentSpeed * percent / 100.0f);

            }
        }

        private void InvertPlayerRotation()
        {
            if (_inverted == -1)
            {
                Quaternion rotation = transform.localRotation;
                rotation.y = -rotation.y;
                transform.localRotation = rotation;
            }

            _timeConfusion = Time.time;
            _inverted = 1;

        }

        #endregion

        #region Public Methods

        public void EnableMovement()
        {
            _enableMovement = true;

            if (grounded)
                _body.isKinematic = false;

        }

        public IEnumerator DisableMovement(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            _enableMovement = false;

			if (!grounded)
			{
                _playerInfo.Body.isKinematic = true;
                _playerInfo.Body.isKinematic = false;
                //_playerInfo.Body.useGravity = true;
                yield return new WaitForSeconds(1f);
                _playerInfo.Body.isKinematic = true;
            }
            else
            {
                _playerInfo.Body.isKinematic = true;
            }
			
        }

        public IEnumerator InvertControlForSeconds(float seconds)
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

        public IEnumerator DoubleJumpForSeconds(float seconds)
        {
            _canUseDoubleJump = true;
            _timeJump = Time.time;

            yield return new WaitForSeconds(seconds);

            if (Time.time >= _timeJump + seconds)
            {
                _canUseDoubleJump = false;
            }
        }

        public IEnumerator BonificationAgilityForSeconds(int points, float seconds)
        {
            _timeSpeed = Time.time;
            _playerInfo.CurrentAgility = _playerInfo.BaseAgility;
            _playerInfo.CurrentAgility += points;
            _currentSpeed = _baseSpeed;
            _currentSpeed += (float)(0.1 * _playerInfo.CurrentAgility * _currentSpeed);

            yield return new WaitForSeconds(seconds);

            if (Time.time >= _timeSpeed + seconds)
            {
                _playerInfo.CurrentAgility = _playerInfo.BaseAgility;
                _currentSpeed = _baseSpeed;
            }

        }

        public IEnumerator PenalizationAgilityForSeconds(int points, float seconds)
        {
            _playerInfo.CurrentAgility = _playerInfo.BaseAgility;
            _playerInfo.CurrentAgility -= points;
            _currentSpeed += (float)(0.1 * _playerInfo.CurrentAgility * _currentSpeed);

            yield return new WaitForSeconds(seconds);

            _playerInfo.CurrentAgility = _playerInfo.BaseAgility;
            _currentSpeed = _baseSpeed;

        }

        #endregion

        #region Animation Events

        public virtual void OnPlayJumpSound(AnimationEvent animationEvent)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(audioLoopJump, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        public virtual void OnPlayStepSound(AnimationEvent animationEvent)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(audioSteps[Random.Range(0, audioSteps.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        #endregion

    }
}
