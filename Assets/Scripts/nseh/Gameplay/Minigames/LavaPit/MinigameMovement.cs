using UnityEngine;
using System;
using System.Collections.Generic;
using nseh.Managers.Main;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Minigames
{
    public class MinigameMovement : MonoBehaviour
    {

        #region Public Properties

        public float speedVertical;
        public float speedHorizontal;
        public int gamepadIndex;
        public bool started;
        public int puntuation;
        public List<AudioClip> audioSteps;

        #endregion

        #region Private Properties

        private Rigidbody _body;
        private String _horizontalString;
        private String _jumpString;

        #endregion

        #region Private Methods

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
            started = false;
            _horizontalString = String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, gamepadIndex);
            _jumpString = String.Format("{0}{1}", Inputs.JUMP, gamepadIndex);

        }

        private void Update()
        {
            if (started)
            {
                if (Input.GetButtonDown(_jumpString))
                {
                    _body.velocity = new Vector3(_body.velocity.x, _body.velocity.y/*0?*/, speedVertical);
                }

                if (Input.GetAxis(_horizontalString) != 0)
                {
                    _body.velocity = new Vector3(speedHorizontal * Input.GetAxis(_horizontalString), _body.velocity.y/*0?*/, _body.velocity.z);

                }
            }

        }

        #endregion

        #region Public Methods

        public virtual void OnPlayStepSound(AnimationEvent animationEvent)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(audioSteps[UnityEngine.Random.Range(0, audioSteps.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        #endregion

    }
}