using System;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasClockMinigameHUDManager : MonoBehaviour
    {
        
        #region Public Properties

        public Text _readyText;

        #endregion

        #region Public C# Properties
        
        public Text ReadyText
        {
            get { return _readyText; }
        }

        #endregion

        private void Start()
        {
        }

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
