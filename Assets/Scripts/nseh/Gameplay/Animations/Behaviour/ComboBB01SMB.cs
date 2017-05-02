using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboBB01SMB : BaseStateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // On entering this state, disable player's movement component
            _playerInfo.PlayerMovement.DisableMovement();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboBB01Hash, stateInfo.normalizedTime);
            if (_action != null && _action.ButtonHasBeenPressed())
            {
                animator.SetTrigger(_playerInfo.ComboBB02Hash);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboBB01Hash, 0.0F);
            animator.ResetTrigger(_playerInfo.ComboBB01Hash);
        }
    } 
}
