using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using UnityEngine;
using CombatAnimParameters = nseh.Utils.Constants.Animations.Combat;
using MoveAnimParameters = nseh.Utils.Constants.Animations.Movement;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        #region Public C# Properties

        // TODO: make states and hashes as private and assign them on Start

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
        public string DefinitiveStateName { get { return CombatAnimParameters.CHARACTER_DEFINITIVE; } }
        public string HabilityStateName { get { return CombatAnimParameters.CHARACTER_HABILITY; } }
        public string DefenseStateName { get { return CombatAnimParameters.CHARACTER_DEFENSE; } }
        public string ImpactStateName { get { return CombatAnimParameters.CHARACTER_IMPACT; } }
        public string DeadStateName { get { return CombatAnimParameters.CHARACTER_DEAD; } }
        public string TakeDamageStateName { get { return CombatAnimParameters.CHARACTER_TAKE_DAMAGE; } }


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
        public int DefinitiveHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_DEFINITIVE); } }
        public int HabilityHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_HABILITY); } }
        public int DefenseHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_DEFENSE); } }
        public int ImpactHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_IMPACT); } }
        public int TakeDamageHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_TAKE_DAMAGE); } }
        public int DeadHash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_DEAD); } }
        public int TimeComboAAA01Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_AAA_01_TIME); } }
        public int TimeComboAAA02Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_AAA_02_TIME); } }
        public int TimeComboBB01Hash { get { return Animator.StringToHash(CombatAnimParameters.CHARACTER_COMBO_BB_01_TIME); } }

        #endregion

        #region Public Methods

        public int GetHash(AttackType type)
        {
            int hash = -1;

            switch (type)
            {
                case AttackType.None:
                    hash = -1;
                    break;

                case AttackType.CharacterAttackAStep1:
                    hash = ComboAAA01Hash;
                    break;

                case AttackType.CharacterAttackAStep2:
                    hash = ComboAAA02Hash;
                    break;

                case AttackType.CharacterAttackAStep3:
                    hash = ComboAAA03Hash;
                    break;

                case AttackType.CharacterAttackBStep1:
                    hash = ComboBB01Hash;
                    break;

                case AttackType.CharacterAttackBStep2:
                    hash = ComboBB02Hash;
                    break;

                case AttackType.CharacterAttackBSharp:
                    hash = -1;
                    break;

                case AttackType.CharacterHability:
                    hash = HabilityHash;
                    break;

                case AttackType.CharacterDefinitive:
                    hash = DefinitiveHash;
                    break;
            }

            return hash;
        }

        public int GetHash(DefenseType type)
        {
            int hash = -1;

            switch (type)
            {
                case DefenseType.None:
                    hash = -1;
                    break;

                case DefenseType.NormalDefense:
                    hash = DefenseHash;
                    break;
            }

            return hash;
        }

        public string GetStateNameInfo(AttackType type)
        {
            string nameInfo = null;

            switch (type)
            {
                case AttackType.None:
                    nameInfo = null;
                    break;

                case AttackType.CharacterAttackAStep1:
                    nameInfo = ComboAAA01StateName;
                    break;

                case AttackType.CharacterAttackAStep2:
                    nameInfo = ComboAAA02StateName;
                    break;

                case AttackType.CharacterAttackAStep3:
                    nameInfo = ComboAAA03StateName;
                    break;

                case AttackType.CharacterAttackBStep1:
                    nameInfo = ComboBB01StateName;
                    break;

                case AttackType.CharacterAttackBStep2:
                    nameInfo = ComboBB02StateName;
                    break;

                case AttackType.CharacterAttackBSharp:
                    nameInfo = null;
                    break;

                case AttackType.CharacterHability:
                    nameInfo = HabilityStateName;
                    break;

                case AttackType.CharacterDefinitive:
                    nameInfo = DefinitiveStateName;
                    break;
            }

            return nameInfo;
        }

        public string GetStateNameInfo(DefenseType type)
        {
            string nameInfo = null;

            switch (type)
            {
                case DefenseType.None:
                    nameInfo = null;
                    break;

                case DefenseType.NormalDefense:
                    nameInfo = DefenseStateName;
                    break;
            }

            return nameInfo;
        }

        #endregion
    }
}
