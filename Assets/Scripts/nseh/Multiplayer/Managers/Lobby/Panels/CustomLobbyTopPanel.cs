using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CustomLobby
{
    public class CustomLobbyTopPanel : MonoBehaviour
    {
        #region Public Properties

        public bool IsInGame
        {
            get
            {
                return _isInGame;
            }

            set
            {
                _isInGame = value;
            }
        }

        #endregion

        #region Private Properties

        [SerializeField]
        private bool _isInGame;

        #endregion

        #region Protected Properties

        protected bool _isDisplayed;
        protected Image _panelImage;

        #endregion

        private void Start()
        {
            _isDisplayed = true;
            _isInGame = false;

            _panelImage = GetComponent<Image>();
        }

        private void Update()
        {
            if (!_isInGame)
            {
                return;
            }

            // Handle proper user button here
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleVisibility(!_isDisplayed);
            }
        }

        #region Public Methods

        public void ToggleVisibility(bool visible)
        {
            _isDisplayed = visible;

            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(_isDisplayed);
            }

            if (_panelImage != null)
            {
                _panelImage.enabled = _isDisplayed;
            }
        }

        #endregion
    }
}
