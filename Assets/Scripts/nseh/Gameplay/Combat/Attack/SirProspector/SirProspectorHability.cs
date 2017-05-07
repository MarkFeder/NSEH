using UnityEngine;

namespace nseh.Gameplay.Combat.Attack.SirProspector
{
    public class SirProspectorHability : CharacterAttack
    {
        #region Private Properties

        [SerializeField]
        private float _bonification;
        [SerializeField]
        private float _seconds;
        [SerializeField]
        private float _reusedTime;

        private float _timer;

        #endregion

        #region Protected Methods

        protected override void Start()
        {
            base.Start();
            _timer = 0.0f;
        }

        protected void Update()
        {
            _timer += Time.deltaTime;

            if (!_enabled && _timer > _reusedTime)
            {
                // Enable attack again
                _enabled = true;
                _timer = 0.0f;
            }
        }

        #endregion

        #region Public Methods

        public override void StartAction()
        {
            if (_enabled)
            {
                base.StartAction();

                // Apply bonification defense
                _playerInfo.PlayerHealth.BonificationDefenseForSeconds(_bonification, _seconds);

                // Reestablish timer
                _enabled = false;
                _timer = 0.0f;
            }
        }

        #endregion
    }
}
