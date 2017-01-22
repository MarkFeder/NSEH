using nseh.Gameplay.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace nseh.Gameplay.Combat.Attack
{
    public enum ImpactType
    {
        None = 0,
        Normal = 1
    }

    public class CharacterImpact : HandledAction
    {
        public ImpactType ImpactType { get; set; }

        public CharacterImpact(ImpactType impactType, int hashAnimation, string stateName, Animator animator)
            :base(hashAnimation, stateName, animator)
        {
            this.ImpactType = ImpactType;
        }
    }
}
