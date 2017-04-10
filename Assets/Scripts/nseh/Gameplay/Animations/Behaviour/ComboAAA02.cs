using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA02 : StateMachineBehaviour
    {
        private PlayerInfo playerInfo;
        private IAction action;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.playerInfo = animator.gameObject.GetSafeComponent<PlayerInfo>();
            this.action = this.playerInfo.PlayerCombat.Actions.Where(act => act.HashAnimation == stateInfo.shortNameHash).FirstOrDefault();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (action != null && (action.KeyHasBeenPressed() || action.ButtonHasBeenPressed()) && stateInfo.normalizedTime >= 0.65)
            {
                animator.SetBool(this.playerInfo.ComboAAA01Hash, false);
                animator.SetBool(this.playerInfo.ComboAAA02Hash, false);
                animator.SetBool(this.playerInfo.ComboAAA03Hash, true);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(this.playerInfo.ComboAAA02Hash, false);
        }
    } 
}
