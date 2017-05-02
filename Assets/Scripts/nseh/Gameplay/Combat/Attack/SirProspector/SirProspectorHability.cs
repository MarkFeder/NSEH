using UnityEngine;

namespace nseh.Gameplay.Combat.Attack.SirProspector
{
    public class SirProspectorHability : CharacterAttack
    {
        #region Private Properties

        private float _bonification;
        private float _seconds;
        private float _reusedTime;
        private float _timer;

        #endregion

        #region Protected Methods

        protected override void Start()
        {
            base.Start();

            _bonification = 50.0f;
            _seconds = 10.0f;
            _reusedTime = 20.0f;
            _timer = 0.0f;
        }

        protected void Update()
        {
            _timer += Time.deltaTime;

            if (!_enabled && _timer > _reusedTime)
            {
                Debug.Log(string.Format("Reestablished bonification defense after {0} seconds", _timer));

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
