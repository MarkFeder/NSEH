using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using System;
using UnityEngine;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Gameplay.Entities.Player
{
    public class PlayerInfo : MonoBehaviour
    {
        #region Public Properties

        [Header("Input properties")]
        public int gamepadIndex;

        [Space(20)]

        [Header("Damages of attacks")]
        public float damageAttackA;
        public float damageAttackB;
        public float damageComboAAA01;
        public float damageComboAAA02;
        public float damageComboBB01;
        public float damageComboBSharp;
        public float damageDefinitive;
        public float damnageHability;

        [Space(20)]

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

        private float horizontal;
        private float vertical;

        [Range(1, 4)]
        private int player;

        private bool teletransported;
        private bool jumpPressed;

        public Sprite characterPortrait;

        #endregion

        #region Public C# Properties

        #region Movement and player info

        public float Horizontal
        {
            get
            {
                return this.horizontal;
            }

            set
            {
                this.horizontal = value;
            }
        }

        public float Vertical
        {
            get
            {
                return this.vertical;
            }

            set
            {
                this.vertical = value;
            }
        }

        public int GamepadIndex
        {
            get
            {
                return this.gamepadIndex;
            }

            set
            {
                this.gamepadIndex = value;
            }
        }

        public int Player
        {
            get
            {
                return this.player;
            }

            set
            {
                this.player = value;
            }
        }

        public bool Teletransported
        {
            get
            {
                return this.teletransported;
            }

            set
            {
                this.teletransported = value;
            }
        }

        public bool JumpPressed
        {
            get
            {
                return this.jumpPressed;
            }
        }

        public Sprite CharacterPortrait
        {
            get
            {
                return (this.characterPortrait) ? this.characterPortrait : null;
            }
        }

        #endregion

        #region Damage of attacks

        public float DamageAttackA
        {
            get
            {
                return this.damageAttackA;
            }
        }

        public float DamageAttackB
        {
            get
            {
                return this.damageAttackB;
            }
        }

        public float DamageComboAAA01
        {
            get
            {
                return this.damageComboAAA01;
            }
        }

        public float DamageComboAAA02
        {
            get
            {
                return this.damageComboAAA02;
            }
        }

        public float DamageComboBB01
        {
            get
            {
                return this.damageComboBB01;
            }
        }

        public float DamageComboBSharp
        {
            get
            {
                return this.damageComboBSharp;
            }
        }

        public float DamageDefinitive
        {
            get
            {
                return this.damageDefinitive;
            }
        }

        public float DamageHability
        {
            get
            {
                return this.damnageHability;
            }
        }

        #endregion

        #region Particles

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

        #endregion

        private void Start()
        {
            this.teletransported = false;
            this.jumpPressed = false;
        }

        private void Update()
        {
            this.horizontal = Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, this.gamepadIndex));
            this.vertical = Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, this.gamepadIndex));

            this.jumpPressed = Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP, this.gamepadIndex));
        }

        #region Particles

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

        #endregion
    }
}
