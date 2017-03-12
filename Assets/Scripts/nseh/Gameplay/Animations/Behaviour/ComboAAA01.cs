using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using nseh.Utils;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA01 : StateMachineBehaviour
    {
        private PlayerCombat component;
        private IAction action;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.component = animator.gameObject.GetSafeComponent<PlayerCombat>();
            this.action = component.Actions.Where(act => act.HashAnimation == stateInfo.shortNameHash).FirstOrDefault();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (action != null && action.KeyHasBeenPressed() || action.ButtonHasBeenPressed())
            {
                animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_01), false);
                animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_03), false);
                animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_02), true);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_01), false);
        }
    }

}