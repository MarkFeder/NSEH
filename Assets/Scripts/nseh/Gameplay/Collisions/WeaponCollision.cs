using nseh.Gameplay.Base.Interfaces;
using nseh.Utils.Helpers;
using System;
using UnityEngine;
using System.Collections.Generic;
using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Combat;
using System.Linq;
using Constants = nseh.Utils.Constants;
using Colors = nseh.Utils.Constants.Colors;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Collisions
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class WeaponCollision : MonoBehaviour, IWeapon
    {
        // External References
        protected Collider hitBox;
        protected CharacterCombat characterCombat;
        protected CharacterMovement characterMovement;

        protected List<GameObject> enemyTargets;
        protected GameObject rootCharacter;

        // Properties
        [SerializeField]
        protected string enemyType;

        protected string parentObjName;

        public List<GameObject> EnemyTargets
        {
            get
            {
                return this.enemyTargets;
            }
        }

        protected virtual void Awake()
        {
            this.enemyType = Tags.PLAYER;
            
            this.hitBox = GetComponent<Collider>();
            this.hitBox.isTrigger = true;
            this.hitBox.enabled = false;

            this.characterCombat = this.transform.root.GetComponent<CharacterCombat>();
            this.characterMovement = this.transform.root.gameObject.GetSafeComponent<CharacterMovement>();
            this.enemyTargets = new List<GameObject>();

            this.parentObjName = this.transform.root.name;
            this.rootCharacter = this.transform.root.gameObject;
        }

        private bool EnemyHasBeenTakenAback(ref GameObject enemy)
        {
            CharacterMovement enemyMov = enemy.GetComponent<CharacterMovement>();
            return !(this.characterMovement.IsFacingRight && !enemyMov.IsFacingRight 
                    || !this.characterMovement.IsFacingRight && enemyMov.IsFacingRight);
        }

        protected void OnTriggerEnter(Collider other)
        {
            GameObject enemy = other.gameObject;

            if (enemy.CompareTag(this.enemyType) && this.parentObjName != enemy.name)
            {
                bool enemyTakenAback = this.EnemyHasBeenTakenAback(ref enemy);

                if (enemyTakenAback)
                {
                    var attack = this.characterCombat.Actions.OfType<CharacterAttack>().Where(at => at.HashAnimation == this.characterCombat.CurrentHashAnimation).FirstOrDefault();

                    Debug.Log(String.Format("<color={0}> {1} does the attack: {2}</color>", Colors.FUCHSIA, this.parentObjName, attack.StateName));

                    attack.PerformDamage(this.rootCharacter, enemy);
                }
                else
                {
                    // enemies are watching each other
                    this.enemyTargets.Add(enemy);

                    var attack = this.characterCombat.Actions.OfType<CharacterAttack>().Where(at => at.HashAnimation == this.characterCombat.CurrentHashAnimation).FirstOrDefault();

                    Debug.Log(String.Format("<color={0}> {1} does the attack: {2}</color>", Colors.FUCHSIA, this.parentObjName, attack.StateName));

                    attack.PerformDamage(this.rootCharacter, this.EnemyTargets);
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
    }
}
