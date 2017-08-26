using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CustomLobby
{
    public class CustomLobbyMainMenu : MonoBehaviour
    {
        #region Private Properties

        [SerializeField]
        private CustomLobbyManager _lobbyManager;

        [SerializeField]
        private RectTransform _lobbyServerList;
        [SerializeField]
        private RectTransform _lobbyPanel;

        [SerializeField]
        private InputField _ipInput;
        [SerializeField]
        private InputField _matchNameInput;

        #endregion

        #region Public Methods

        public void OnEnable()
        {
            _lobbyManager.TopPanel.ToggleVisibility(true);

            _ipInput.onEndEdit.RemoveAllListeners();
            _ipInput.onEndEdit.AddListener(onEndEditIP);

            //_matchNameInput.onEndEdit.RemoveAllListeners();
            //_matchNameInput.onEndEdit.AddListener(onEndEditGameName);
        }

        public void OnClickHost()
        {
            _lobbyManager.StartHost();
        }

        public void OnClickJoin()
        {
            _lobbyManager.ChangeTo(_lobbyPanel);

            _lobbyManager.networkAddress = _ipInput.text;
            _lobbyManager.StartClient();

            _lobbyManager._backDelegate = _lobbyManager.StopClientClbk;
            _lobbyManager.DisplayIsConnecting();

            _lobbyManager.SetServerInfo("Connecting...", _lobbyManager.networkAddress);
        }

        public void OnClickDedicated()
        {
            _lobbyManager.ChangeTo(null);
            _lobbyManager.StartServer();

            _lobbyManager._backDelegate = _lobbyManager.StopServer;

            _lobbyManager.SetServerInfo("Dedicated Server", _lobbyManager.networkAddress);
        }

        //public void OnClickCreateMatchmakingGame()
        //{
        //    _lobbyManager.StartMatchMaker();
        //    _lobbyManager.matchMaker.CreateMatch(
        //        _matchNameInput.text,
        //        (uint)_lobbyManager.maxPlayers,
        //        true,
        //        "", "", "", 0, 0,
        //        _lobbyManager.OnMatchCreate);

        //    _lobbyManager._backDelegate = _lobbyManager.StopHost;
        //    // _lobbyManager._isMatchmaking = true;
        //    _lobbyManager.DisplayIsConnecting();

        //    _lobbyManager.SetServerInfo("Matchmaker Host", _lobbyManager.matchHost);
        //}

        public void OnClickOpenServerList()
        {
            _lobbyManager.StartMatchMaker();
            _lobbyManager._backDelegate = _lobbyManager.SimpleBackClbk;
            _lobbyManager.ChangeTo(_lobbyServerList);
        }

        #endregion

        #region Private Methods

        private void onEndEditIP(string text)
        {
            // TODO: Change Input here
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }

        private void onEndEditGameName(string text)
        {
            // TODO: Change Input here
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // OnClickCreateMatchmakingGame();
            }
        } 

        #endregion
    }
}
