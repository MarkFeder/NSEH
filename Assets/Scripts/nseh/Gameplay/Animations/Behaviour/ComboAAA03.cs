using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA03 : StateMachineBehaviour
    {
        private PlayerInfo playerInfo;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            this.playerInfo = animator.gameObject.GetSafeComponent<PlayerInfo>();
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(this.playerInfo.ComboAAA01Hash, false);
            animator.SetBool(this.playerInfo.ComboAAA02Hash, false);
            animator.SetBool(this.playerInfo.ComboAAA03Hash, false);
        }
    }
}
