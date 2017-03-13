using System;
using UnityEngine;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        #region Public Properties

        [Header("Input properties")]
        public int gamepadIndex;

        [Space(20)]

        #endregion

        #region Private Properties

        private float horizontal;
        private float vertical;

        [Range(1, 4)]
        public int player;

        private bool teletransported;
        private bool jumpPressed;

        public Sprite characterPortrait;

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

        #endregion

        private void Start()
        {
            this.teletransported = false;
            this.jumpPressed = false;
        }

        private void Update()
        {
            this.horizontal = Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, this.gamepadIndex));
            this.vertical = Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, this.gamepadIndex));

            this.jumpPressed = Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex));
        }
    }
}
