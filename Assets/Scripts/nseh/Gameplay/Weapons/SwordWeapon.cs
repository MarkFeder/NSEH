using nseh.Gameplay.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nseh.Gameplay.Weapons
{
    public class SwordWeapon : Weapon
    {
        protected override void Awake()
        {
            base.Awake();

            this.amountDamage = 10.0f;
        }
    }
}
