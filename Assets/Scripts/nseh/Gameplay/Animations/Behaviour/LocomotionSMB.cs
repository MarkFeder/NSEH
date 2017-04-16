using nseh.Gameplay.Base.Abstract.Animations;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class LocomotionSMB : BaseStateMachineBehaviour
    {
        private IAction nextAction;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            ClearParams(ref animator);
            this.playerInfo.PlayerMovement.EnableMovement();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            this.nextAction = this.playerInfo.PlayerCombat.Actions.Where(action => action.ButtonHasBeenPressed()).FirstOrDefault();
            if (nextAction != null && nextAction.ButtonHasBeenPressed())
            {
                this.nextAction.DoAction();
            }
        }

        private void ClearParams(ref Animator animator)
        {
            IEnumerator<CharacterAttack> enumerator = this.playerInfo.PlayerCombat.Actions.OfType<CharacterAttack>().GetEnumerator();
            while(enumerator.MoveNext())
            {
                animator.ResetTrigger(enumerator.Current.HashAnimation);
            }
        }
    }
}
