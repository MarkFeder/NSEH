using System;
using System.Collections;
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
        #region Public Properties

        [Header("Input properties")]
        public int gamepadIndex;

        [Range(1, 4)]
        public int player;
        public string playerName;
        public Sprite characterPortrait;

        [Space(20)]

        #endregion

        #region Private Properties

        private Rigidbody body;
        private Animator animator;
        private Collider playerCollider;
        private PlayerHealth playerHealth;
        private PlayerEnergy playerEnergy;
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;

        private float horizontal;
        private float vertical;

        private int score;

        private bool teletransported;
        private bool jumpPressed;

        #endregion

        #region Public C# Properties

        public float Horizontal
        {
            get
            {
                return this.horizontal;
            }

            set
            {
                this.horizontal = value;
            }
        }

        public float Vertical
        {
            get
            {
                return this.vertical;
            }

            set
            {
                this.vertical = value;
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
                this.gamepadIndex = value;
            }
        }

        public int Player
        {
            get
            {
                return this.player;
            }

            set
            {
                this.player = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }

        public bool Teletransported
        {
            get
            {
                return this.teletransported;
            }

            set
            {
                this.teletransported = value;
            }
        }

        public bool JumpPressed
        {
            get
            {
                return this.jumpPressed;
            }
        }

        public Sprite CharacterPortrait
        {
            get
            {
                return (this.characterPortrait) ? this.characterPortrait : null;
            }
        }

        public Rigidbody Body
        {
            get
            {
                return (this.body) ? this.body : null;
            }
        }

        public Animator Animator
        {
            get
            {
                return this.animator;
            }
        }

        public PlayerHealth PlayerHealth
        {
            get
            {
                return this.playerHealth;
            }
        }

        public PlayerEnergy PlayerEnergy
        {
            get
            {
                return this.playerEnergy;
            }
        }

        public PlayerMovement PlayerMovement
        {
            get
            {
                return this.playerMovement;
            }
        }

        public PlayerCombat PlayerCombat
        {
            get
            {
                return this.playerCombat;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
        }

        public Collider PlayerCollider
        {
            get { return this.playerCollider; }
        }

        


        #endregion

        private void Awake()
        {
            this.body = GetComponent<Rigidbody>();
            this.animator = GetComponent<Animator>();

            this.playerHealth = GetComponent<PlayerHealth>();
            this.playerEnergy = GetComponent<PlayerEnergy>();
            this.playerMovement = GetComponent<PlayerMovement>();
            this.playerCombat = GetComponent<PlayerCombat>();
            this.playerCollider = GetComponent<Collider>();

            this.SetupParticles();
            this.SetupLookUpKeyParticles();
    }

        private void Start()
        {
            this.score = 0;
            this.teletransported = false;
            this.jumpPressed = false;
        }

        private void Update()
        {
            this.horizontal = InputUE.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, this.gamepadIndex));
            this.vertical = InputUE.GetAxis(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, this.gamepadIndex));

            this.jumpPressed = InputUE.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex));
        }
    }
}
