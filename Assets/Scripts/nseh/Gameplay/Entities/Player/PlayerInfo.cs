using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using System;
using UnityEngine;
using Inputs = nseh.Utils.Constants.Input;
using InputUE = UnityEngine.Input;

namespace nseh.Gameplay.Entities.Player
{
    [RequireComponent(typeof(PlayerHealth))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCombat))]
    public partial class PlayerInfo : MonoBehaviour
    {
        #region Private Properties

        [Header("Input properties")]
        [SerializeField]
        private int _gamepadIndex;

        [Range(1, 4)]
        [SerializeField]
        private int _player;
        [SerializeField]
        private string _playerName;
        [SerializeField]
        private Sprite _characterPortrait;

        [SerializeField]
        private bool _bavaScene;

        [Space(20)]

        private AudioSource _soundPlayer;
        private Rigidbody _body;
        private Animator _animator;
        private Collider _playerCollider;
        private PlayerHealth _playerHealth;
        private PlayerEnergy _playerEnergy;
        private PlayerMovement _playerMovement;
        private PlayerCombat _playerCombat;

        private float _horizontal;
        private float _vertical;

        private int _score;

        private bool _teletransported;
        private bool _jumpPressed;

        #endregion

        #region Public C# Properties

        public float Horizontal
        {
            get
            {
                return _horizontal;
            }

            set
            {
                _horizontal = value;
            }
        }

        public float Vertical
        {
            get
            {
                return _vertical;
            }

            set
            {
                _vertical = value;
            }
        }

        public int GamepadIndex
        {
            get
            {
                return _gamepadIndex;
            }

            set
            {
                _gamepadIndex = value;
            }
        }

        public int Player
        {
            get
            {
                return _player;
            }

            set
            {
                _player = value;
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }

            set
            {
                _score = value;
            }
        }

        public bool Teletransported
        {
            get
            {
                return _teletransported;
            }

            set
            {
                _teletransported = value;
            }
        }

        public bool JumpPressed
        {
            get
            {
                return _jumpPressed;
            }
        }

        public Sprite CharacterPortrait
        {
            get
            {
                return (_characterPortrait) ? _characterPortrait : null;
            }
        }

        public AudioSource SoundPlayer
        {
            get { return _soundPlayer; }
        }

        public Rigidbody Body
        {
            get
            {
                return (_body) ? _body : null;
            }
        }

        public Animator Animator
        {
            get
            {
                return _animator;
            }
        }

        public PlayerHealth PlayerHealth
        {
            get
            {
                return _playerHealth;
            }
        }

        public PlayerEnergy PlayerEnergy
        {
            get
            {
                return _playerEnergy;
            }
        }

        public PlayerMovement PlayerMovement
        {
            get
            {
                return _playerMovement;
            }
        }

        public PlayerCombat PlayerCombat
        {
            get
            {
                return _playerCombat;
            }
        }

        public string PlayerName
        {
            get
            {
                return _playerName;
            }
        }

        public Collider PlayerCollider
        {
            get { return _playerCollider; }
        }

        #endregion

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _soundPlayer = GetComponent<AudioSource>();

            _playerHealth = GetComponent<PlayerHealth>();
            _playerEnergy = GetComponent<PlayerEnergy>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerCombat = GetComponent<PlayerCombat>();
            _playerCollider = GetComponent<Collider>();

            SetupParticles();
            SetupLookUpKeyParticles();
        }

        private void Start()
        {
            _score = 0;
            _teletransported = false;
            _jumpPressed = false;
        }

        private void Update()
        {
            _horizontal = InputUE.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, _gamepadIndex));
            _vertical = InputUE.GetAxis(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, _gamepadIndex));

            _jumpPressed = InputUE.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, _gamepadIndex));
        }

        #region Public Methods
        
        public string GetButton(AttackType type)
        {
            string button = null;

            switch(type)
            {
                case AttackType.CharacterAttackAStep1:
                case AttackType.CharacterAttackAStep2:
                case AttackType.CharacterAttackAStep3:
                    button = String.Format("{0}{1}", Inputs.A, _gamepadIndex);
                    break;

                case AttackType.CharacterAttackBStep1:
                case AttackType.CharacterAttackBStep2:
                    button = String.Format("{0}{1}", Inputs.B, _gamepadIndex);
                    break;

                case AttackType.CharacterAttackBSharp:
                    button = null;
                    break;

                case AttackType.CharacterDefinitive:
                    button = String.Format("{0}{1}", Inputs.DEFINITIVE, _gamepadIndex);
                    break;

                case AttackType.CharacterHability:
                    button = String.Format("{0}{1}", Inputs.HABILITY, _gamepadIndex);
                    break;

                case AttackType.None:
                    button = null;
                    break;
            }

            return button;
        }

        public string GetButton(DefenseType type)
        {
            string button = null;

            switch (type)
            {
                case DefenseType.None:
                    button = null;
                    break;

                case DefenseType.NormalDefense:
                    button = String.Format("{0}{1}", Inputs.DEFENSE, _gamepadIndex);
                    break;
            }

            return button;
        } 

        #endregion
    }
}
