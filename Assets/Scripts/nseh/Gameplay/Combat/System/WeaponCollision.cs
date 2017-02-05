using nseh.Utils.Helpers;
using System;
using UnityEngine;
using System.Collections.Generic;
using nseh.Gameplay.Base.Abstract;
using System.Linq;
using Constants = nseh.Utils.Constants;
using Colors = nseh.Utils.Constants.Colors;
using Tags = nseh.Utils.Constants.Tags;
using Actions = nseh.Utils.Constants.Animations.Combat;
using Movements = nseh.Utils.Constants.Animations.Movement;
using SystemObject = System.Object;
using nseh.Gameplay.Combat.Defense;

namespace nseh.Gameplay.Combat.System
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class WeaponCollision : MonoBehaviour
    {
        // External References
        protected Collider hitBox;
        protected CharacterCombat characterCombat;
        protected CharacterMovement characterMovement;
        protected Animator anim;

        protected List<GameObject> enemyTargets;
        protected GameObject rootCharacter;

        // Properties
        public string enemyType;
        public float sphereRadius = 0.2f;
        public float offset;

        protected string parentObjName;

        private int layerMask;

        public void Awake()
        {
            this.enemyType = Tags.PLAYER;
            
            this.hitBox = GetComponent<Collider>();
            this.hitBox.isTrigger = true;
            this.hitBox.enabled = false;

            this.characterCombat = this.transform.root.GetComponent<CharacterCombat>();
            this.characterMovement = this.transform.root.GetComponent<CharacterMovement>();
            this.anim = this.transform.root.GetComponent<Animator>();
            this.enemyTargets = new List<GameObject>();

            this.parentObjName = this.transform.root.name;
            this.rootCharacter = this.transform.root.gameObject;

            this.layerMask = LayerMask.GetMask("Player");
        }

        #region Private Methods

        private bool EnemyHasBeenTakenAback(ref GameObject enemy)
        {
            CharacterMovement enemyMov = enemy.GetComponent<CharacterMovement>();

            return !(this.characterMovement.IsFacingRight && !enemyMov.IsFacingRight
                    || !this.characterMovement.IsFacingRight && enemyMov.IsFacingRight);
        }

        #endregion

        #region Trigger Methods

        protected void OnTriggerEnter(Collider other)
        {
            GameObject enemy = other.gameObject;

            if (enemy.CompareTag(this.enemyType) 
                && this.parentObjName != enemy.name)
            {
                bool enemyTakenAback = this.EnemyHasBeenTakenAback(ref enemy);

                if (enemyTakenAback)
                {
                    var attack = this.characterCombat.CurrentAction as HandledAction;
                        
                    if (!SystemObject.ReferenceEquals(null, attack))
                    {
                        Debug.Log(String.Format("<color={0}> {1} does the attack: {2}</color>", Colors.FUCHSIA, this.parentObjName, attack.StateName));

                        this.PerformDamage(ref this.rootCharacter, ref attack, ref enemy);
                    }
                }
                else
                {
                    // enemies are watching each other
                    this.enemyTargets.Add(enemy);

                    var attack = this.characterCombat.CurrentAction as HandledAction;

                    if (!SystemObject.ReferenceEquals(null, attack))
                    {
                        Debug.Log(String.Format("<color={0}> {1} does the attack: {2}</color>", Colors.FUCHSIA, this.parentObjName, attack.StateName));

                        this.PerformDamage(ref this.rootCharacter, ref attack, ref this.enemyTargets);
                    }
                }
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            GameObject enemy = other.gameObject;

            if (enemy.CompareTag(this.enemyType) && this.parentObjName != enemy.name)
            {
                this.enemyTargets.Remove(enemy);
            }
        }

        #endregion

        #region Combat System

        public void PerformDamage(ref GameObject sender, ref HandledAction senderAction, ref GameObject enemy)
        {
            if (sender != null && enemy != null)
            {
                int amountDamage = (int)(senderAction as CharacterAttack).Damage;

                enemy.GetSafeComponent<CharacterHealth>().TakeDamage(amountDamage);
            }
            else
            {
                Debug.Log("Some parameter is null");
            }
        }

        public void PerformDamage(ref GameObject sender, ref HandledAction senderAction, ref List<GameObject> targetEnemies)
        {
            if (targetEnemies != null && targetEnemies.Count > 0)
            {
                for (int i = 0; i < targetEnemies.Count(); i++)
                {
                    var enemy = targetEnemies[i];

                    // Check if sender is not the enemy
                    if (!sender.name.Equals(enemy.name))
                    {
                        // Check combat system rules
                        var enemyAction = enemy.GetSafeComponent<CharacterCombat>().CurrentAction as HandledAction;

                        if (!SystemObject.ReferenceEquals(null, enemyAction))
                        {
                            int conflict = CombatRules.CompareActions(ref senderAction, ref enemyAction);

                            this.ResolveConflict(conflict, ref sender, ref senderAction, ref enemy, ref enemyAction);
                        }
                        else
                        {
                            // Enemy is not taking any action
                            int amountDamage = (int)(senderAction as CharacterAttack).Damage;

                            enemy.GetSafeComponent<CharacterHealth>().TakeDamage(amountDamage);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Some parameter is null");
            }
        }

        private void ResolveConflict(int conflict, ref GameObject sender, ref HandledAction senderAction, ref GameObject enemy, ref HandledAction enemyAction)
        {
            // Print debug info to log
            // See: http://stackoverflow.com/questions/1810785/why-cant-i-pass-a-property-or-indexer-as-a-ref-parameter-when-net-reflector-sh
            string senderName = sender.name;
            string enemyName = enemy.name;
            string colorDebug = Colors.BROWN;

            CombatRules.PrintConflictInfo(ref colorDebug, ref conflict, ref senderName, ref enemyName);

            if (senderAction is CharacterAttack && enemyAction is CharacterAttack)
            {
                CharacterAttack enemyAttack = enemyAction as CharacterAttack;
                CharacterAttack senderAttack = senderAction as CharacterAttack;

                this.ResolveConflict(conflict, ref sender, ref senderAttack, ref enemy, ref enemyAttack);
            }
            else if (senderAction is CharacterAttack && enemyAction is CharacterDefense)
            {
                CharacterAttack senderAttack = senderAction as CharacterAttack;
                CharacterDefense enemyDefense = enemyAction as CharacterDefense;

                this.ResolveConflict(conflict, ref sender, ref senderAttack, ref enemy, ref enemyDefense);
            }
            else
            {
                Debug.Log("Check types for senderAction and EnemyAction");
            }
        }

        private void ResolveConflict(int conflict, ref GameObject sender, ref CharacterAttack senderAttack, ref GameObject enemy, ref CharacterDefense enemyDefense)
        {
            if (conflict == -1)
            {
                if (senderAttack.AttackType == AttackType.CharacterAttackBSharp)
                {
                    enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAttack.Damage / 2);
                    enemyDefense.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
                }
                else if (senderAttack.AttackType == AttackType.CharacterAttackBStep2 
                        || senderAttack.AttackType == AttackType.CharacterDefinitive)
                {
                    enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAttack.Damage);
                    enemyDefense.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
                }
            }
            else if (conflict == 0)
            {
                senderAttack.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
            }
            else
            {
                Debug.Log(string.Format("No conflict for characters: {0} and {1}", enemy.name, sender.name));
            }
        }

        private void ResolveConflict(int conflict, ref GameObject sender, ref CharacterAttack senderAction, ref GameObject enemy, ref CharacterAttack enemyAction)
        {
            if (conflict == -1)
            {
                // Cancel both attacks; do not take any damage effect
                enemyAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
                senderAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
            }
            else if (conflict == 0)
            {
                // Both attacks take effect normally without interrumption
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.Damage);
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.Damage);
            }
            else if (conflict == 1)
            {
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.Damage);
                // Cancel a's action
                senderAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
            }
            else if (conflict == 2)
            {
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.Damage);
                // Cancel b's action
                // It should be sth related to CharacterCombat but we have the animator itself there
                enemyAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
            }
            else if (conflict == 3)
            {
                // Both receives action's damage
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.Damage);
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.Damage);

                // Cancel both a and b
                senderAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
            }
            else
            {
                Debug.Log(string.Format("No conflict for characters: {0} and {1}", enemy.name, sender.name));
            }

            // TODO: Apply force to gameobject
            // TODO: We should store hashAnimations somewhere
        }

        #endregion
    }
}
