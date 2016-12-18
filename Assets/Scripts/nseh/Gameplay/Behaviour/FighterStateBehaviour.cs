using UnityEngine;
using System.Collections;

namespace nseh.Gameplay
{
    public class FighterStateBehaviour : StateMachineBehaviour
    {
        public FighterStates behaviourState;

        public float horizontalForce;
        public float verticalForce;

        protected Fighter fighter;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (fighter == null)
            {
                fighter = animator.gameObject.GetComponent<Fighter>();
            }

            fighter.CurrentState = behaviourState;

            fighter.Body.AddRelativeForce(new Vector3(0, verticalForce, 0));
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            fighter.Body.AddRelativeForce(new Vector3(0, 0, horizontalForce));
        }
    } 
}
