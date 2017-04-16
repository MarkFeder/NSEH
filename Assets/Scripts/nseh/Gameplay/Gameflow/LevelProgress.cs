using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Level;
using nseh.Managers.General;
using nseh.Managers.Main;
using nseh.Utils;
using System;

namespace nseh.Gameplay.Gameflow
{
    public class LevelProgress : LevelEvent
    {
        #region Private Properties
        private BarComponent _progressBar;
        private float _maxProgress;
        private float _currentProgress;
        #endregion

        #region Public C# Properties
        public float MaxProgress
        {
            get
            {
                return this._maxProgress;
            }

            set
            {
                this._maxProgress = value;
                if(this._progressBar != null)
                {
                    this._progressBar.MaxValue = this._maxProgress;
                }
            }
        }

        public float CurrentProgress
        {
            get
            {
                return this._currentProgress;
            }

            set
            {
                this._currentProgress = value;
                if (this._progressBar != null)
                {
                    this._progressBar.Value = this._currentProgress;
                }
            }
        }
        #endregion

        public override void ActivateEvent()
        {
            IsActivated = true;
            this._progressBar = LvlManager.CanvasProgressManager.ProgressBar;
            MaxProgress = CalculateMaxProgress();
            this.CurrentProgress = 0;
        }

        public override void EventTick()
        {
            if(CurrentProgress >= MaxProgress)
            {
                LvlManager.ChangeState(LevelManager.States.LoadingMinigame);
            }
        }

        public override void EventRelease()
        {
            this.CurrentProgress = 0;
            IsActivated = false;
        }

        #region Public Methods

        public void IncreaseProgress(float amount)
        {
            this.CurrentProgress += amount;
            this.CurrentProgress = Mathf.Clamp(this.CurrentProgress, 0.0f, this.MaxProgress);
        }

        public void DecreaseProgress(float amount)
        {
            this.CurrentProgress -= amount;
            this.CurrentProgress = Mathf.Clamp(this.CurrentProgress, 0.0f, this.MaxProgress);
        }

        #endregion

        #region Private Methods

        private float CalculateMaxProgress()
        {
            float aux = 0;
            Debug.Log("asdasrasdr " + LvlManager.Players.Count);
            for(int i = 0; i<LvlManager.Players.Count; i++)
            {
                aux += LvlManager.Players[i].PlayerRunTimeInfo.PlayerHealth.maxHealth * 3;
            }
            return aux / 2;
        }

        #endregion
    }
}


