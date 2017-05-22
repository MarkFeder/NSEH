using nseh.Gameplay.Base.Abstract.Animations;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class LocomotionSMB : BaseStateMachineBehaviour
    {
        #region Private Properties

        private IAction _nextAction;

        #endregion

        #region Public Methods

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            ClearParams(ref animator);
            _playerInfo.PlayerMovement.EnableMovement();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            _nextAction = _playerInfo.PlayerCombat.Actions.OfType<CharacterAttack>().Where(action =>
            {
                return action.IsEnabled &&
                       action.IsSimpleAttack && 
                       action.ButtonHasBeenPressed();

            }).FirstOrDefault();

            if (_nextAction != null)
            {
                _nextAction.StartAction();
            }
        }

        #endregion

        #region Private Methods

        private void ClearParams(ref Animator animator)
        {
            IEnumerator<CharacterAttack> enumerator = _playerInfo.PlayerCombat.Actions.OfType<CharacterAttack>().GetEnumerator();
            while (enumerator.MoveNext())
            {
                animator.ResetTrigger(enumerator.Current.Hash);
            }
        }

        #endregion
    }
}
