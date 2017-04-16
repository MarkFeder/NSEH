using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Base.Abstract.Animations
{
    public abstract class BaseStateMachineBehaviour : StateMachineBehaviour
    {
        protected PlayerInfo playerInfo;
        protected IAction action;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            this.playerInfo = animator.gameObject.GetSafeComponent<PlayerInfo>();
            this.action = this.playerInfo.PlayerCombat.Actions.Where(act => act.HashAnimation == stateInfo.shortNameHash).FirstOrDefault();
        }
    }
}
