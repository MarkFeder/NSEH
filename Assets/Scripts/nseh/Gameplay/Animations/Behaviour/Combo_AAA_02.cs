﻿using UnityEngine;
using System.Collections;
using nseh.Utils;
using nseh.Utils.Helpers;
using nseh.Gameplay.Base.Abstract;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class Combo_AAA_02 : StateMachineBehaviour
    {

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var component = animator.gameObject.GetSafeComponent<CharacterCombat>();
            var targetEnemy = component.TargetEnemy;
            var action = component.GetCharacterAttack(stateInfo.shortNameHash);

            if (action != null && (action.KeyHasBeenPressed() || action.ButtonHasBeenPressed()))
            {
                animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_01), false);
                animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_02), false);
                animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_03), true);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Animator.StringToHash(Constants.Animations.Combat.CHARACTER_COMBO_AAA_02), false);
        }

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    } 
}
