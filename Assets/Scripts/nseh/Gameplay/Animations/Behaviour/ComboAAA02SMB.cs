using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA02SMB : BaseStateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboAAA02Hash, stateInfo.normalizedTime);
            if (_action != null && _action.ButtonHasBeenPressed())
            {
                animator.SetTrigger(_playerInfo.ComboAAA03Hash);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboAAA02Hash, 0.0F);
            animator.ResetTrigger(_playerInfo.ComboAAA02Hash);
        }
    } 
}
