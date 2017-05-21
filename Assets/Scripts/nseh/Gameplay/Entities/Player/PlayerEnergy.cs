using nseh.Gameplay.Combat;
using nseh.Managers.General;
using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        #region Private Properties

        private BarComponent _energyBar;
        private PlayerInfo _playerInfo;

        [SerializeField]
        private float _maxEnergy;
        [SerializeField]
        private float _currentEnergy;

        #endregion

        #region Public C# Properties

        public float MaxEnergy
        {
            get { return _maxEnergy; }
            set
            {
                _maxEnergy = value;
                if(_energyBar != null)
                {
                    _energyBar.MaxValue = _maxEnergy;
                }
            }
        }

        public float CurrentEnergy
        {
            get { return _currentEnergy; }
            set
            {
                _currentEnergy = value;
                if (_energyBar != null)
                {
                    _energyBar.Value = _currentEnergy;
                }
            }
        }

        public bool IsValidEnergy
        {
            get { return _currentEnergy >= _maxEnergy && 
                        (_currentEnergy > 0.0f && _maxEnergy > 0.0f); }
        }

        public bool CanUseEnergyForDefinitive
        {
            get { return _currentEnergy >= _maxEnergy; }
        }

        public bool CanUseEnergyForHability
        {
            get { return _currentEnergy > 0.0f && 
                        (_currentEnergy - _maxEnergy * 0.25f) >= 0.0f; }
        }

        public BarComponent EnergyBar
        {
            set { _energyBar = value; }
        }

        #endregion

        public void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();

            MaxEnergy = _maxEnergy;
            CurrentEnergy = _currentEnergy;
            // CurrentEnergy = 0;
        }

        public void Update()
        {
        }

        public void ResetEnergy()
        {
            CurrentEnergy = 0;

            Debug.Log("The energy was reseted to 0");
        }

        #region Public Methods

        public void IncreaseEnergy(float amount)
        {
            float oldEnergy = _currentEnergy;

            CurrentEnergy += amount;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0.0f, MaxEnergy);

            Debug.Log(string.Format("Energy has been increased from: {0} to: {1}", oldEnergy, _currentEnergy));
        }

        public void DecreaseEnergy(float amount)
        {
            float oldEnergy = _currentEnergy;

            CurrentEnergy -= amount;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0.0f, MaxEnergy);

            Debug.Log(string.Format("Energy has been reduced from: {0} to: {1}", oldEnergy, _currentEnergy));
        }


        /// <summary>
        /// Decrease energy by a x percent
        /// </summary>
        /// <param name="percent">It should be of this type: 20(%), 30(%), etc.</param>
        public void DecreaseEnergyByPercent(float percent)
        {
            CurrentEnergy -= (MaxEnergy * (percent / 100.0f));
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0.0f, MaxEnergy);

            Debug.Log(string.Format("Energy has been reduced in: {0}%", percent));
        }

        #endregion

        #region Private Methods

        #endregion
    }
}


