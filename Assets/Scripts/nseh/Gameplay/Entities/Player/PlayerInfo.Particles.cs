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
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesAttackA).GetObject();
                    this.currentPoolParticle = this.poolParticlesAttackA;
                    break;

                case AttackType.CharacterAttackAStep2:
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesComboAAA).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboAAA;
                    break;

                case AttackType.CharacterAttackAStep3:
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesComboAAA).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboAAA;
                    break;

                case AttackType.CharacterAttackBStep1:
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesAttackB).GetObject();
                    this.currentPoolParticle = this.poolParticlesAttackB;
                    break;

                case AttackType.CharacterAttackBStep2:
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesComboBB).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboBB;
                    break;

                case AttackType.CharacterAttackBSharp:
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesComboBSharp).GetObject();
                    this.currentPoolParticle = this.poolParticlesComboBSharp;
                    break;

                case AttackType.CharacterHability:
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesHability).GetObject();
                    this.currentPoolParticle = this.poolParticlesHability;
                    break;

                case AttackType.CharacterDefinitive:
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesDefinitive).GetObject();
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
                    particle = GameManager.Instance.LevelManager.ObjectPoolManager.GetPool(this.poolParticlesDefense).GetObject();
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


            if (!_bavaScene)
            {
                if (this.particleAttackA != null)
                {
                    sb.AppendFormat(ParticlePools.PLAYER_ATTACK_A, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleAttackA, 10));
                    sb.Clear();
                }

                if (this.particleAttackB != null)
                {

                    sb.AppendFormat(ParticlePools.PLAYER_ATTACK_B, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleAttackB, 10));
                    sb.Clear();
                }

                if (this.particleComboAAA != null)
                {
                    sb.AppendFormat(ParticlePools.PLAYER_COMBO_AAA, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleComboAAA, 10));
                    sb.Clear();
                }

                if (this.particleComboBB != null)
                {

                    sb.AppendFormat(ParticlePools.PLAYER_COMBO_BB, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleComboBB, 10));
                    sb.Clear();
                }

                if (this.particleComboBSharp != null)
                {
                    sb.AppendFormat(ParticlePools.PLAYER_COMBO_B_SHARP, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleComboBSharp, 5));
                    sb.Clear();
                }

                if (this.particleDefinitive != null)
                {
                    sb.AppendFormat(ParticlePools.PLAYER_DEFINITIVE, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleDefinitive, 5));
                    sb.Clear();
                }

                if (this.particleHability != null)
                {

                    sb.AppendFormat(ParticlePools.PLAYER_HABILITY, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleHability, 5));
                    sb.Clear();
                }

                if (this.particleDefense != null)
                {
                    sb.AppendFormat(ParticlePools.PLAYER_DEFENSE, _playerName);
                    checkers.Add(GameManager.Instance.LevelManager.ObjectPoolManager.CreatePool(sb.ToString(), this.particleDefense, 5));
                    sb.Clear();
                } 
            }

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
           this.poolParticlesAttackA = string.Format(ParticlePools.PLAYER_ATTACK_A, _playerName);

           this.poolParticlesAttackB = string.Format(ParticlePools.PLAYER_ATTACK_B, _playerName);

           this.poolParticlesComboAAA = string.Format(ParticlePools.PLAYER_COMBO_AAA, _playerName);

           this.poolParticlesComboBB = string.Format(ParticlePools.PLAYER_COMBO_BB, _playerName);

           this.poolParticlesComboBSharp = string.Format(ParticlePools.PLAYER_COMBO_B_SHARP, _playerName);

           this.poolParticlesDefinitive = string.Format(ParticlePools.PLAYER_DEFINITIVE, _playerName);

           this.poolParticlesHability = string.Format(ParticlePools.PLAYER_HABILITY, _playerName);

           this.poolParticlesDefense = string.Format(ParticlePools.PLAYER_DEFENSE, _playerName);
        }

        #endregion
    }
}
