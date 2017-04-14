using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat.Defense;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Colors = nseh.Utils.Constants.Colors;
using SystemObject = System.Object;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.System
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class WeaponCollision : MonoBehaviour
    {
        #region Public Properties

        public int index;

        #endregion

        #region Private Properties

        private PlayerCombat playerCombat;
        private PlayerMovement playerMovement;
        private PlayerInfo playerInfo;

        private List<GameObject> enemyTargets;
        private GameObject rootCharacter;

        private delegate void FuncRef<T1, T2, T3, T4>(ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4);
        private Dictionary<int, FuncRef<CharacterAttack, PlayerInfo, CharacterAttack, PlayerInfo>> collisionHandlersAttacks;
        private Dictionary<int, FuncRef<CharacterAttack, PlayerInfo, CharacterDefense, PlayerInfo>> collisionHandlersAttackAndDefense;

        private string parentObjName;
        private int layerMask;

        #endregion

        #region Public C# Properties

        public int Index
        {
            get { return this.index; }
        }

        #endregion

        #region Private Methods

        private bool EnemyHasBeenTakenAback(ref GameObject enemy)
        {
            PlayerMovement enemyMov = enemy.GetComponent<PlayerMovement>();

            return !this.playerMovement.IsFacingRight && !enemyMov.IsFacingRight ||
                   !this.playerMovement.IsFacingRight && enemyMov.IsFacingRight;
        }

        private void Start()
        {
            this.layerMask = LayerMask.GetMask(Tags.PLAYER);

            // Setup collisionHandlers
            this.collisionHandlersAttacks = new Dictionary<int, FuncRef<CharacterAttack, PlayerInfo, CharacterAttack, PlayerInfo>>();
            this.collisionHandlersAttacks[-1] = CancelCollisionHandler;
            this.collisionHandlersAttacks[0] = NoneCollisionHandler;
            this.collisionHandlersAttacks[1] = FirstOverSecondCollisionHandler;
            this.collisionHandlersAttacks[2] = SecondOverFirstCollisionHandler;
            this.collisionHandlersAttacks[3] = BothCollisionHandler;

            this.collisionHandlersAttackAndDefense = new Dictionary<int, FuncRef<CharacterAttack, PlayerInfo, CharacterDefense, PlayerInfo>>();
            this.collisionHandlersAttackAndDefense[-1] = CancelCollisionHandlerAttDef;
            this.collisionHandlersAttackAndDefense[0] = NoneCollisionHandlerAttDef;

            this.playerInfo = this.transform.root.GetComponent<PlayerInfo>();
            this.playerCombat = this.playerInfo.PlayerCombat;
            this.playerMovement = this.playerInfo.PlayerMovement;
            this.enemyTargets = new List<GameObject>();

            this.parentObjName = this.transform.root.name;
            this.rootCharacter = this.transform.root.gameObject;
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

            if (enemy.CompareTag(Tags.PLAYER_BODY))
            {
                PlayerInfo enemyInfo = enemy.GetComponent<PlayerInfo>();

                if (enemyInfo.Player != this.playerInfo.Player)
                {
                    bool enemyTakenAback = this.EnemyHasBeenTakenAback(ref enemy);

                    if (enemyTakenAback)
                    {
                        var attack = this.playerCombat.CurrentAction as HandledAction;

                        if (!SystemObject.ReferenceEquals(null, attack))
                        {
                            Debug.Log(String.Format("<color={0}> {1} does the attack: {2}</color>", Colors.FUCHSIA, this.parentObjName, attack.StateName));

                            this.PerformDamage(ref this.rootCharacter, ref attack, ref enemy, ref this.playerInfo, ref enemyInfo);
                        }
                    }
                    else
                    {
                        // enemies are watching each other
                        this.enemyTargets.Add(enemy);

                        var attack = this.playerCombat.CurrentAction as HandledAction;

                        if (!SystemObject.ReferenceEquals(null, attack))
                        {
                            Debug.Log(String.Format("<color={0}> {1} does the attack: {2}</color>", Colors.FUCHSIA, this.parentObjName, attack.StateName));

                            this.PerformDamage(ref this.rootCharacter, ref attack, ref this.playerInfo, ref this.enemyTargets);
                        }
                    }
                }

                this.enemyTargets.Clear();
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
        public void PerformDamage(ref GameObject sender, ref HandledAction senderAction, ref GameObject enemy, ref PlayerInfo senderInfo, ref PlayerInfo enemyInfo)
        {
            if (sender != null && enemy != null)
            {
                CharacterAttack senderAttack = (senderAction as CharacterAttack);
                int amountDamage = (int)senderAttack.CurrentDamage;

                // Reduce health
                enemyInfo.PlayerHealth.TakeDamage(amountDamage);

                // Display effects
                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAttack.AttackType), enemyInfo.ParticleBodyPos.position);
            }
            else
            {
                Debug.Log("Some parameter is null");
            }
        }

        // Enemy are facing each other
        public void PerformDamage(ref GameObject sender, ref HandledAction senderAction, ref PlayerInfo senderInfo, ref List<GameObject> targetEnemies)
        {
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

                            this.ResolveConflict(conflict, ref sender, ref senderAction, ref senderInfo, ref enemy, ref enemyAction, ref enemyInfo);
                        }
                        else
                        {
                            // Enemy is not taking any action
                            CharacterAttack senderAttack = (senderAction as CharacterAttack);
                            int amountDamage = (int)senderAttack.CurrentDamage;

                            // Reduce health
                            enemyInfo.PlayerHealth.TakeDamage(amountDamage);

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

        private void ResolveConflict(int conflict, ref GameObject sender, ref HandledAction senderAction, ref PlayerInfo senderInfo, ref GameObject enemy, ref HandledAction enemyAction, ref PlayerInfo enemyInfo)
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

                this.ResolveConflict(conflict, ref sender, ref senderAttack, ref senderInfo, ref enemy, ref enemyAttack, ref enemyInfo);
            }
            else if (senderAction is CharacterAttack && enemyAction is CharacterDefense)
            {
                CharacterAttack senderAttack = senderAction as CharacterAttack;
                CharacterDefense enemyDefense = enemyAction as CharacterDefense;

                this.ResolveConflict(conflict, ref sender, ref senderAttack, ref senderInfo, ref enemy, ref enemyDefense, ref enemyInfo);
            }
            else
            {
                Debug.Log("Check types for senderAction and EnemyAction");
            }
        }

        private void ResolveConflict(int conflict, ref GameObject sender, ref CharacterAttack senderAttack, ref PlayerInfo senderInfo, ref GameObject enemy, ref CharacterDefense enemyDefense, ref PlayerInfo enemyInfo)
        {
            if (conflict == -1 || conflict == 0)
            {
                this.collisionHandlersAttackAndDefense[conflict].Invoke(ref senderAttack, ref senderInfo, ref enemyDefense, ref enemyInfo);
            }
            else
            {
                Debug.Log(string.Format("No conflict for characters: {0} and {1}", enemy.name, sender.name));
            }
        }

        private void ResolveConflict(int conflict, ref GameObject sender, ref CharacterAttack senderAction, ref PlayerInfo senderInfo, ref GameObject enemy, ref CharacterAttack enemyAction, ref PlayerInfo enemyInfo)
        {
            if (conflict >= -1 && conflict <= 3)
            {
                this.collisionHandlersAttacks[conflict].Invoke(ref senderAction, ref senderInfo, ref enemyAction, ref enemyInfo);
            }
            else
            {
                Debug.Log(string.Format("No conflict for characters: {0} and {1}", enemy.name, sender.name));
            }
        }

        #endregion

        #region Collision Handlers - Attacks

        private void CancelCollisionHandler(ref CharacterAttack senderAction, 
                                            ref PlayerInfo senderInfo, 
                                            ref CharacterAttack enemyAction, 
                                            ref PlayerInfo enemyInfo)
        {
            Debug.Log("CancelCollisionHandler()");

            // Cancel both attacks; do not take any damage effect
            enemyAction.Animator.SetTrigger(enemyInfo.ImpactHash);
            senderAction.Animator.SetTrigger(senderInfo.ImpactHash);

            // Display effects
            senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
            enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
        }

        private void NoneCollisionHandler(ref CharacterAttack senderAction,
                                            ref PlayerInfo senderInfo,
                                            ref CharacterAttack enemyAction,
                                            ref PlayerInfo enemyInfo)
        {
            Debug.Log("NoneCollisionHandler()");

            // Both attacks take effect normally without interrumption
            enemyInfo.PlayerHealth.TakeDamage((int)senderAction.CurrentDamage);
            senderInfo.PlayerHealth.TakeDamage((int)enemyAction.CurrentDamage);

            // Display effects
            senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
            enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
        }

        private void FirstOverSecondCollisionHandler(ref CharacterAttack senderAction,
                                            ref PlayerInfo senderInfo,
                                            ref CharacterAttack enemyAction,
                                            ref PlayerInfo enemyInfo)
        {
            Debug.Log("FirstOverSecondCollisionHandler()");

            enemyInfo.PlayerHealth.TakeDamage((int)senderAction.CurrentDamage);
            // Cancel a's action
            senderAction.Animator.SetTrigger(senderInfo.ImpactHash);

            // Display effects
            enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
        }

        private void SecondOverFirstCollisionHandler(ref CharacterAttack senderAction,
                                            ref PlayerInfo senderInfo,
                                            ref CharacterAttack enemyAction,
                                            ref PlayerInfo enemyInfo)
        {
            Debug.Log("SecondOverFirstCollisionHandler()");

            senderInfo.PlayerHealth.TakeDamage((int)enemyAction.CurrentDamage);
            // Cancel b's action
            enemyAction.Animator.SetTrigger(enemyInfo.ImpactHash);

            // Display effects
            senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
        }

        private void BothCollisionHandler(ref CharacterAttack senderAction,
                                            ref PlayerInfo senderInfo,
                                            ref CharacterAttack enemyAction,
                                            ref PlayerInfo enemyInfo)
        {
            Debug.Log("BothCollisionHandler()");

            // Both receives action's damage
            senderInfo.PlayerHealth.TakeDamage((int)enemyAction.CurrentDamage);
            enemyInfo.PlayerHealth.TakeDamage((int)senderAction.CurrentDamage);

            // Cancel both a and b
            senderAction.Animator.SetTrigger(senderInfo.ImpactHash);

            // Display effects
            senderInfo.PlayParticleAtPosition(enemyInfo.GetParticleAttack(enemyAction.AttackType), senderInfo.ParticleBodyPos.position);
            enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAction.AttackType), enemyInfo.ParticleBodyPos.position);
        }

        // Put other collision handlers here if exist

        #endregion

        #region Collision Handlers - Attacks/Defense

        private void CancelCollisionHandlerAttDef(ref CharacterAttack senderAttack, ref PlayerInfo senderInfo, ref CharacterDefense enemyDefense, ref PlayerInfo enemyInfo)
        {
            if (senderAttack.AttackType == AttackType.CharacterAttackBSharp)
            {
                enemyInfo.PlayerHealth.TakeDamage((int)senderAttack.CurrentDamage / 2);
                enemyDefense.Animator.SetTrigger(enemyInfo.ImpactHash);

                // Display effects
                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAttack.AttackType), enemyInfo.ParticleBodyPos.position);
            }
            else if (senderAttack.AttackType == AttackType.CharacterAttackBStep2
                    || senderAttack.AttackType == AttackType.CharacterDefinitive)
            {
                enemyInfo.PlayerHealth.TakeDamage((int)senderAttack.CurrentDamage);
                enemyDefense.Animator.SetTrigger(enemyInfo.ImpactHash);

                // Display effects
                enemyInfo.PlayParticleAtPosition(senderInfo.GetParticleAttack(senderAttack.AttackType), enemyInfo.ParticleBodyPos.position);
            }
        }

        private void NoneCollisionHandlerAttDef(ref CharacterAttack senderAttack, ref PlayerInfo senderInfo, ref CharacterDefense enemyDefense, ref PlayerInfo enemyInfo)
        {
            senderAttack.Animator.SetTrigger(senderInfo.ImpactHash);

            // Display effects
            senderInfo.PlayParticleAtPosition(senderInfo.GetParticleDefense(enemyDefense.CurrentMode), senderInfo.ParticleBodyPos.position);
        }

        #endregion
    }
}
