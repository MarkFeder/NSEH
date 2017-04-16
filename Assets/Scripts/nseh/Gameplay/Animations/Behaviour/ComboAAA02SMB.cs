using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA02SMB : BaseStateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            animator.SetFloat(this.playerInfo.TimeComboAAA02Hash, stateInfo.normalizedTime);
            if (action != null && action.ButtonHasBeenPressed())
            {
                animator.SetTrigger(this.playerInfo.ComboAAA03Hash);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(this.playerInfo.TimeComboAAA02Hash, 0.0F);
            animator.ResetTrigger(this.playerInfo.ComboAAA02Hash);
        }
    } 
}
