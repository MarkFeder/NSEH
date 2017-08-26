using CustomLobby.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Inputs = nseh.Utils.Constants.Input;

namespace CustomLobby
{
    public class CustomLobbyPlayer : NetworkLobbyPlayer
    {
        #region Public Properties

        [SyncVar(hook = "OnMyColor")]
        public Color _playerColor = Color.white;
        [SyncVar(hook = "OnMyName")]
        public string _playerName;
        [SyncVar(hook = "OnMyPortrait")]
        public int _playerPortrait = 0;

        #endregion

        #region Private Properties

        [Space(10)]
        [Header("UI - Portraits")]
        [SerializeField]
        private Sprite[] _portraits;

        [Space(10)]
        [Header("UI - Buttons")]
        [SerializeField]
        private Button _portraitButton;
        [SerializeField]
        private Button _colorButton;
        [SerializeField]
        private Button _readyButton;
        [SerializeField]
        private Button _waitingPlayerButton;
        [SerializeField]
        private Button _removePlayerButton;

        [Space(10)]
        [Header("UI - Fields")]
        [SerializeField]
        private InputField _nameInput;

        private NetworkIdentity _playerIdentity;

        private string _playerInput;

        private static Color[] _colors = new Color[]
        {
            Color.magenta,
            Color.red,
            Color.cyan,
            Color.blue,
            Color.green,
            Color.yellow
        };

        // to avoid assigning the same color to other client
        private static List<int> _colorInUse = new List<int>();

        #endregion

        #region Public Methods

        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            int localPlayerCount = 1;

            // Modify for more than one local player
            _removePlayerButton.interactable = localPlayerCount > 1;
        }

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            _playerInput = String.Format("{0}1", Inputs.B);
            _playerIdentity = GetComponent<NetworkIdentity>();

            if (CustomLobbyManager.Instance != null)
            {
                CustomLobbyManager.Instance.OnPlayersNumberModified(1);
            }

            CustomLobbyPlayerList.Instance.AddPlayer(this);

            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            // Setup player data on UI. The values are SyncVar, so the player
            // will be created with the right value currently on server
            OnMyName(_playerName);
            OnMyPortrait(_playerPortrait);
            OnMyColor(_playerColor);
        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                Text textComponent = _readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY";

