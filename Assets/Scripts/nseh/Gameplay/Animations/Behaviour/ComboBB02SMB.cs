using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboBB02SMB : BaseStateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(this.playerInfo.TimeComboAAA01Hash, 0.0F);
            animator.SetFloat(this.playerInfo.TimeComboAAA02Hash, 0.0F);
            animator.ResetTrigger(this.playerInfo.ComboBB02Hash);
        }
    } 
}
