using System.Linq;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using UnityEngine;

namespace nseh.Gameplay.Base.Abstract.Animations
{
    public abstract class BaseStateMachineBehaviour : StateMachineBehaviour
    {
        #region Protected Properties

        protected PlayerInfo _playerInfo;
        protected IAction _action; 

        #endregion

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _playerInfo = animator.gameObject.GetSafeComponent<PlayerInfo>();
            _action = _playerInfo.PlayerCombat.Actions.Where(act => act.Hash == stateInfo.shortNameHash).FirstOrDefault();
        }
    }
}
