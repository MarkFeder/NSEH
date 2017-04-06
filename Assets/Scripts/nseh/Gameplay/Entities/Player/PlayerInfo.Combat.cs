using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        #region Public Properties

        [Space(20)]

        [Header("Damages of attacks")]
        public float damageAttackA;
        public float damageAttackB;
        public float damageComboAAA01;
        public float damageComboAAA02;
        public float damageComboBB01;
        public float damageComboBSharp;
        public float damageDefinitive;
        public float damageHability;

        #endregion

        #region Public C# Properties

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
                return this.damageHability;
            }
        }
        
        #endregion
    }
}
