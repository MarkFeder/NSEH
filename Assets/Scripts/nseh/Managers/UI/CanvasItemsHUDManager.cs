using System;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasItemsHUDManager : MonoBehaviour
    {
        #region Public Properties

        public Text mainText;
        public Text p1ItemText;
        public Text p2ItemText;

        #endregion

        #region Public C# Properties

        public Text MainText
        {
            get { return mainText; }
        }

        public Text P1ItemText
        {
            get { return p1ItemText; }
        }

        public Text P2ItemText
        {
            get { return p2ItemText; }
        }
        #endregion

        private void Start()
        {
            if (!ValidateItems())
            {
                Debug.Log("One or more of the players' items are null");
                enabled = false;
                return;
            }
        }

        #region Private Methods

        private bool ValidateItems()
        {
            return mainText && p1ItemText && p2ItemText; // includes p3Portrait and p4Portrait when available
        }

        #endregion

        #region Public Methods

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }

        public void ChangeP1ItemText(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                p1ItemText.text = text;
            }
        }

        public void ChangeP2ItemText(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                p2ItemText.text = text;

            }
        }

        #endregion
    }
}
