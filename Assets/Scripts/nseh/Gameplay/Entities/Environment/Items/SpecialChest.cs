using nseh.Gameplay.Base.Abstract.Entities;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Entities.Player;
using nseh.Utils.Helpers;
using System.Linq;
using UnityEngine;
using SpecialItems = nseh.Utils.Constants.Items.SpecialItems;

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
        #region Public Properties

        public SpecialChestType chestType;

        public float seconds;
        public int times; 

        #endregion

        protected override void Activate()
        {
            switch (this.chestType)
            {
                case SpecialChestType.None:

                    Debug.Log("None effect");

                    break;

                case SpecialChestType.Invulnerability:

                    this.Invulnerability(this.seconds);
                    this.spawnItemPoint.DisplayText(itemText, SpecialItems.INVULNERABILITY, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, this.seconds, particlesSpawnPoints.ParticleBodyPos);

                    break;

                case SpecialChestType.AutomaticAttacks:

                    this.AutomaticAttacks(this.times);
                    this.spawnItemPoint.DisplayText(itemText, SpecialItems.AUTOATTACKS, this.timeToDisplayText);
                    this.ParticleAnimation(particlePrefab, 3.0f, particlesSpawnPoints.ParticleBodyPos);

                    break;

                case SpecialChestType.CriticalDamage:

                    this.CriticalDamage(this.times);
                    this.spawnItemPoint.DisplayText(itemText, SpecialItems.CRITICAL, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, 3.0f, particlesSpawnPoints.ParticleBodyPos);

                    break;

                case SpecialChestType.UnlockDefinitiveMode:

                    this.UnlockDefinitiveMode();
                    this.spawnItemPoint.DisplayText(itemText, SpecialItems.ULTIMATE, this.timeToDisplayText);
                    this.ParticleAnimation(this.particlePrefab, 1.0f, particlesSpawnPoints.ParticleBodyPos);

                    break;

                default:

                    Debug.Log("No SpecialChestType is detected");

                    break;
            }

            this.Deactivate();
        }

        protected override void Deactivate()
        {
            Destroy(this.gameObject, this.destructionTime);
        }

        private void Invulnerability(float time)
        {
            this.target.GetComponent<PlayerHealth>().InvulnerabilityModeForSeconds(time);
        }

        private void AutomaticAttacks(int times)
        {
            // TODO: not defined special attacks
        }

        private void CriticalDamage(int times)
        {
            // For each action of type CharacterAttack which is not of CharacterHability or CharacterDefinitive type,
            // increase its damage by (x) times.

            this.target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().Where(act => act.IsSimpleAttack).ForEach(act =>
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
            var definitiveAttack = this.target.GetComponent<PlayerCombat>().Actions.OfType<CharacterAttack>().Where(act =>
            {
                return act.AttackType == AttackType.CharacterDefinitive;

            }).FirstOrDefault();

            if (definitiveAttack != null)
            {
                definitiveAttack.EnabledAttack = true;
            }
        }
    }

}
