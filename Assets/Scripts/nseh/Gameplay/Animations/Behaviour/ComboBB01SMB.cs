using UnityEngine;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboBB01SMB : StateMachineBehaviour
    {

        #region Private Properties

        PlayerInfo _playerInfo;
        PlayerCombat _playerCombat;
        bool _B2;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            
            _playerInfo = animator.GetComponent<PlayerInfo>();
            animator.speed += (float)(_playerInfo.CurrentAgility *0.025 * animator.speed);
            _playerCombat = animator.GetComponent<PlayerCombat>();
            _playerCombat._currentAttack = PlayerCombat.Attack.B1;
            GameManager.Instance.StartCoroutine(_playerInfo.PlayerMovement.DisableMovement(0.2f));
            _B2 = false;

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_playerInfo.HeavyAttackPressed)
                _B2 = true;

            if(_B2 && stateInfo.normalizedTime >= 0.95)
            {
                animator.SetTrigger("Combo_BB_02");
                _B2 = false;
            }           
        }

        #endregion

    } 
}
