using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Level;
using nseh.Managers.General;
using nseh.Managers.Main;
using nseh.Utils;
using System;

namespace nseh.Gameplay.Entities.Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        #region Private Properties
        private BarComponent _energyBar;
        private PlayerInfo playerInfo;
        private float _maxEnergy;
        private float _currentEnergy;
        #endregion

        #region Public C# Properties
        public float MaxEnergy
        {
            get
            {
                return this._maxEnergy;
            }

            set
            {
                this._maxEnergy = value;
                if(this._energyBar != null)
                {
                    this._energyBar.MaxValue = this._maxEnergy;
                }
            }
        }

        public float CurrentEnergy
        {
            get
            {
                return this._currentEnergy;
            }

            set
            {
                this._currentEnergy = value;
                if (this._energyBar != null)
                {
                    this._energyBar.Value = this._currentEnergy;
                }
            }
        }

        public BarComponent EnergyBar
        {
            set
            {
                _energyBar = value;
            }
        }
        #endregion

        public void Start()
        {
            this.playerInfo = this.GetComponent<PlayerInfo>();
            this.MaxEnergy = 200;
            this.CurrentEnergy = 0;
        }

        public void Update()
        {
            if (CurrentEnergy >= MaxEnergy)
            {
                Debug.Log("YOU ARE SO EPIC " + this.gameObject.name + "! Energy (" + this.CurrentEnergy + ").");
            }
            else
            {
                Debug.Log("Current energy of " + this.gameObject.name + "is " + this.CurrentEnergy);
            }
        }

        public void ResetEnergy()
        {
            this.CurrentEnergy = 0;
        }

        #region Public Methods

        public void IncreaseEnergy(float amount)
        {
            this.CurrentEnergy += amount;
            this.CurrentEnergy = Mathf.Clamp(this.CurrentEnergy, 0.0f, this.MaxEnergy);
        }

        public void DecreaseEnergy(float amount)
        {
            this.CurrentEnergy -= amount;
            this.CurrentEnergy = Mathf.Clamp(this.CurrentEnergy, 0.0f, this.MaxEnergy);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}


