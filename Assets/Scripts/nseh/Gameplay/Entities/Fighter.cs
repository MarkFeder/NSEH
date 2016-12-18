using System;
using UnityEngine;
using System.Collections;

namespace nseh.Gameplay
{
    public enum FighterStates
    {

        IDLE, WALK, WALK_BACK, JUMP, DUCK, HADOKEN,
        ATTACK, TAKE_HIT, TAKE_HIT_DEFEND,
        DEFEND, CELEBRATE, DEAD, NONE
    }


    public class Fighter : MonoBehaviour
    {
        #region Constants

        private const string AXIS_HORIZONTAL = "Horizontal";
        private const string AXIS_VERTICAL = "Vertical";

        public enum PlayerType
        {
            HUMAN = 0,
            AI = 1
        }

        #endregion

        #region  Properties

        public static float MAX_HEALTH = 100.0f;

        public float HealthPercent
        {
            get { return _health / MAX_HEALTH; }
        }

        private float _health = MAX_HEALTH;
        public float Health
        {
            get { return _health; }
            set
            {
                if (value < 0.0f)
                {
                    this._health = value;
                }

                this._health = value;
            }
        }

        private string _fighterName;
        public string FighterName
        {
            get { return _fighterName; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this._fighterName = value;
                }

                this._fighterName = String.Empty;
            }
        }


        public PlayerType _player;
        public PlayerType Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public FighterStates _currentState = FighterStates.IDLE;
        public FighterStates CurrentState
        {
            get { return _currentState; }
            set { this._currentState = value; }
        }

        private Animator _animator;
        protected Animator Animator
        {
            get { return _animator; }
            set { _animator = value; }
        }

        private Rigidbody _body;
        public Rigidbody Body
        {
            get { return _body; }
            set { _body = value; }
        }

        private Fighter _oponent;
        public Fighter Oponent
        {
            get { return _oponent; }
            set { _oponent = value; }
        }

        #endregion

        #region  Input 

        public void UpdateHumanInput()
        {
            MoveHorizontal();

            MoveVertical();

            ExecuteAction();
        }

        private void MoveHorizontal()
        {
            if (Input.GetAxis(AXIS_HORIZONTAL) > 0.1)
            {
                Animator.SetBool(AnimatorsParameters.Ryu.WALK, true);
            }
            else
            {
                Animator.SetBool(AnimatorsParameters.Ryu.WALK, false);
            }

            if (Input.GetAxis(AXIS_HORIZONTAL) < -0.1)
            {
                Animator.SetBool(AnimatorsParameters.Ryu.WALK_BACK, true);
            }
            else
            {
                Animator.SetBool(AnimatorsParameters.Ryu.WALK_BACK, false);
            }
        }

        private void MoveVertical()
        {
            if (Input.GetAxis(AXIS_VERTICAL) < -0.1)
            {
                Animator.SetBool(AnimatorsParameters.Ryu.DUCK, true);
            }
            else
            {
                Animator.SetBool(AnimatorsParameters.Ryu.DUCK, false);
            }
        }

        private void ExecuteAction()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Animator.SetTrigger(AnimatorsParameters.Ryu.JUMP);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Animator.SetTrigger(AnimatorsParameters.Ryu.PUNCH);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Animator.SetTrigger(AnimatorsParameters.Ryu.KICK);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Animator.SetTrigger(AnimatorsParameters.Ryu.HADOKEN);
            }
        }

        #endregion

        // Use this for initialization
        void Start()
        {
            Body = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            Animator.SetFloat("health", HealthPercent);

            if (Oponent != null)
            {
                Animator.SetFloat("oponent_health", Oponent.HealthPercent);
            }
            else
            {
                Animator.SetFloat("oponent_health", 1);
            }

            if (Player == PlayerType.HUMAN)
            {
                UpdateHumanInput();
            }
        }
    }

}