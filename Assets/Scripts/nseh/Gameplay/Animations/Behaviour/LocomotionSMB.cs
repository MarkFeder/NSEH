using nseh.Gameplay.Entities.Player;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class LocomotionSMB : StateMachineBehaviour
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

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_playerInfo.LightAttackPressed)
                animator.SetTrigger("Attack_A");
            else if (_playerInfo.HeavyAttackPressed)
                animator.SetTrigger("Attack_B");
        }

        #endregion

    }
}
