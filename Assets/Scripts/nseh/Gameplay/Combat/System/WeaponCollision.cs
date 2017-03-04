﻿using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat.Defense;
using nseh.Gameplay.Movement;
using nseh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Actions = nseh.Utils.Constants.Animations.Combat;
using Colors = nseh.Utils.Constants.Colors;
using SystemObject = System.Object;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.System
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class WeaponCollision : MonoBehaviour
    {
        // External References
        protected Collider hitBox;
        protected CharacterCombat characterCombat;
        protected PlayerMovement characterMovement;
        protected Animator anim;

        protected List<GameObject> enemyTargets;
        protected GameObject rootCharacter;

        // Properties
        public string enemyType;

        protected string parentObjName;
         
        private int layerMask;

        protected void Awake()
        {
            this.enemyType = Tags.PLAYER;
            
            this.hitBox = GetComponent<Collider>();
            //this.hitBox.isTrigger = true;
            this.hitBox.enabled = false;

            this.characterCombat = this.transform.root.GetComponent<CharacterCombat>();
            this.characterMovement = this.transform.root.GetComponent<PlayerMovement>();
            this.anim = this.transform.root.GetComponent<Animator>();
            this.enemyTargets = new List<GameObject>();

            this.parentObjName = this.transform.root.name;
            this.rootCharacter = this.transform.root.gameObject;

            this.layerMask = LayerMask.GetMask("Player");
        }

        #region Private Methods

        private bool EnemyHasBeenTakenAback(ref GameObject enemy)
        {
            PlayerMovement enemyMov = enemy.GetComponent<PlayerMovement>();

            return !(this.characterMovement.IsFacingRight && !enemyMov.IsFacingRight
                    || !this.characterMovement.IsFacingRight && enemyMov.IsFacingRight);
        }

        #endregion

        #region Trigger Methods

        protected void OnCollisionEnter(Collision collision)
        {
            GameObject enemy = collision.gameObject;

            //// For Debugging
            //foreach (ContactPoint contact in collision.contacts)
            //{
            //    Debug.DrawRay(contact.point, contact.normal, Color.red, 5.0f);
            //}

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

        protected void OnCollisionExit(Collision collision)
        {
            GameObject enemy = collision.gameObject;

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
                int amountDamage = (int)(senderAction as CharacterAttack).CurrentDamage;

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
                            int amountDamage = (int)(senderAction as CharacterAttack).CurrentDamage;

                            enemy.GetSafeComponent<CharacterHealth>().TakeDamage(amountDamage);
                            enemy.GetSafeComponent<Rigidbody>().AddForce(-enemy.transform.forward * 100.0f);
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
                    enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAttack.CurrentDamage / 2);
                    enemyDefense.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
                }
                else if (senderAttack.AttackType == AttackType.CharacterAttackBStep2 
                        || senderAttack.AttackType == AttackType.CharacterDefinitive)
                {
                    enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAttack.CurrentDamage);
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
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.CurrentDamage);
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.CurrentDamage);
            }
            else if (conflict == 1)
            {
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.CurrentDamage);
                // Cancel a's action
                senderAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
            }
            else if (conflict == 2)
            {
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.CurrentDamage);
                // Cancel b's action
                // It should be sth related to CharacterCombat but we have the animator itself there
                enemyAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
            }
            else if (conflict == 3)
            {
                // Both receives action's damage
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.CurrentDamage);
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.CurrentDamage);

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
