using nseh.Gameplay.Entities.Player;
using UnityEngine;
using nseh.Managers.Main;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class HurtSMB : StateMachineBehaviour
    {

        #region Private Properties

        PlayerInfo _playerInfo;
        PlayerCombat _playerCombat;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            animator.speed = 1;
            _playerInfo = animator.GetComponent<PlayerInfo>();
            _playerCombat = animator.GetComponent<PlayerCombat>();
            _playerCombat.DesactivateAllColliders();
            GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.DisableMovement(0.2f));
        }

        #endregion

    }
}
