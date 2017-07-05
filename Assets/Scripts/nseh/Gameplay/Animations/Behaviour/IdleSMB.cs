using UnityEngine;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class IdleSMB : StateMachineBehaviour
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
            _playerCombat._currentAttack = PlayerCombat.Attack.None;
            _playerInfo.PlayerMovement.EnableMovement();
            //_playerInfo.LightAttackPressed = false;
            //_playerInfo.HeavyAttackPressed = false;

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_playerInfo.LightAttackPressed)
                animator.SetTrigger("Attack_A");
            else if (_playerInfo.HeavyAttackPressed)
                animator.SetTrigger("Attack_B");
            else if (_playerInfo.DefinitivePressed && _playerInfo.CanUseEnergyForDefinitive())
                animator.SetTrigger("Definitive");
            else if (_playerInfo.AbilityPressed && _playerInfo.CanUseEnergyForAbility())
                animator.SetTrigger("Ability");
            else if (_playerInfo.DefensePressed)
                animator.SetTrigger("Defense");
        }

        #endregion

    }
}