                _readyButton.interactable = false;
                _portraitButton.interactable = false;
                _nameInput.interactable = false;
            }
            else
            {
                Text textComponent = _readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "JOIN" : "...";
                textComponent.color = Color.white;

                _readyButton.interactable = isLocalPlayer;
                _portraitButton.interactable = isLocalPlayer;
                _nameInput.interactable = isLocalPlayer;
            }
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            Debug.Log(string.Format("OnStartAuthority: [netId={0}; name={1}]", _playerIdentity.netId, _playerIdentity.name));

            // Returning from a game, color of text can still be the one for "Ready"
            _readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            _portraitButton.GetComponent<Image>().sprite = _portraits[0];

            SetupLocalPlayer();
        }

        public void OnPlayerListChanged(int idx)
        {
            // GetComponent<Image>
        }

        // -------- CALLBACKS -------- //

        public void OnMyName(string newName)
        {
            _playerName = newName;
            _nameInput.text = _playerName;
        }

        public void OnMyPortrait(int newPortrail)
        {
            _playerPortrait = newPortrail;
            _portraitButton.GetComponent<Image>().sprite = _portraits[newPortrail];
        }

        public void OnMyColor(Color newColor)
        {
            _playerColor = newColor;
            _colorButton.GetComponent<Image>().color = newColor;
        }

        // -------- CALLBACKS -------- //

        // -------- UI HANDLER ------- //
        // Note that those handler use Command function, as we need to change the value on the server 
        // not locally so that all client get the new value throught syncvar

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnPortraitClicked()
        {
            CmdPortraitChanged();
        }

        public void OnColorClicked()
        {
            CmdColorChange();
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnRemovePlayerClick()
        {
            string debugMsg = isLocalPlayer ? "I'm a local Player" : "I'm a server";
            Debug.Log("OnRemovePlayerClick: " + debugMsg);

            if (isLocalPlayer)
            {
                RemovePlayer();
            }
            else if (isServer)
            {
                CustomLobbyManager.Instance.KickPlayer(connectionToClient);
            }
        }

        public void ToggleJoinButton(bool enabled)
        {
            _readyButton.gameObject.SetActive(enabled);
            _waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            CustomLobbyManager.Instance.CountDownPanel.UIText.text = "Match Starting in " + countdown;
            CustomLobbyManager.Instance.CountDownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton()
        {
            CheckRemoveButton();
        }

        // -------- UI HANDLER ------- //

        // -------- SERVER ----------- //

        [Command]
        public void CmdPortraitChanged()
        {
            Debug.Log(string.Format("CmdPortraitChanged: [netId={0}; name={1}]", _playerIdentity.netId, _playerIdentity.name));

            int oldPortrait = _playerPortrait;

            int idx = _playerPortrait;

            idx = (idx < 0) ? 0 : idx;

            idx = (idx + 1) % _portraits.Length;

            _playerPortrait = idx;

            CustomLobbyManager.Instance.SetPlayerTypeLobby(_playerIdentity.connectionToClient, _playerPortrait);

            Debug.Log(string.Format("CmdPortraitChanged: [oldPortrait={0}; newPortrait={1}]", oldPortrait, _playerPortrait));
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            _playerName = name;
        }

        [Command]
        public void CmdColorChange()
        {
            Debug.Log(string.Format("CmdColorChange: [netId={0}; name={1}; color={2}]", _playerIdentity.netId, _playerIdentity.name, _playerColor));

            int idx = Array.IndexOf(_colors, _playerColor);

            int inUseIdx = _colorInUse.IndexOf(idx);

            if (idx < 0) idx = 0;

            idx = (idx + 1) % _colors.Length;

            bool alreadyInUse = false;

            do
            {
                alreadyInUse = false;
                for (int i = 0; i < _colorInUse.Count; ++i)
                {
                    if (_colorInUse[i] == idx)
                    {
                        // that color is already in use
                        alreadyInUse = true;
                        idx = (idx + 1) % _colors.Length;
                    }
                }
            }
            while (alreadyInUse);

            if (inUseIdx >= 0)
            {
                // if we already add an entry in the colorTabs, we change it
                _colorInUse[inUseIdx] = idx;
            }
            else
            {
                // we add it to the list
                _colorInUse.Add(idx);
            }

            _playerColor = _colors[idx];

            Debug.Log(string.Format("CmdColorChange: [netId={0}; name={1}; color={2}]", _playerIdentity.netId, _playerIdentity.name, _playerColor));
        }

        // -------- SERVER ----------- //

        #endregion

        #region Private Methods

        private void Update()
        {
            // Simulate GoBackButton until we have a real one
            if (!String.IsNullOrEmpty(_playerInput) && Input.GetButtonDown(_playerInput))
            {
                OnRemovePlayerClick();
                if (CustomLobbyManager.Instance != null)
                {
                    CustomLobbyManager.Instance.GoBackButton();
                }
            }
        }

        private void OnDestroy()
        {
            CustomLobbyPlayerList.Instance.RemovePlayer(this);
            if (CustomLobbyManager.Instance != null)
            {
                CustomLobbyManager.Instance.OnPlayersNumberModified(-1);
            }
        }

        private void SetupOtherPlayer()
        {
            _nameInput.interactable = true;
            _removePlayerButton.interactable = NetworkServer.active;

            // ChangeReadyButtonColor(NotReadyColor)

            _readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            _readyButton.interactable = false;

            OnClientReady(false);
        }

        private void SetupLocalPlayer()
        {
            _nameInput.interactable = true;
            _colorButton.interactable = true;

            CheckRemoveButton();

            if (_playerPortrait == 0)
            {
                CmdPortraitChanged();
            }

            if (_playerColor == Color.white)
            {
                CmdColorChange();
            }

            // ChangeReadyButtonColor()

            _readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
            _readyButton.interactable = true;

            if (String.IsNullOrEmpty(_playerName))
            {
                CmdNameChanged("Player " + (CustomLobbyPlayerList.Instance.PlayerListContentTransform.childCount - 1));
            }

            _nameInput.interactable = true;
            _portraitButton.interactable = true;

            // Setup all listeners for sync vars
            _nameInput.onEndEdit.RemoveAllListeners();
            _nameInput.onEndEdit.AddListener(OnNameChanged);

            _colorButton.onClick.RemoveAllListeners();
            _colorButton.onClick.AddListener(OnColorClicked);

            _portraitButton.onClick.RemoveAllListeners();
            _portraitButton.onClick.AddListener(OnPortraitClicked);

            _readyButton.onClick.RemoveAllListeners();
            _readyButton.onClick.AddListener(OnReadyClicked);


            if (CustomLobbyManager.Instance != null)
            {
                CustomLobbyManager.Instance.OnPlayersNumberModified(0);
            }
        }

        #endregion
    }
}
