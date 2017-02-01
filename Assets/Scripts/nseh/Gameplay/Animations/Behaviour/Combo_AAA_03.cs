using UnityEngine;
using System.Collections;
using nseh.Utils;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class Combo_AAA_03 : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_01), false);
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_02), false);
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_03), false);
        }
    }
}
