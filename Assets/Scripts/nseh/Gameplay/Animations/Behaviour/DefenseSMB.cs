﻿using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class DefenseSMB : BaseStateMachineBehaviour
    {
        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // On entering this state, disable player's movement component
            _playerInfo.PlayerMovement.DisableMovement(0.2f);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_action != null && _action.ButtonHasBeenReleased())
            {
                _action.StopAction();
            }
        }

        #endregion
    }
}
