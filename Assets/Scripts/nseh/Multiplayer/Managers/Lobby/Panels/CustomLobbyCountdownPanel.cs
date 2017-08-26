using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CustomLobby
{
    public class CustomLobbyCountdownPanel : MonoBehaviour
    {
        #region Public Properties

        public Text UIText
        {
            get
            {
                return _uiText;
            }

            set
            {
                _uiText = value;
            }
        }

        #endregion

        #region Private Properties

        [SerializeField]
        private Text _uiText;

        #endregion

        #region Public Methods

        public void DisplayForSeconds(string text, int seconds)
        {
            gameObject.SetActive(true);
            StartCoroutine(DisplayForSecondsInternal(text, seconds));
        }

        #endregion

        #region Private Methods

        internal IEnumerator DisplayForSecondsInternal(string text, int seconds)
        {
            _uiText.text = text;

            yield return new WaitForSeconds(seconds);

            gameObject.SetActive(false);
            _uiText.text = "";
        }

        #endregion
    }
}
