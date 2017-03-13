using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat.Defense;
using nseh.Gameplay.Entities.Player;
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
        #region Protected Properties

        protected Collider hitBox;
        protected PlayerCombat characterCombat;
        protected PlayerMovement characterMovement;
        protected PlayerInfo playerInfo;
        protected Animator anim;

        protected List<GameObject> enemyTargets;
        protected GameObject rootCharacter;

        protected string parentObjName;

        #endregion

        #region Private Properties

        private int layerMask;

        #endregion

        #region Private Methods

        private bool EnemyHasBeenTakenAback(ref GameObject enemy)
        {
            PlayerMovement enemyMov = enemy.GetComponent<PlayerMovement>();

            return !(this.characterMovement.IsFacingRight && !enemyMov.IsFacingRight
                    || !this.characterMovement.IsFacingRight && enemyMov.IsFacingRight);
        }

        private void Awake()
        {
            this.hitBox = GetComponent<Collider>();
            this.hitBox.enabled = false;

            this.characterCombat = this.transform.root.GetComponent<PlayerCombat>();
            this.characterMovement = this.transform.root.GetComponent<PlayerMovement>();
            this.playerInfo = this.transform.root.GetComponent<PlayerInfo>();
            this.anim = this.transform.root.GetComponent<Animator>();
            this.enemyTargets = new List<GameObject>();

            this.parentObjName = this.transform.root.name;
            this.rootCharacter = this.transform.root.gameObject;

            this.layerMask = LayerMask.GetMask("Player");
        }

        #endregion

        #region Trigger Methods

        protected void OnCollisionEnter(Collision collider)
        {
            GameObject enemy = collider.gameObject;

            //// For Debugging
            //foreach (ContactPoint contact in collision.contacts)
            //{
            //    Debug.DrawRay(contact.point, contact.normal, Color.red, 5.0f);
            //}

            if (enemy.CompareTag(Tags.PLAYER))
            {
                PlayerInfo enemyInfo = enemy.GetComponent<PlayerInfo>();

                if (enemyInfo.Player != this.playerInfo.Player)
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

                            this.PerformDamage(ref this.rootCharacter, ref attack, this.enemyTargets);
                        }
                    }

                    this.enemyTargets.Clear(); 
                }
            }
        }

        //protected void OnCollisionExit(Collision collider)
        //{
        //    GameObject enemy = collider.gameObject;

        //    if (enemy.CompareTag(Tags.PLAYER) && this.parentObjName != enemy.name)
        //    {
        //        Debug.Log("OnCollisionExit");
        //        this.enemyTargets.Remove(enemy);
        //    }
        //}

        #endregion

        #region Combat System

        // Enemy is taken aback
        public void PerformDamage(ref GameObject sender, ref HandledAction senderAction, ref GameObject enemy)
        {
            if (sender != null && enemy != null)
            {
                CharacterAttack senderAttack = (senderAction as CharacterAttack);
                int amountDamage = (int)senderAttack.CurrentDamage;

                // Reduce health
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage(amountDamage);

                // Display effects
                PlayerInfo enemyInfo = enemy.GetSafeComponent<PlayerInfo>();
                PlayerInfo senderInfo = enemy.GetSafeComponent<PlayerInfo>();

                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAttack.AttackType), enemyInfo.ParticleBodyPos.position);
            }
            else
            {
                Debug.Log("Some parameter is null");
            }
        }

        // Enemy are facing each other
        public void PerformDamage(ref GameObject sender, ref HandledAction senderAction, List<GameObject> targetEnemies)
        {
            PlayerInfo senderInfo = sender.GetComponent<PlayerInfo>();

            if (targetEnemies != null && targetEnemies.Count > 0)
            {
                for (int i = 0; i < targetEnemies.Count(); i++)
                {
                    var enemy = targetEnemies[i];

                    PlayerInfo enemyInfo = enemy.GetComponent<PlayerInfo>();
                    
                    // Check if sender is not the enemy
                    if (senderInfo.Player != enemyInfo.Player)
                    {
                        // Check combat system rules
                        var enemyAction = enemy.GetSafeComponent<PlayerCombat>().CurrentAction as HandledAction;

                        if (!SystemObject.ReferenceEquals(null, enemyAction))
                        {
                            int conflict = CombatRules.CompareActions(ref senderAction, ref enemyAction);

                            this.ResolveConflict(conflict, ref sender, ref senderAction, ref enemy, ref enemyAction);
                        }
                        else
                        {
                            // Enemy is not taking any action
                            CharacterAttack senderAttack = (senderAction as CharacterAttack);
                            int amountDamage = (int)senderAttack.CurrentDamage;

                            // Reduce health
                            enemy.GetSafeComponent<CharacterHealth>().TakeDamage(amountDamage);

                            // Display effects
                            enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAttack.AttackType), enemyInfo.ParticleHeadPos.position);
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
            PlayerInfo senderInfo = sender.GetSafeComponent<PlayerInfo>();
            PlayerInfo enemyInfo = enemy.GetSafeComponent<PlayerInfo>();

            if (conflict == -1)
            {
                if (senderAttack.AttackType == AttackType.CharacterAttackBSharp)
                {
                    enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAttack.CurrentDamage / 2);
                    enemyDefense.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));

                    // Display effects
                    enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAttack.AttackType), enemyInfo.ParticleBodyPos.position);
                }
                else if (senderAttack.AttackType == AttackType.CharacterAttackBStep2 
                        || senderAttack.AttackType == AttackType.CharacterDefinitive)
                {
                    enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAttack.CurrentDamage);
                    enemyDefense.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));

                    // Display effects
                    enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAttack.AttackType), enemyInfo.ParticleBodyPos.position);
                }
            }
            else if (conflict == 0)
            {
                senderAttack.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));

                // Display effects
                senderInfo.PlayParticleAtPosition(senderInfo.GetParticleDefense(enemyDefense.CurrentMode), senderInfo.ParticleBodyPos.position);
            }
            else
            {
                Debug.Log(string.Format("No conflict for characters: {0} and {1}", enemy.name, sender.name));
            }
        }

        private void ResolveConflict(int conflict, ref GameObject sender, ref CharacterAttack senderAction, ref GameObject enemy, ref CharacterAttack enemyAction)
        {
            PlayerInfo senderInfo = sender.GetSafeComponent<PlayerInfo>();
            PlayerInfo enemyInfo = enemy.GetSafeComponent<PlayerInfo>();

            if (conflict == -1)
            {
                // Cancel both attacks; do not take any damage effect
                enemyAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));
                senderAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));

                // Display effects
                senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
            }
            else if (conflict == 0)
            {
                // Both attacks take effect normally without interrumption
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.CurrentDamage);
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.CurrentDamage);

                // Display effects
                senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
            }
            else if (conflict == 1)
            {
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.CurrentDamage);
                // Cancel a's action
                senderAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));

                // Display effects
                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
            }
            else if (conflict == 2)
            {
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.CurrentDamage);
                // Cancel b's action
                // It should be sth related to CharacterCombat but we have the animator itself there
                enemyAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));

                // Display effects
                senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
            }
            else if (conflict == 3)
            {
                // Both receives action's damage
                sender.GetSafeComponent<CharacterHealth>().TakeDamage((int)enemyAction.CurrentDamage);
                enemy.GetSafeComponent<CharacterHealth>().TakeDamage((int)senderAction.CurrentDamage);

                // Cancel both a and b
                senderAction.Animator.SetTrigger(Animator.StringToHash(Actions.CHARACTER_IMPACT));

                // Display effects
                senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
            }
            else
            {
                Debug.Log(string.Format("No conflict for characters: {0} and {1}", enemy.name, sender.name));
            }

            // TODO: We should store hashAnimations somewhere
        }

        #endregion
    }
}
