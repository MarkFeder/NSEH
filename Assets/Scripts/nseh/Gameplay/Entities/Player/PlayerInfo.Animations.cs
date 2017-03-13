using UnityEngine;
using MoveAnimParameters = nseh.Utils.Constants.Animations.Movement;
using CombatAnimParameters= nseh.Utils.Constants.Animations.Combat;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        #region Public C# Properties

        // Movement - StateName

        public string HorizontalStateName { get { return MoveAnimParameters.H; } }
        public string GroundedStateName { get { return MoveAnimParameters.GROUNDED; } }
        public string SpeedStateName { get { return MoveAnimParameters.SPEED; } }
        public string LocomotionStateName { get { return MoveAnimParameters.LOCOMOTION; } }
        public string IdleStateName { get { return MoveAnimParameters.IDLE; } }
        public string IdleJumpStateName { get { return MoveAnimParameters.IDLE_JUMP; } }
        public string LocomotionJumpStateName { get { return MoveAnimParameters.LOCOMOTION_JUMP; } }

        // Combat - StateName

        public string ComboAAA01StateName { get { return CombatAnimParameters.CHARACTER_COMBO_AAA_01; } }
        public string ComboAAA02StateName { get { return CombatAnimParameters.CHARACTER_COMBO_AAA_02; } }
        public string ComboAAA03StateName { get { return CombatAnimParameters.CHARACTER_COMBO_AAA_03; } }
        public string ComboBB01StateName { get { return CombatAnimParameters.CHARACTER_COMBO_BB_01; } }
        public string ComboBB02StateName { get { return CombatAnimParameters.CHARACTER_COMBO_BB_02; } }
        public string DefinitiveStateName { get { return CombatAnimParameters.CHARACTER_COMBO_BB_02; } }
        public string HabilityStateName { get { return CombatAnimParameters.CHARACTER_COMBO_BB_02; } }
        public string DefenseStateName { get { return CombatAnimParameters.CHARACTER_COMBO_BB_02; } }
        public string ImpactStateName { get { return CombatAnimParameters.CHARACTER_IMPACT; } }

        // Movement - Hash

        public int HorizontalHash { get { return Animator.StringToHash(MoveAnimParameters.H); } }
        public int GroundedHash { get { return Animator.StringToHash(MoveAnimParameters.GROUNDED);  } }
        public int SpeedHash { get { return Animator.StringToHash(MoveAnimParameters.SPEED); } }
        public int LocomotionHash { get { return Animator.StringToHash(MoveAnimParameters.LOCOMOTION); } }
        public int IdleHash { get { return Animator.StringToHash(MoveAnimParameters.IDLE); } }
        public int IdleJumpHash { get { return Animator.StringToHash(MoveAnimParameters.IDLE_JUMP); } }
        public int LocomotionJumpHash { get { return Animator.StringToHash(MoveAnimParameters.LOCOMOTION_JUMP); } }

        // Combat - Hash

        public int ComboAAA01Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_AAA_01); } }
        public int ComboAAA02Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_AAA_02); } }
        public int ComboAAA03Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_AAA_03); } }
        public int ComboBB01Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_BB_01); } }
        public int ComboBB02Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_BB_02); } }
        public int DefinitiveHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_BB_02); } }
        public int HabilityHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_BB_02); } }
        public int DefenseHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_BB_02); } }
        public int ImpactHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_IMPACT); } }

        #endregion
    }
}
