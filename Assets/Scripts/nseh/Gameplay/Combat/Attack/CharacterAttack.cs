using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat.Defense;
using nseh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Colors = nseh.Utils.Constants.Colors;
using Constants = nseh.Utils.Constants.Animations.Combat;

namespace nseh.Gameplay.Combat
{
    public enum AttackType
    {
        None = 0,
        CharacterAttackAStep1 = 1,
        CharacterAttackAStep2 = 2,
        CharacterAttackAStep3 = 3,
        CharacterAttackBStep1 = 4,
        CharacterAttackBStep2 = 5,
        CharacterAttackBSharp = 6,
        CharacterHability = 7,
        CharacterDefinitive = 8,
    }

    public class CharacterAttack : HandledAction
    {
        public float Damage { get; set; }
        public AttackType AttackType { get; private set; }

        public CharacterAttack(AttackType attackType, int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None, 
            string buttonToPress = null, 
            float damage = 0.0f) 
            : base(hashAnimation, stateName, animator)
        {
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
            this.Damage = damage;
            this.AttackType = attackType;
        }

        public override void DoAction()
        {
            base.DoAction();
        }

        public void PerformDamage(GameObject sender, GameObject enemy)
        {
            if (sender != null && enemy != null)
            {
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)this.Damage);
            }
            else
            {
                Debug.Log("Sender or enemy is null");
            }
        }

        public void PerformDamage(GameObject sender, List<GameObject> targetEnemies)
        {
            if (targetEnemies != null && targetEnemies.Count > 0)
            {
                targetEnemies.ForEach(obj =>
                {
                    // Check if sender is not the enemy
                    if (sender != obj)
                    {
                        // Check combat system rules
                        var enemyAction = obj.GetSafeComponent<CharacterCombat>().CurrentAction;

                        if (enemyAction != null)
                        {
                            int conflict = CombatRules.CompareAttacks(this, enemyAction);

                            var enemyAttack = enemyAction as CharacterAttack;

                            this.ResolveConflict(conflict, ref sender, ref obj, ref enemyAttack);
                        }
                        else
                        {
                            // Enemy is not taking any action
                            obj.GetSafeComponent<CharacterHealth>().TakeDamage((int)this.Damage);
                        }
                    }
                });
            }
            else
            {
                Debug.Log("TargetEnemies is null");
            }
        }

        // We should move this function to Combact rules
        private void ResolveConflict(int conflict, ref GameObject sender, ref GameObject enemy, ref CharacterAttack enemyAction)
        {
            var enemyAttack = enemyAction as CharacterAttack;

            if (conflict == -1)
            {
                // Cancel both attacks; do not take any damage effect
                enemyAttack.Animator.SetTrigger(Animator.StringToHash(Constants.CHARACTER_IMPACT));
                this.Animator.SetTrigger(Animator.StringToHash(Constants.CHARACTER_IMPACT));
            }
            else if (conflict == 0)
            {
                // Both attacks take effect normally without interrumption
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)this.Damage);
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAttack.Damage);
            }
            else if (conflict == 1)
            {
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)this.Damage);
                // Cancel a's action
                this.Animator.SetTrigger(Animator.StringToHash(Constants.CHARACTER_IMPACT));
            }
            else if (conflict == 2)
            {
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAttack.Damage);
                // Cancel b's action
                // It should be sth related to CharacterCombat but we have the animator itself there
                enemyAttack.Animator.SetTrigger(Animator.StringToHash(Constants.CHARACTER_IMPACT));
            }
            else if (conflict == 3)
            {
                // Both receives action's damage
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAttack.Damage);
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)this.Damage);

                // Cancel both a and b
                this.Animator.SetTrigger(Animator.StringToHash(Constants.CHARACTER_IMPACT));
            }
            else
            {
                Debug.Log(string.Format("No conflict for characters: {0} and {1}", enemy.name, sender.name));
            }

            // TODO: Apply force to gameobject
            // TODO: We should store hashAnimations somewhere

            // Print debug info to log
            // See: http://stackoverflow.com/questions/1810785/why-cant-i-pass-a-property-or-indexer-as-a-ref-parameter-when-net-reflector-sh
            string senderName = sender.name;
            string enemyName = enemy.name;
            string colorDebug = Colors.BROWN;

            CombatRules.PrintConflictInfo(ref colorDebug, ref conflict, ref senderName, ref enemyName);
        }
    }

    public static class CombatRules
    {
        public static int ConvertAttackTypeToIndex(AttackType attackType)
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

        public static void PrintConflictInfo(ref string color, ref int conflict, ref string senderName, ref string enemyName)
        {
            // Convert index into string to improve readability
            Debug.Log(String.Format("<color={0}> The conflict is: \"{1}\" between {2} and {3} </color>", color, conflictToStr[conflict], senderName, enemyName));
        }

        public static int CompareAttacks(HandledAction attackA, HandledAction attackB)
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
    }
}
