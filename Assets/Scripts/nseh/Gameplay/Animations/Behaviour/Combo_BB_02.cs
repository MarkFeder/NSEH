using nseh.Utils;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class Combo_BB_02 : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_BB_01), false);
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_BB_02), false);
        }
    } 
}
