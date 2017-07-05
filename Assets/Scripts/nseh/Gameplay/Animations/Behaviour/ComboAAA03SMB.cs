using UnityEngine;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA03SMB : StateMachineBehaviour
    {

        #region Private Properties

        PlayerCombat _playerCombat;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _playerCombat = animator.GetComponent<PlayerCombat>();
            _playerCombat._currentAttack = PlayerCombat.Attack.A3;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.speed = 1;
        }

        #endregion

    }
}
