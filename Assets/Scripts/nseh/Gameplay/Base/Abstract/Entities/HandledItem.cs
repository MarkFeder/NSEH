using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Base.Abstract.Entities
{
    public abstract class Chest : MonoBehaviour
    {
        protected GameObject mesh;
        protected float durationTime;
        protected List<GameObject> targets;
        protected Transform position;

        protected abstract void Activate();
        protected abstract void Deactivate();

        protected virtual void OnTriggerEnter(Collider other) { }
        protected virtual void OnTriggerExit(Collider other) { }
    }

    public enum StandardChestType
    {
        None = 0,
        Health = 1,
        Damage = 2,
        Velocity = 3,
        Jump = 4,
        Defense = 5
    }

    public class StandardChest : Chest
    {
        protected StandardChestType chestType;

        public StandardChest(StandardChestType type, float durationTime, Transform position)
        {
            this.chestType = type;
            this.durationTime = durationTime;
            this.position = position;

            this.targets = new List<GameObject>();
        }

        protected override void Activate()
        {
            switch(this.chestType)
            {
                case StandardChestType.None:
                    break;

                case StandardChestType.Health:

                    break;

                case StandardChestType.Damage:

                    break;

                case StandardChestType.Velocity:

                    break;

                case StandardChestType.Jump:

                    break;

                case StandardChestType.Defense:

                    break;
            }
        }

        protected override void Deactivate()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.PLAYER) && Time.time > this.durationTime)
            {
                GameObject target = other.gameObject;
                this.targets.Add(target);

                // TODO
                this.Activate();
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                GameObject target = other.gameObject;
                this.targets.Add(target);

                // TODO
                this.Deactivate();
            }
        }
    }

    public enum SpecialChestType
    {
        None = 0,
        Invulnerability = 1,
        AutomaticAttacks = 2,
        CriticalDamage = 3,
        FreeDefinitiveMode = 4
    }

    public class SpecialChest : Chest
    {
        protected SpecialChestType chestType;

        protected override void Activate()
        {
            throw new NotImplementedException();
        }

        protected override void Deactivate()
        {
            throw new NotImplementedException();
        }
    }

    public enum DisadvantageChestType
    {
        None = 0,
        ChestBomb = 1,
        PoisonCloud = 2,
        ConfusedPotion = 3
    }

    public class DisadvantageChest : Chest
    {
        protected DisadvantageChestType chestType;

        protected override void Activate()
        {
            throw new NotImplementedException();
        }

        protected override void Deactivate()
        {
            throw new NotImplementedException();
        }
    }
}
