using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat.Defense;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.Combat.System
{
    public static class CombatRules
    {
        #region Private Methods

        private static int ConvertAttackTypeToIndex(AttackType attackType)
        {
            int index = -1;

            switch (attackType)
            {
                // None
                case AttackType.None:
                    index = -1;
                    break;
                // A
                case AttackType.CharacterAttackAStep1:
                    index = 1;
                    break;
                // B
                case AttackType.CharacterAttackBStep1:
                    index = 2;
                    break;
                // Combo B#
                case AttackType.CharacterAttackBSharp:
                    index = 3;
                    break;
                // Combo AAA
                case AttackType.CharacterAttackAStep2:
                    index = 4;
                    break;
                case AttackType.CharacterAttackAStep3:
                    index = 4;
                    break;
                // Combo BB
                case AttackType.CharacterAttackBStep2:
                    index = 5;
                    break;
                // Hability
                case AttackType.CharacterHability:
                    index = 6;
                    break;
                // Definitive
                case AttackType.CharacterDefinitive:
                    index = 7;
                    break;
            }

            if (!(index >= -1 && index <= 7))
            {
                // Defense
                index = 8;
            }

            return index;
        }

        // See GDD Documentation to understand conflictsTable
        // ** Ignore index of first column and row (99)
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

        public static int CompareActions(ref HandledAction senderAction, ref HandledAction enemyAction)
        {
            if (senderAction is CharacterAttack && enemyAction is CharacterAttack)
            {
                CharacterAttack a = senderAction as CharacterAttack;
                CharacterAttack b = enemyAction as CharacterAttack;

                int a_int = CombatRules.ConvertAttackTypeToIndex(a.AttackType);
                int b_int = CombatRules.ConvertAttackTypeToIndex(b.AttackType);

                // Access conflict inside the table - O(1) complexity
                return conflictsTable[a_int, b_int];
            }
            else if (senderAction is CharacterAttack && enemyAction is CharacterDefense)
            {
                CharacterAttack a = senderAction as CharacterAttack;

                int a_int = CombatRules.ConvertAttackTypeToIndex(a.AttackType);

                return conflictsTable[a_int, 8];
            }
            else if (senderAction is CharacterDefense && enemyAction is CharacterAttack)
            {
                CharacterAttack b = enemyAction as CharacterAttack;

                int b_int = CombatRules.ConvertAttackTypeToIndex(b.AttackType);

                return conflictsTable[8, b_int];
            }
            else if (senderAction is CharacterDefense && enemyAction is CharacterDefense)
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
