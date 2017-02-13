using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
using nseh.Utils.Helpers;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Entities.Environment.Items
{
    public enum SpecialChestType
    {
        None = 0,
        Invulnerability = 1,
        AutomaticAttacks = 2,
        CriticalDamage = 3,
        UnlockDefinitiveMode = 4
    }

    public class SpecialChest : Chest
    {
        protected SpecialChestType chestType;

        protected override void Activate()
        {
            switch (this.chestType)
            {
                case SpecialChestType.None:

                    Debug.Log("None effect");

                    break;

                case SpecialChestType.Invulnerability:

                    this.Invulnerability(10.0f);

                    break;

                case SpecialChestType.AutomaticAttacks:

                    this.AutomaticAttacks(2);

                    break;

                case SpecialChestType.CriticalDamage:

                    this.CriticalDamage(2);

                    break;

                case SpecialChestType.UnlockDefinitiveMode:

                    this.UnlockDefinitiveMode();

                    break;

                default:

                    Debug.Log("No StandardChestType is detected");

                    break;
            }
        }

        private void Invulnerability(float time)
        {
            this.target.GetComponent<CharacterHealth>().InvulnerabilityModeForSeconds(time);
        }

        private void AutomaticAttacks(int times)
        {
            // TODO: not defined special attacks
        }

        private void CriticalDamage(int times)
        {
            // For each action of type CharacterAttack which is not of CharacterHability or CharacterDefinitive type,
            // increase its damage by (x) times.

            this.target.GetComponent<CharacterCombat>().Actions.OfType<CharacterAttack>().Where(act => act.IsSimpleAttack).ForEach(act =>
            {
                string log = "the attack of type: " + act.AttackType.ToString() + " changed its damage from (" + act.CurrentDamage + ")";

                act.CurrentDamage = act.CurrentDamage * times;
                act.Critical = true;

                log += " to (" + act.CurrentDamage + ")";

                Debug.Log(log);
            });
        }

        private void UnlockDefinitiveMode()
        {
            var definitiveAttack = this.target.GetComponent<CharacterCombat>().Actions.OfType<CharacterAttack>().Where(act =>
            {
                return act.AttackType == AttackType.CharacterDefinitive;

            }).FirstOrDefault();

            if (definitiveAttack != null)
            {
                definitiveAttack.EnabledAttack = true;
            }
        }

        protected override void Deactivate()
        {
            throw new NotImplementedException();
        }
    }

}
