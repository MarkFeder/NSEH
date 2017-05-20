using nseh.Gameplay.Base.Abstract.Animations;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class DefinitiveSMB : BaseStateMachineBehaviour
    {
        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // On entering this state, disable player's movement component
            _playerInfo.PlayerMovement.DisableMovement();
        }

        #endregion
    }
}