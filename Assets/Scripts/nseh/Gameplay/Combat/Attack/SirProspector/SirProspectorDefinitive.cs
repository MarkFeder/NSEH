using nseh.Utils.Helpers;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Combat.Attack.SirProspector
{
    public class SirProspectorDefinitive : CharacterAttack
    {
        #region Private Properties

        [SerializeField]
        private float _percent;
        [SerializeField]
        private float _seconds;

        [SerializeField]
        private MeshRenderer _sword;
        [SerializeField]
        private MeshRenderer _shovel;

        private const string _group = "SirProspectorDefinitive";

        #endregion

        protected override void Start()
        {
            base.Start();

            _sword.enabled = false;
            _shovel.enabled = true;
        }

        public override void StartAction()
        {
            base.StartAction();

            StartCoroutine(ExecuteDefinitiveAction());
        }

        #region Private Methods

        private IEnumerator ExecuteDefinitiveAction()
        {
            _shovel.enabled = false;
            _sword.enabled = true;

            var attacks = _playerInfo.PlayerCombat.Actions.OfType<CharacterAttack>().ToArray();
            RunInfo info = null;

            foreach (CharacterAttack attack in attacks)
            {
                info = attack.IncreaseDamageForSecondsExternal(_percent, _seconds).ParallelCoroutine(_group);
            }
            while (info.count > 0) yield return null;

            _shovel.enabled = true;
            _sword.enabled = false;
        }

        #endregion
    }
}
