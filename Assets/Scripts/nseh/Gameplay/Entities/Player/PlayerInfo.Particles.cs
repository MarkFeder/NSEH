using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        #region Public Properties

        [Header("Player's body position to display particles")]
        public Transform particleBodyPos;
        public Transform particleHeadPos;
        public Transform particleFootPos;
        public Transform particleWeaponPos;

        [Space(20)]

        [Header("Particle's prefab depending on the type of action")]
        public float particleDestructionTime;
        public GameObject particleAttackA;
        public GameObject particleAttackB;
        public GameObject particleComboAAA;
        public GameObject particleComboBB;
        public GameObject particleComboBSharp;
        public GameObject particleDefinitive;
        public GameObject particleHability;
        public GameObject particleDefense;

        #endregion

        #region Public C# Properties

        public Transform ParticleBodyPos
        {
            get
            {
                return (this.particleBodyPos) ? this.particleBodyPos : null;
            }
        }

        public Transform ParticleHeadPos
        {
            get
            {
                return (this.particleHeadPos) ? this.particleHeadPos : null;
            }
        }

        public Transform ParticleFootPos
        {
            get
            {
                return (this.particleFootPos) ? this.particleFootPos : null;
            }
        }

        public Transform ParticleWeaponPos
        {
            get
            {
                return (this.particleWeaponPos) ? this.particleWeaponPos : null;
            }
        }

        #endregion

        #region Public methods

        public GameObject GetParticleAttack(AttackType type)
        {
            GameObject particle = null;

            switch (type)
            {
                case AttackType.CharacterAttackAStep1:
                    particle = this.particleAttackA;
                    break;

                case AttackType.CharacterAttackAStep2:
                    particle = this.particleComboAAA;
                    break;

                case AttackType.CharacterAttackAStep3:
                    particle = this.particleComboAAA;
                    break;

                case AttackType.CharacterAttackBStep1:
                    particle = this.particleAttackB;
                    break;

                case AttackType.CharacterAttackBStep2:
                    particle = this.particleComboBB;
                    break;

                case AttackType.CharacterAttackBSharp:
                    particle = this.particleComboBSharp;
                    break;

                case AttackType.CharacterHability:
                    particle = this.particleHability;
                    break;

                case AttackType.CharacterDefinitive:
                    particle = this.particleDefinitive;
                    break;

                default:
                    Debug.Log("AttackType unrecognized!");
                    break;
            }

            return particle;
        }

        public GameObject GetParticleDefense(DefenseType type)
        {
            GameObject particle = null;

            switch (type)
            {
                case DefenseType.NormalDefense:
                    particle = this.particleDefense;
                    break;

                default:
                    Debug.Log("DefenseType unrecognized!");
                    break;
            }

            return particle;
        }

        public void PlayParticleAtPosition(GameObject particlePrefab, Vector3 position)
        {
            if (particlePrefab != null)
            {
                GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);
                particle.GetComponent<ParticleSystem>().Play();

                Destroy(particle, this.particleDestructionTime);
            }
            else
            {
                Debug.Log("ParticlePrefab is null!");
            }
        }

        #endregion
    }
}
