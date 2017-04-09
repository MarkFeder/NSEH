using System;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasItemsHUDManager : MonoBehaviour
    {
        #region Public Properties

        public Text _mainText;
        public Text _p1ItemText;
        public Text _p2ItemText;

        #endregion

        #region Public C# Properties

        public Text MainText
        {
            get { return _mainText; }
        }

        public Text P1ItemText
        {
            get { return _p1ItemText; }
        }

        public Text P2ItemText
        {
            get { return _p2ItemText; }
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
            return _mainText && _p1ItemText && _p2ItemText; // includes p3Portrait and p4Portrait when available
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

        #endregion
    }
}
