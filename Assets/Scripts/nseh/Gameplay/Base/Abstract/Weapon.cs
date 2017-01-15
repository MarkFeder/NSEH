using nseh.Gameplay.Base.Interfaces;
using System;
using UnityEngine;
using Constants = nseh.Utils.Constants;

namespace nseh.Gameplay.Base.Abstract
{
    [Serializable]
    //[RequireComponent(typeof(Collider))]
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        protected string targetType;
        protected Rigidbody body;

        protected Collider hitBox;
        private bool hasCollide;
        protected Transform parent;

        protected CharacterCombat characterCombat;
        
        protected float amountDamage;

        protected int countCollisions;


        protected virtual void Awake()
        {
            this.targetType = Constants.Tags.PLAYER;

            this.body = GetComponent<Rigidbody>();
            this.body.useGravity = false;
            this.body.isKinematic = false;
            this.body.detectCollisions = true;

            this.hitBox = GetComponent<Collider>();
            this.hitBox.isTrigger = false;
            this.hasCollide = false;
            
            this.characterCombat = transform.root.GetComponent<CharacterCombat>();

            this.parent = this.transform.parent;

            this.countCollisions = 0;
        }

        protected virtual void Start()
        {
            this.AdjustHitBoxToBoneOnStart();
        }

        protected virtual void Update()
        {
            // Set the hitbox position of his parent to match bone
            this.hitBox.transform.position = this.parent.position;

            Debug.Log("COUNT COLLISIONS : " + this.countCollisions);
        }

        protected virtual void AdjustHitBoxToBoneOnStart()
        {
            var hitBoxCapsule = this.hitBox as CapsuleCollider;

            if (hitBoxCapsule != null)
            {
                Vector3 capCenter = hitBoxCapsule.center;
                capCenter.y += 0.30f;

                hitBoxCapsule.center = capCenter;
            }
        }

        //protected virtual float CalculateDamage(AttackType attack)
        //{
        //    float modifierDamage;

        //    switch (attack)
        //    {
        //        case AttackType.CharacterAttackA:
        //            modifierDamage = 1.0f;
        //            break;

        //        case AttackType.CharacterAttackB:
        //            modifierDamage = 3.0f;
        //            break;

        //        default:
        //            modifierDamage = 0.0f;
        //            break;
        //    }

        //    return this.amountDamage * modifierDamage;
        //}

        //private void HasAlreadyHit(Animator anim, AttackType attackMode, GameObject enemy)
        //{
        //    if (!anim.AnimatorIsPlaying(this.characterCombat.ConvertModeToStateInfo(attackMode)) && this.countCollisions == 1)
        //    {
        //        this.countCollisions = 0;
        //    }
        //    else if (anim.AnimatorIsPlaying(this.characterCombat.ConvertModeToStateInfo(attackMode)) && this.countCollisions == 0)
        //    {
        //        var enemyHealth = enemy.GetComponent<IHealth>();
        //        enemyHealth.TakeDamage((int)this.CalculateDamage(attackMode));

        //        this.countCollisions++;
        //    }
        //    else if (anim.AnimatorIsPlaying(this.characterCombat.ConvertModeToStateInfo(attackMode)) && this.countCollisions == 1)
        //    {
        //        this.countCollisions = 0;
        //    }
        //}

        //protected virtual void MakeDamage(GameObject target)
        //{
        //    var attackMode = this.characterCombat.CurrentMode;
        //    var enemyHealth = target.GetComponent<IHealth>();
        //    enemyHealth.TakeDamage((int)this.CalculateDamage(attackMode));

        //    this.hasCollide = false;
        //}

        private void OnCollisionExit(Collision collision)
        {
            GameObject obj = collision.gameObject;
            if (obj != this && obj.CompareTag(this.targetType) && !this.hasCollide)
            {
                Debug.Log("OnCollisionEnter--Player: " + obj.name + " is in range");
                //this.hasCollide = true;
                //this.MakeDamage(obj);
            }
        }
    }
}
