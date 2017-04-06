using System;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasClockHUDManager : MonoBehaviour
    {
        #region Public Properties

        public Text clockText;

        #endregion

        #region Public C# Properties

        public Text ClockText
        {
            get { return clockText; }
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
            return clockText;
        }

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }

        public void ChangeClockText(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                clockText.text = text;
            }
        }

        #endregion
    }
}
