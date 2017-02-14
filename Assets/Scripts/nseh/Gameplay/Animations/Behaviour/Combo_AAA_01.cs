using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Interfaces;
using nseh.Utils;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class Combo_AAA_01 : StateMachineBehaviour
    {
        private CharacterCombat component;
        private IAction action;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.component = animator.gameObject.GetSafeComponent<CharacterCombat>();
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

        //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    var step1 = animator.gameObject.GetComponent<PaladinComboAAAStep1>();
        //    var step2 = animator.gameObject.GetComponent<PaladinComboAAAStep2>();
        //    var step3 = animator.gameObject.GetComponent<PaladinComboAAAStep3>();

        //    if ((step1 != null && step2 != null && step3 != null) && (step2.KeyHasBeenPressed() || step2.ButtonHasBeenPressed()))
        //    {
        //        step2.DoAction();
        //        step1.StopAction();
        //        step3.StopAction();
        //    }
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_01), false);
        }
    }

}