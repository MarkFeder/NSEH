using UnityEngine;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class DeathSMB : StateMachineBehaviour
    {

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            PlayerMovement _playerMovement;

            _playerMovement = animator.GetComponent<PlayerMovement>();
            GameManager.Instance.StartCoroutine(_playerMovement.DisableMovementDeath(0));
        }

        #endregion

    }
}
