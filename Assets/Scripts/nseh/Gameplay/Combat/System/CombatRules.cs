using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace nseh.Gameplay.Combat.System
{
    public static class CombatRules
    {
        #region Private Methods

        private static int ConvertAttackTypeToIndex(AttackType attackType)
        {
            int toReturn = -1;

            switch (attackType)
            {
                // None
                case AttackType.None:
                    toReturn = -1;
                    break;
                // A
                case AttackType.CharacterAttackAStep1:
                    toReturn = 1;
                    break;
                // B
                case AttackType.CharacterAttackBStep1:
                    toReturn = 2;
                    break;
                // Combo B#
                case AttackType.CharacterAttackBSharp:
                    toReturn = 3;
                    break;
                // Combo AAA
                case AttackType.CharacterAttackAStep2:
                    toReturn = 4;
                    break;
                case AttackType.CharacterAttackAStep3:
                    toReturn = 4;
                    break;
                // Combo BB
                case AttackType.CharacterAttackBStep2:
                    toReturn = 5;
                    break;
                // Hability
                case AttackType.CharacterHability:
                    toReturn = 6;
                    break;
                // Definitive
                case AttackType.CharacterDefinitive:
                    toReturn = 7;
                    break;
            }

            if (!(toReturn >= -1 && toReturn <= 7))
            {
                // Defense
                toReturn = 8;
            }

            return toReturn;
        }

        // See GDD Documentation to understand conflictsTable
        // ** Ignore index of first column and row
        // -1: Cancel; both attacks do not take effect
        //  0: None; both attacks take effect without interrumption
        //  1: Attack a wins and cancels b
        //  2: Attack b wins and cancels a
        //  3: Both recieves attack (a or b) and cancel combo
        private static int[,] conflictsTable = new int[,]
        {
            {99, 0, 1, 2, 3, 4, 5, 6, 7 },
            {0, -1, 2, 1, 3, 3, 0, 0, 0 },
            {1, 2, -1, 2, 3, 3, 0, 0, 0 },
            {2, 1, 2, -1, 3, 3, 0, 0, -1 },
            {3, 3, 3, 3, 3, 4, 0, 0, 0 },
            {4, 3, 3, 3, 4, 3, 0, 0, -1 },
            {5, 0, 0, 0, 0, 0, 0, 0, 0 },
            {6, 0, 0, 0, 0, 0, 0, 0, 0 },
            {7, 0, 0, -1, 0, -1, 0, -1, 0 }
        };

        private static Dictionary<int, string> conflictToStr = new Dictionary<int, string>()
        {
            { -1, "Cancel" },
            { 0, "None" },
            { 1, "A over B" },
            { 2, "B over A" },
            { 3, "A and B" },
        };

        #endregion

        #region Public Methods

        public static void PrintConflictInfo(ref string color, ref int conflict, ref string senderName, ref string enemyName)
        {
            // Convert index into string to improve readability
            Debug.Log(String.Format("<color={0}> The conflict is: \"{1}\" between {2} and {3} </color>", color, conflictToStr[conflict], senderName, enemyName));
        }

        public static int CompareAttacks(ref HandledAction attackA, ref HandledAction attackB)
        {
            if (attackA is CharacterAttack && attackB is CharacterAttack)
            {
                CharacterAttack a = attackA as CharacterAttack;
                CharacterAttack b = attackB as CharacterAttack;

                int a_int = CombatRules.ConvertAttackTypeToIndex(a.AttackType);
                int b_int = CombatRules.ConvertAttackTypeToIndex(b.AttackType);

                // Access conflict inside the table - O(1) complexity
                return conflictsTable[a_int, b_int];
            }
            else if (attackA is CharacterAttack && attackB is CharacterDefense)
            {
                CharacterAttack a = attackA as CharacterAttack;

                int a_int = CombatRules.ConvertAttackTypeToIndex(a.AttackType);

                return conflictsTable[a_int, 8];
            }
            else if (attackA is CharacterDefense && attackB is CharacterAttack)
            {
                CharacterAttack b = attackB as CharacterAttack;

                int b_int = CombatRules.ConvertAttackTypeToIndex(b.AttackType);

                return conflictsTable[8, b_int];
            }
            else if (attackA is CharacterDefense && attackB is CharacterDefense)
            {
                return conflictsTable[8, 8];
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
