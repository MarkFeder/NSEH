using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using nseh.Gameplay.Combat.System;
using nseh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Inputs = nseh.Utils.Constants.Input;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Entities.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerCombat : MonoBehaviour
    {
        #region Private Properties

        private List<Collider> colliders;
        private PlayerInfo playerInfo;

        private List<IAction> actions;

        #endregion

        #region Public C# Properties

        public List<IAction> Actions
        {
            get { return this.actions; }
        }

        public int CurrentHashAnimation
        {
            get { return (this.playerInfo.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash); }
        }

        public IAction CurrentAction
        {
            get { return this.actions.Where(act => act.HashAnimation == this.CurrentHashAnimation).FirstOrDefault(); }
        }

        public IAction CurrentDefenseAction
        {
            get { return this.actions.OfType<CharacterDefense>().Where(act => act.HashAnimation == this.CurrentHashAnimation).FirstOrDefault(); }
        }

        public GameObject TargetEnemy
        {
            get;
            set;
        }

        #endregion

        private void Awake()
        {
            this.colliders = this.gameObject.GetSafeComponentsInChildren<Collider>().Where(c => c.tag.Equals(Tags.WEAPON)).ToList();
        }

        private void Start()
        {
            this.playerInfo = GetComponent<PlayerInfo>();
            this.actions = this.FillCharacterActions();
        }

        private void Update()
        {
            if (this.CurrentAction != null)
            {
                Debug.Log(string.Format("[{0}] - Current action is: {1}", this.playerInfo.PlayerName, this.CurrentAction.ToString()));
            }
        }


        #region Actions 

        private List<IAction> FillCharacterActions()
        {
            var list = new List<IAction>()
            {
                new CharacterAttack(AttackType.CharacterAttackAStep1, this.playerInfo.ComboAAA01Hash, this.playerInfo.ComboAAA01StateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.playerInfo.GamepadIndex), this.playerInfo.DamageAttackA),
                new CharacterAttack(AttackType.CharacterAttackAStep2, this.playerInfo.ComboAAA02Hash, this.playerInfo.ComboAAA02StateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.playerInfo.GamepadIndex), this.playerInfo.DamageComboAAA01),
                new CharacterAttack(AttackType.CharacterAttackAStep3, this.playerInfo.ComboAAA03Hash, this.playerInfo.ComboAAA03StateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.A, this.playerInfo.GamepadIndex), this.playerInfo.DamageComboAAA02),
                new CharacterAttack(AttackType.CharacterAttackBStep1, this.playerInfo.ComboBB01Hash, this.playerInfo.ComboBB01StateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.B, this.playerInfo.GamepadIndex), this.playerInfo.DamageAttackB),
                new CharacterAttack(AttackType.CharacterAttackBStep2, this.playerInfo.ComboBB02Hash, this.playerInfo.ComboBB02StateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.B, this.playerInfo.GamepadIndex), this.playerInfo.DamageComboBB01),
                new CharacterAttack(AttackType.CharacterDefinitive, this.playerInfo.DefinitiveHash, this.playerInfo.DefinitiveStateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.DEFINITIVE, this.playerInfo.GamepadIndex), this.playerInfo.DamageDefinitive),
                new CharacterAttack(AttackType.CharacterHability, this.playerInfo.HabilityHash, this.playerInfo.HabilityStateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.HABILITY, this.playerInfo.GamepadIndex), this.playerInfo.DamageHability),
                new CharacterDefense(DefenseType.NormalDefense, this.playerInfo.DefenseHash, this.playerInfo.DefenseStateName, this.playerInfo.Animator, KeyCode.None, String.Format("{0}{1}", Inputs.DEFENSE, this.playerInfo.GamepadIndex))
            };

            return list;
        }

        #endregion

        #region Public Methods

        public void DeactivateSpecificCollider(int index)
        {
            if (this.colliders != null && this.colliders.Count > 0)
            {
                // Deactivate other colliders
                for (int i = 0; i < this.colliders.Count; i++)
                {
                    Collider collider = this.colliders[i];
                    WeaponCollision weaponCollision = collider.GetComponent<WeaponCollision>();

                    if (weaponCollision.Index == index)
                    {
                        collider.enabled = false;
                        weaponCollision.enabled = false;
                    }
                }
            }
            else
            {
                Debug.Log(String.Format("DeactivateCollider({0}): colliders are 0 or null", index));
            }
        }

        #endregion

        #region Animation Events

        public void ActivateCollider(int index)
        {
            if (this.colliders != null && this.colliders.Count > 0)
            {
                // Deactivate other colliders
                for (int i = 0; i < this.colliders.Count; i++)
                {
                    Collider collider = this.colliders[i];
                    WeaponCollision weaponCollision = collider.GetComponent<WeaponCollision>();

                    if (weaponCollision.Index != index)
                    {
                        collider.enabled = false;
                        weaponCollision.enabled = false;
                    }
                    else
                    {
                        collider.enabled = true;
                        weaponCollision.enabled = true;
                    }
                }
            }
            else
            {
                Debug.Log(String.Format("ActivateCollider({0}): colliders are 0 or null", index));
            }
        }

        public void DeactivateCollider(int index)
        {
            if (this.colliders != null && this.colliders.Count > 0)
            {
                // Deactivate other colliders
                for (int i = 0; i < this.colliders.Count; i++)
                {
                    Collider collider = this.colliders[i];
                    WeaponCollision weaponCollision = collider.GetComponent<WeaponCollision>();

                    collider.enabled = false;
                    weaponCollision.enabled = false;
                }
            }
            else
            {
                Debug.Log(String.Format("DeactivateCollider({0}): colliders are 0 or null", index));
            }
        }

        #endregion
    }
}
