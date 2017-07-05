using nseh.Gameplay.Entities.Player;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class DefenseSMB : StateMachineBehaviour
    {

        #region Private Properties

        PlayerInfo _playerInfo;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _playerInfo = animator.GetComponent<PlayerInfo>();
            _playerInfo.PlayerMovement.DisableMovement(0.2f);
            _playerInfo.HealthMode = HealthMode.Defense;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            _playerInfo.HealthMode = HealthMode.Normal;
        }

        #endregion

    }
}
