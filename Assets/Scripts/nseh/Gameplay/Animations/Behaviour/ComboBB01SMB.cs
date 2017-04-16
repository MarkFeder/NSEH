﻿using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboBB01SMB : BaseStateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // On entering this state, disable player's movement component
            this.playerInfo.PlayerMovement.DisableMovement();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            animator.SetFloat(this.playerInfo.TimeComboBB01Hash, stateInfo.normalizedTime);
            if (action != null && action.ButtonHasBeenPressed())
            {
                animator.SetTrigger(this.playerInfo.ComboBB02Hash);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(this.playerInfo.TimeComboBB01Hash, 0.0F);
            animator.ResetTrigger(this.playerInfo.ComboBB01Hash);
        }
    } 
}
