using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using nseh.Managers.Main;
using nseh.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ParticlePools = nseh.Utils.Constants.Pool.Particles;

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

        #region Private Properties

        private string currentPoolParticle;

        private string poolParticlesAttackA;
        private string poolParticlesAttackB;
        private string poolParticlesComboAAA;
        private string poolParticlesComboBB;
        private string poolParticlesComboBSharp;
        private string poolParticlesDefinitive;
        private string poolParticlesHability;
        private string poolParticlesDefense;

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

        public string CurrentPoolParticle
        {
            get { return this.currentPoolParticle; }
        }

        #endregion

        #region Public methods

        public GameObject GetParticleAttack(AttackType type)
        {
            GameObject particle = null;

            switch (type)
            {
                case AttackType.CharacterAttackAStep1:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesAttackA).GetObject();
                    this.currentPoolParticle = this.poolParticlesAttackA;
                    break;

                case AttackType.CharacterAttackAStep2:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesComboAAA).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboAAA;
                    break;

                case AttackType.CharacterAttackAStep3:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesComboAAA).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboAAA;
                    break;

                case AttackType.CharacterAttackBStep1:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesAttackB).GetObject();
                    this.currentPoolParticle = this.poolParticlesAttackB;
                    break;

                case AttackType.CharacterAttackBStep2:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesComboBB).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboBB;
                    break;

                case AttackType.CharacterAttackBSharp:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesComboBSharp).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboBSharp;
                    break;

                case AttackType.CharacterHability:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesHability).GetObject();
                    this.currentPoolParticle = this.poolParticlesHability;
                    break;

                case AttackType.CharacterDefinitive:
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesDefinitive).GetObject();
                    this.currentPoolParticle = this.poolParticlesDefinitive;
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
                    particle = GameManager.Instance.ObjectPoolManager.GetPool(this.poolParticlesDefense).GetObject();
                    this.currentPoolParticle = this.poolParticlesDefense;
                    break;

                default:
                    Debug.Log("DefenseType unrecognized!");
                    break;
            }

            return particle;
        }

        #endregion

        #region Private Methods

        private void SetupParticles()
        {
            StringBuilder sb = new StringBuilder();
            List<bool> checkers = new List<bool>();

            sb.AppendFormat(ParticlePools.PLAYER_ATTACK_A, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleAttackA, 5));
            sb.Clear();

            sb.AppendFormat(ParticlePools.PLAYER_ATTACK_B, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleAttackB, 5));
            sb.Clear();

            sb.AppendFormat(ParticlePools.PLAYER_COMBO_AAA, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleComboAAA, 5));
            sb.Clear();

            sb.AppendFormat(ParticlePools.PLAYER_COMBO_BB, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleComboBB, 5));
            sb.Clear();

            sb.AppendFormat(ParticlePools.PLAYER_COMBO_B_SHARP, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleComboBSharp, 3));
            sb.Clear();

            sb.AppendFormat(ParticlePools.PLAYER_DEFINITIVE, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleDefinitive, 3));
            sb.Clear();

            sb.AppendFormat(ParticlePools.PLAYER_HABILITY, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleHability, 3));
            sb.Clear();

            sb.AppendFormat(ParticlePools.PLAYER_DEFENSE, this.playerName);
            checkers.Add(GameManager.Instance.ObjectPoolManager.CreatePool(sb.ToString(), this.particleDefense, 5));
            sb.Clear();

            sb = null;

            bool result = checkers.All(c => c == true);

            if (result)
            {
                Debug.Log("All Particle Managers have been created properly");
            }
            else
            {
                Debug.Log("Particle Managers have not been created properly");
            }
        }

        private void SetupLookUpKeyParticles()
        {
           this.poolParticlesAttackA = string.Format(ParticlePools.PLAYER_ATTACK_A, this.playerName);

           this.poolParticlesAttackB = string.Format(ParticlePools.PLAYER_ATTACK_B, this.playerName);

           this.poolParticlesComboAAA = string.Format(ParticlePools.PLAYER_COMBO_AAA, this.playerName);

           this.poolParticlesComboBB = string.Format(ParticlePools.PLAYER_COMBO_BB, this.playerName);

           this.poolParticlesComboBSharp = string.Format(ParticlePools.PLAYER_COMBO_B_SHARP, this.playerName);

           this.poolParticlesDefinitive = string.Format(ParticlePools.PLAYER_DEFINITIVE, this.playerName);

           this.poolParticlesHability = string.Format(ParticlePools.PLAYER_HABILITY, this.playerName);

           this.poolParticlesDefense = string.Format(ParticlePools.PLAYER_DEFENSE, this.playerName);
        }

        #endregion
    }
}
