using nseh.Gameplay.Entities.Player;
using UnityEngine;
using nseh.Managers.Main;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class DefinitiveSMB : StateMachineBehaviour
    {

        #region Private Properties

        PlayerInfo _playerInfo;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _playerInfo = animator.GetComponent<PlayerInfo>();
            GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.DisableMovement(0.2f));
            _playerInfo.HealthMode = HealthMode.Invulnerability;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            _playerInfo.HealthMode = HealthMode.Normal;
        }

        #endregion

    }
}