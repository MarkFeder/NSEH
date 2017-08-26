using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomLobby
{
    public class CustomLobbyInfoPanel : MonoBehaviour
    {
        #region Public Properties

        public Text InfoText
        {
            get
            {
                return _infoText;
            }
        }

        #endregion

        #region Private Properties

        [SerializeField]
        private Text _infoText;
        [SerializeField]
        private Text _buttonText;

        [SerializeField]
        private Button _singleButton;

        #endregion

        public void Display(string info, string buttonInfo, UnityEngine.Events.UnityAction buttonClbk)
        {
            // Change texts on info text and button text
            _infoText.text = info;
            _buttonText.text = buttonInfo;

            // Add listeners to the button
            _singleButton.onClick.RemoveAllListeners();
            if (buttonClbk != null)
            {
                _singleButton.onClick.AddListener(buttonClbk);
            }
            _singleButton.onClick.AddListener(() => { gameObject.SetActive(false); });

            // Display panel with new changes
            gameObject.SetActive(true);
        }
    }
}
