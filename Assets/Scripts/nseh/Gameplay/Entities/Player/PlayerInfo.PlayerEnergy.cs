using nseh.Managers.General;
using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {

        #region Private Properties

        private BarComponent _energyBar;
        private float _maxEnergy;

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

        public BarComponent EnergyBar
        {
            set { _energyBar = value; }
        }

        #endregion

        #region Public Methods

        public void IncreaseEnergy(float amount)
        {
            CurrentEnergy += amount;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0.0f, MaxEnergy);
        }

        public void DecreaseEnergy(float amount)
        {
            CurrentEnergy -= amount;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0.0f, MaxEnergy);
        }

        public void DecreaseEnergyByPercent(float percent)
        {
            CurrentEnergy -= (MaxEnergy * (percent / 100.0f));
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0.0f, MaxEnergy);
        }

        public bool CanUseEnergyForDefinitive()
        {
            if ((_currentEnergy ) == _maxEnergy)
            {
                DecreaseEnergy(_maxEnergy);
                return true;
            }

            return false;
        }

        public bool CanUseEnergyForAbility()
        {
            if((_currentEnergy - _maxEnergy * 0.25f) >= 0.0f)
            {
                DecreaseEnergyByPercent(25);
                return true;
            }

            return false;
        }

        #endregion

    }
}


