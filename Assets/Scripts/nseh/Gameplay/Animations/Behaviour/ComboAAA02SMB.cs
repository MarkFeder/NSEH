using nseh.Gameplay.Entities.Player;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA02SMB : StateMachineBehaviour
    {
		
        #region Private Properties

        PlayerInfo _playerInfo;
        PlayerCombat _playerCombat;
        bool _A3;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _A3 = false;
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _playerInfo = animator.GetComponent<PlayerInfo>();
            _playerCombat = animator.GetComponent<PlayerCombat>();
            _playerCombat._currentAttack = PlayerCombat.Attack.A2;
            
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_playerInfo.LightAttackPressed)
                _A3 = true;

            if (_A3 && stateInfo.normalizedTime >= 0.7)
            {
                animator.SetTrigger("Combo_AAA_03");
                _A3 = false;
            }
        }

        #endregion

    }
}