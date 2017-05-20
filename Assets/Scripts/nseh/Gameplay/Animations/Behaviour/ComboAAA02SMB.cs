using System.Linq;
using nseh.Gameplay.Base.Abstract.Animations;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using UnityEngine;

namespace nseh.Gameplay.Animations.Behaviour
{
    public class ComboAAA02SMB : BaseStateMachineBehaviour
    {
		#region Private Properties

		[SerializeField]
		private AttackType _nextActionType;
		private IAction _nextAction;

		#endregion

		#region Public Methods

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboAAA02Hash, stateInfo.normalizedTime);

            _nextAction = _playerInfo.PlayerCombat.Actions.OfType<CharacterAttack>().Where(act =>
            {
	            return act.IsEnabled &&
        	           act.ButtonHasBeenPressed() &&
        	           act.AttackType == _nextActionType;

            }).FirstOrDefault();
            if (_nextAction != null)
            {
                _nextAction.StartAction();
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.SetFloat(_playerInfo.TimeComboAAA02Hash, 0.0F);
            _action.StopAction();
        }

        #endregion
    } 
}
