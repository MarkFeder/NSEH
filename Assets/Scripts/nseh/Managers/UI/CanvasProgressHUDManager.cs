using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.General;

namespace nseh.Managers.UI
{
    public class CanvasProgressHUDManager : MonoBehaviour
    {
        #region Private Properties
        [SerializeField]
        private BarComponent _progressBar;
        #endregion

        #region Public C# Properties
        public BarComponent ProgressBar
        {
            get
            {
                return this._progressBar;
            }
        }
        #endregion

        void Start()
        {
            if (!ValidateProgressBar())
            {
                Debug.Log("Progress Bar is null");
                enabled = false;
                return;
            }
        }

        #region Private Methods
        private bool ValidateProgressBar()
        {
            return this._progressBar;
        }
        #endregion

        #region Public Methods
        public void EnableCanvas()
        {
            this.gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
