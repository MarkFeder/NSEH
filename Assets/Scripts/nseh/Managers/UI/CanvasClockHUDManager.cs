using System;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasClockHUDManager : MonoBehaviour
    {
        #region Public Properties

        public Text _clockText;
        public Text _readyText;

        #endregion

        #region Public C# Properties

        public Text ClockText
        {
            get { return _clockText; }
        }

        public Text ReadyText
        {
            get { return _readyText; }
        }

        #endregion

        private void Start()
        {
            if (!ValidateTextClock())
            {
                Debug.Log("The text clock is null");
                enabled = false;
                return;
            }
        }

        #region Public Methods

        public bool ValidateTextClock()
        {
            return _clockText;
        }

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
