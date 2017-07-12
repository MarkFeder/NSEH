using System;
using UnityEngine;
using nseh.Managers.Main;
using System.Collections.Generic;
using Inputs = nseh.Utils.Constants.Input;
using BaseParameters = nseh.Utils.Constants.PlayerInfo;

namespace nseh.Gameplay.Entities.Player
{
    [RequireComponent(typeof(PlayerCombat))]
    [RequireComponent(typeof(PlayerMovement))]

    public partial class PlayerInfo : MonoBehaviour
    {

        #region Private Properties

        [Header("Stats")]
        [SerializeField]
        private int _baseStrength;
        [SerializeField]
        private int _currentStrength;

        [SerializeField]
        private int _baseEndurance;
        [SerializeField]
        private int _currentEndurance;

        [SerializeField]
        private int _baseAgility;
        [SerializeField]
        private int _currentAgility;

        [Space(10)]

        [Header("Health")]
        [SerializeField]
        private float _currentHealth; /*Just Debug*/
        private int _maxHealth;
        private int _penalization;

        [Space(10)]

        [Header("Energy")]
        [SerializeField]
        private float _currentEnergy;

        [Serializable]
        public struct ItemsList
        {
            public string name;
            public float time;
            public GameObject particle;
        }
        [Header("Items")]
        public List<ItemsList> items;

        [Header("Score")]
        [SerializeField]
        private int _currentScore;

        [Header("Player's body position")]
        [SerializeField]
        private Transform particleBodyPos;
        [SerializeField]
        private Transform particleHeadPos;
        [SerializeField]
        private Transform particleFootPos;

        [Space(10)]


        [Header("Particles")]
        [SerializeField]
        private GameObject _hitParticle;
  
        [Space(10)]

        [Header("Audio Clips")]
        [SerializeField]
        private List<AudioClip> _hitClip;
        [SerializeField]
        private List<AudioClip> _takeDamageClip;

        [SerializeField]
        private AudioClip _deathClip;

        [Space(10)]

        [Header("Input properties")]
        [SerializeField]
        private int _gamepadIndex;
        [SerializeField]
        private string _playerName;
        [SerializeField]
        private Sprite _characterPortrait;

        [Space(10)]

        [Header("Skeleton Colliders")]
        [SerializeField]
        private List<Collider> colliderList;

        private Rigidbody _body;
        private Animator _animator;
        

        private PlayerMovement _playerMovement;
        private PlayerCombat _playerCombat;

        private String _horizontalString;
        private String _verticalString;

        private String _interactString;
        private String _jumpString;
        private String _lightAttackString;
        private String _heavyAttackString;
        private String _defenseString;
        private String _abilityString;
        private String _definitiveString;
        private String _pauseString;

        private float _horizontal;
        private float _vertical;

        private bool _teletransported;
        private bool _jumpPressed;
        private bool _lightAttackPressed;
        private bool _heavyAttackPressed;
        private bool _interactPressed;
        private bool _defensePressed;
        private bool _abilityPressed;
        private bool _definitivePressed;
        private bool _pausePressed;


        #endregion

        #region Public C# Properties

        public float Horizontal
        {
            get { return _horizontal; }
            set { _horizontal = value; }
        }
        
        public float Vertical
        {
            get { return _vertical; }
            set { _vertical = value; }
        }

        public int GamepadIndex
        {
            get { return _gamepadIndex; }
            set { _gamepadIndex = value; }
        }

        public bool Teletransported
        {
            get { return _teletransported; }
            set { _teletransported = value; }
        }

        public Sprite CharacterPortrait
        {
            get { return (_characterPortrait) ? _characterPortrait : null; }
        }

        public Rigidbody Body
        {
            get { return (_body) ? _body : null; }
        }

        public Animator Animator
        {
            get { return _animator; }
        }

        public PlayerMovement PlayerMovement
        {
            get { return _playerMovement; }
        }

        public PlayerCombat PlayerCombat
        {
            get { return _playerCombat; }
        }

        public string PlayerName
        {
            get { return _playerName; }
        }

        public Transform ParticleBodyPos
        {
            get
            {
                return particleBodyPos;
            }

        }

        public Transform ParticleHeadPos
        {
            get
            {
                return particleHeadPos;
            }

        }

        public Transform ParticleFootPos
        {
            get
            {
                return particleFootPos;
            }
        }

