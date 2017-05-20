using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboBB02SMB : BaseStateMachineBehaviour
    {
        #region Public Methods

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboAAA01Hash, 0.0F);
            animator.SetFloat(_playerInfo.TimeComboAAA02Hash, 0.0F);
            _action.StopAction();
        }

        #endregion
    } 
}
