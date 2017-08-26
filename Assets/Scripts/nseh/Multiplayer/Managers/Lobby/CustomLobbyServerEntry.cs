using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

namespace CustomLobby
{
    public class CustomLobbyServerEntry : MonoBehaviour 
    {
        #region Public Properties

        public Text ServerInfoText
        {
            get
            {
                return _serverInfoText;
            }
        }

        public Text SlotInfo
        {
            get
            {
                return _slotInfo;
            }
        }

        public Button JoinButton
        {
            get
            {
                return _joinButton;
            }
        }

        #endregion

        #region Private Properties

        [SerializeField]
        private Text _serverInfoText;
        [SerializeField]
        private Text _slotInfo;
        [SerializeField]
        private Button _joinButton;

        #endregion

        #region Public Methods

        //public void Populate(MatchInfoSnapshot match, CustomLobbyManager lobbyManager, Color c)
        //{
        //    _serverInfoText.text = match.name;

        //    _slotInfo.text = match.currentSize.ToString() + "/" + match.maxSize.ToString(); ;

        //    NetworkID networkID = match.networkId;

        //    _joinButton.onClick.RemoveAllListeners();
        //    _joinButton.onClick.AddListener(() => { JoinMatch(networkID, lobbyManager); });

        //    GetComponent<Image>().color = c;
        //}

        #endregion

        #region Private Properties

        //private void JoinMatch(NetworkID networkID, CustomLobbyManager lobbyManager)
        //{
        //    lobbyManager.matchMaker.JoinMatch(networkID, "", "", "", 0, 0, lobbyManager.OnMatchJoined);
        //    lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        //    lobbyManager._isMatchmaking = true;
        //    lobbyManager.DisplayIsConnecting();
        //}

        #endregion
    }
}