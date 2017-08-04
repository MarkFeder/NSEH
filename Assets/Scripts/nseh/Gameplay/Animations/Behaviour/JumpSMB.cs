using nseh.Gameplay.Entities.Player;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class JumpSMB : StateMachineBehaviour
    {

        #region Private Properties

        PlayerInfo _playerInfo;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _playerInfo = animator.GetComponent<PlayerInfo>();
            _playerInfo.PlayerMovement.EnableMovement();
        }

        #endregion

    }
}
