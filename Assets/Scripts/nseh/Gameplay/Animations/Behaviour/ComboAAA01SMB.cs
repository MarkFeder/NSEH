using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA01SMB : StateMachineBehaviour
    {

        #region Private Properties

        PlayerInfo _playerInfo;
        PlayerCombat _playerCombat;
        bool _A2;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _playerInfo = animator.GetComponent<PlayerInfo>();
            animator.speed += (float)(_playerInfo.CurrentAgility * 0.025);
            _playerCombat = animator.GetComponent<PlayerCombat>();
            _playerCombat._currentAttack = PlayerCombat.Attack.A1;
            GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.DisableMovement(0.2f));
            _A2 = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_playerInfo.LightAttackPressed)
                _A2 = true;

            if (_A2 && stateInfo.normalizedTime >= 0.95)
            {
                animator.SetTrigger("Combo_AAA_02");
                _A2 = false;
            }
        }

        #endregion
    }
}