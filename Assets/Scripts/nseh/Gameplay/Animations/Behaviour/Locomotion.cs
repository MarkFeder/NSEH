using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class Locomotion : StateMachineBehaviour
    {
        private PlayerInfo playerInfo;
        private IAction nextAction;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.playerInfo = animator.gameObject.GetSafeComponent<PlayerInfo>();
            ClearAllAttacks(ref animator);

            // Enable movement again after playing combo
            this.playerInfo.PlayerMovement.EnableMovement();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.nextAction = this.playerInfo.PlayerCombat.Actions.Where(action => action.ButtonHasBeenPressed()).FirstOrDefault();

            if (nextAction != null && nextAction.ButtonHasBeenPressed())
            {
                this.nextAction.DoAction();
            }
        }

        private void ClearAllAttacks(ref Animator animator)
        {
            IEnumerator<CharacterAttack> enumerator = this.playerInfo.PlayerCombat.Actions.OfType<CharacterAttack>().GetEnumerator();
            while(enumerator.MoveNext())
            {
                animator.ResetTrigger(enumerator.Current.HashAnimation);
            }
        }
    }
}