        public int CurrentEndurance
        {
            get
            {
                return _currentEndurance;
            }
        }

        public int CurrentStrength
        {
            get
            {
                return _currentStrength;
            }
        }

        public int CurrentAgility
        {
            get
            {
                return _currentAgility;
            }

            set { _currentAgility = value; }
        }

        public bool JumpPressed
        {
            get
            {
                return _jumpPressed;
            }

            set
            {
                _jumpPressed = value;
            }
        }

        public bool LightAttackPressed
        {
            get
            {
                return _lightAttackPressed;
            }

            set
            {
                _lightAttackPressed = value;
            }
        }

        public bool HeavyAttackPressed
        {
            get
            {
                return _heavyAttackPressed;
            }

            set
            {
                _heavyAttackPressed = value;
            }
        }

        public bool InteractPressed
        {
            get
            {
                return _interactPressed;
            }

            set
            {
                _interactPressed = value;
            }
        }

        public bool DefensePressed
        {
            get
            {
                return _defensePressed;
            }

            set
            {
                _defensePressed = value;
            }
        }

        public bool AbilityPressed
        {
            get
            {
                return _abilityPressed;
            }

            set
            {
                _abilityPressed = value;
            }
        }

        public bool DefinitivePressed
        {
            get
            {
                return _definitivePressed;
            }

            set
            {
                _definitivePressed = value;
            }
        }

        public bool PausePressed
        {
            get
            {
                return _pausePressed;
            }

            set
            {
                _pausePressed = value;
            }
        }

        public int CurrentScore
        {
            get
            {
                return _currentScore;
            }
        }

        public GameObject HitParticle
        {
            get
            {
                return _hitParticle;
            }
        }

        public int BaseAgility
        {
            get
            {
                return _baseAgility;
            }

        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _maxHealth = BaseParameters.MAXHEALTH;
            _penalization = BaseParameters.PENALIZATION;
            _maxEnergy = BaseParameters.MAXENERGY;
            _playerMovement = GetComponent<PlayerMovement>();
            _playerCombat = GetComponent<PlayerCombat>();
            _currentAgility = _baseAgility;
            _currentEndurance = _baseEndurance;
            _currentStrength = _baseStrength;
        }

        private void Start()
        {
            SetupInputStrings();
            _teletransported = false;

            MaxHealth = _maxHealth;
            CurrentHealth = _maxHealth;
            _deathCount = 0;
            _isDead = false;
            
            MaxEnergy = _maxEnergy;
            CurrentEnergy = _currentEnergy;
        }

        private void Update()
        {
            if (!GameManager.Instance.isPaused && !_isDead)
            {
                _horizontal = Input.GetAxis(_horizontalString);
                _vertical = Input.GetAxis(_verticalString);
                JumpPressed = Input.GetButtonDown(_jumpString);
                LightAttackPressed = Input.GetButtonDown(_lightAttackString);
                HeavyAttackPressed = Input.GetButtonDown(_heavyAttackString);
                InteractPressed = Input.GetButtonDown(_interactString);
                DefensePressed = Input.GetButtonDown(_defenseString);
                AbilityPressed = Input.GetButtonDown(_abilityString);
                DefinitivePressed = Input.GetButtonDown(_definitiveString);
                PausePressed = Input.GetButtonDown(_pauseString);
            }
        }

        private void SetupInputStrings()
        {
            _horizontalString = String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, _gamepadIndex);
            _verticalString = String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, _gamepadIndex);

            _interactString = String.Format("{0}{1}", Inputs.INTERACT, _gamepadIndex);
            _jumpString = String.Format("{0}{1}", Inputs.JUMP, _gamepadIndex);
            _lightAttackString = String.Format("{0}{1}", Inputs.A, _gamepadIndex);
            _heavyAttackString = String.Format("{0}{1}", Inputs.B, _gamepadIndex);
            _defenseString = String.Format("{0}{1}", Inputs.DEFENSE, _gamepadIndex);
            _abilityString = String.Format("{0}{1}", Inputs.ABILITY, _gamepadIndex);
            _definitiveString = String.Format("{0}{1}", Inputs.DEFINITIVE, _gamepadIndex);
            _pauseString = String.Format("{0}{1}", Inputs.OPTIONS, _gamepadIndex);
    }

        #endregion

    }
}
