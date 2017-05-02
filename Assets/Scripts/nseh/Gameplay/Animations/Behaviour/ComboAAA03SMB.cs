﻿using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA03SMB : BaseStateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboAAA01Hash, 0.0F);
            animator.SetFloat(_playerInfo.TimeComboAAA02Hash, 0.0F);
            animator.ResetTrigger(_playerInfo.ComboAAA03Hash);
        }
    }
}
