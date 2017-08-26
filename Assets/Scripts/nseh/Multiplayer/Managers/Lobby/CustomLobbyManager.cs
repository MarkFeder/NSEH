using CustomLobby.Hooks;
using CustomLobby.Utils;
using nseh.Gameplay.Base.Interfaces.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using nseh.Managers.Main;
using System;

namespace CustomLobby
{
    public class CustomLobbyManager : NetworkLobbyManager, IService
    {
        #region Public Properties

        public static CustomLobbyManager Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}

				return null;
			}
		}

        public CustomLobbyCountdownPanel CountDownPanel
        {
            get
            {
                return _countdownPanel;
            }

            set
            {
                _countdownPanel = value;
            }
        }

        public CustomLobbyTopPanel TopPanel
        {
            get
            {
                return _topPanel;
            }
        }

        public bool IsActivated
        {
            get
            {
                return _isActivated;
            }

            set
            {
                _isActivated = value;
            }
        }

        public delegate void BackButtonDelegate();
        public BackButtonDelegate _backDelegate;

        #endregion

        #region Private Properties

        [SerializeField]
        private Character _chosenCharacter = Character.None;

        [Header("Unity UI Lobby")]
        [Tooltip("Time in second between all players ready & match start")]
        [SerializeField]
        private float _prematchCountdown = 5.0f;

        [Space]
        [Header("UI Reference")]

        [SerializeField]
        private RectTransform _mainMenuPanel;
        [SerializeField]
        private RectTransform _lobbyPanel;
        private RectTransform _currentPanel;

        [SerializeField]
        private CustomLobbyTopPanel _topPanel;
        [SerializeField]
        private CustomLobbyInfoPanel _infoPanel;
        [SerializeField]
        private CustomLobbyCountdownPanel _countdownPanel;

        [SerializeField]
        private Text _statusInfo;
        [SerializeField]
        private Text _hostInfo;

        private static short MsgKicked = MsgType.Highest + 1;
		private static CustomLobbyManager _instance;

		private int _playerNumber;

        private Dictionary<int, int> _currentPlayers;

		#endregion

		#region Protected Properties

		protected bool _disconnectServer;

        protected bool _isActivated;

        protected ulong _currentMatchID;

        protected LobbyHook _lobbyHooks;

		#endregion

        private void Start()
        {
            _instance = this;
            _currentPanel = _mainMenuPanel;
            _lobbyHooks = GetComponent<LobbyHook>();
            GetComponent<Canvas>().enabled = true;

            _playerNumber = 0;
            _currentPlayers = new Dictionary<int, int>();
            _disconnectServer = false;

            SetServerInfo("Offline", "None");
        }

        #region Public Methods - IService

        public void Setup(GameManager manager)
        {
        }

        public void Activate()
        {
            _isActivated = true;
        }

        public void Tick()
        {
        }

        public void Release()
        {
            _isActivated = false;
        }

        #endregion

        #region Private Methods - Server Side

        // Do not use this method yet.
        private void LoadPlayerPrefab(NetworkConnection connection, short playerControllerId, Character chosenCharacter)
		{
			// A dict for this could be perfect: (idCharacter) -> (resourceLocation)
			GameObject characterPrefab = null;

			switch (chosenCharacter)
			{
				case Character.None:
					Debug.Log("Nothing to load ...");
					break;

				case Character.Wrarr:
					Debug.Log("Loading Wrarr ...");
					characterPrefab = Instantiate(Resources.Load("Wrarr", typeof(GameObject))) as GameObject;
					break;

				case Character.SirProspector:
					Debug.Log("Loading SirProspector ...");
					characterPrefab = Instantiate(Resources.Load("SirProspector", typeof(GameObject))) as GameObject;
					break;

				case Character.Granhilda:
					Debug.Log("Loading Granhilda ...");
					characterPrefab = Instantiate(Resources.Load("Granhilda", typeof(GameObject))) as GameObject;
					break;

				case Character.MySon:
					Debug.Log("Loading MySon ...");
					characterPrefab = Instantiate(Resources.Load("MySon", typeof(GameObject))) as GameObject;
					break;
			}

			if (characterPrefab != null)
			{
				NetworkServer.AddPlayerForConnection(connection, characterPrefab, playerControllerId);
			}
		}

        private IEnumerator ServerCountdownCoroutine()
        {
            float remainingTime = _prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {
                    //to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {
                            //there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            (lobbySlots[i] as CustomLobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as CustomLobbyPlayer).RpcUpdateCountdown(0);
                }
            }

            ServerChangeScene(playScene);
        }

        #endregion

        #region Private Methods - Client Side

        private void OnFailedToConnect(NetworkConnectionError error)
        {
            Debug.LogError("Could not connect to server: " + error);
        }

        #endregion

        #region Public Methods - Client Side

        public override void OnLobbyClientDisconnect(NetworkConnection conn)
        {
            Debug.LogError(string.Format("OnLobbyClientDisconnect: [{0}]", conn.lastError));
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            Debug.Log(String.Format("OnLobbyClientSceneChanged: [conn:{0}]", conn.ToString()));

            if (SceneManager.GetSceneAt(0).name == lobbyScene)
            {
                if (_topPanel.IsInGame)
                {
                    ChangeTo(_lobbyPanel);

                    // handle matchmaking here

                    if (conn.playerControllers[0].unetView.isClient)
                    {
                        _backDelegate = StopHostClbk;
                    }
                    else
                    {
                        _backDelegate = StopClientClbk;
                    } 
                }
                else
                {
                    ChangeTo(_mainMenuPanel);
                }

                _topPanel.ToggleVisibility(true);
                _topPanel.IsInGame = false;
            }
            else
            {
                ChangeTo(null);

                Destroy(GameObject.Find("MainMenuUI(Clone)"));

                _topPanel.IsInGame = true;
                _topPanel.ToggleVisibility(false);
            }
        }

        public override void OnClientSceneChanged(NetworkConnection conn) {}

		public override void OnClientConnect(NetworkConnection conn)
		{
            base.OnClientConnect(conn);

            _infoPanel.gameObject.SetActive(false);

            conn.RegisterHandler(MsgKicked, KickedMessageHandler);

            if (!NetworkServer.active)
            {
                // Run on pure client (not self hosting client)

                ChangeTo(_lobbyPanel);
                _backDelegate = StopClientClbk;
                SetServerInfo("Client", networkAddress);

                //CustomNetworkMessage msg = new CustomNetworkMessage();
                //msg.ChosenCharacter = _chosenCharacter;
                //ClientScene.AddPlayer(conn, 0, msg);
            }
		}

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            ChangeTo(_mainMenuPanel);
            
            if (_infoPanel.isActiveAndEnabled)
            {
                _infoPanel.gameObject.SetActive(false);
                _countdownPanel.DisplayForSeconds("Could not connect to server", 3);
            }
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            ChangeTo(_mainMenuPanel);
            _infoPanel.Display("Client error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
        }

        public override void OnStopClient()
        {
            offlineScene = null;
        }

        #endregion

        #region Public Methods - UI Handler

        public void GoBackButton()
        {
            _backDelegate();
            _topPanel.IsInGame = false;
        }

        public void DisplayIsConnecting()
        {
            _infoPanel.Display("Connecting...", "Cancel", () => { _backDelegate(); });
        }

        public void SetServerInfo(string status, string host)
        {
            _statusInfo.text = status;
            _hostInfo.text = host;
        }

        public void ChangeTo(RectTransform newPanel)
        {
            if (_currentPanel != null)
            {
                _currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }

            _currentPanel = newPanel;

            if (_currentPanel == _mainMenuPanel)
            {
                SetServerInfo("Offline", "None");
            }
        }

        #endregion

        #region Public Methods - Server Side

        #region Management

        public void AddLocalPlayer()
        {
            TryToAddPlayer();
        }

        public void RemovePlayer(CustomLobbyPlayer player)
        {
            player.RemovePlayer();
        }

        public void SimpleBackClbk()
        {
            ChangeTo(_mainMenuPanel);
        }

        public void StopHostClbk()
        {
            StopHost();
            ChangeTo(_mainMenuPanel);
        }

        public void StopClientClbk()
        {
            StopClient();
            ChangeTo(_mainMenuPanel);
        }

        public void StopServerClbk()
        {
            StopServer();
            ChangeTo(_mainMenuPanel);
        }

        public void KickPlayer(NetworkConnection conn)
        {
            conn.Send(MsgKicked, new KickMsg());
        }

        public void KickedMessageHandler(NetworkMessage netMsg)
        {
            _infoPanel.Display("Kicked by Server", "Close", null);
            netMsg.conn.Disconnect();
        }

        #endregion

        public override void OnStartHost()
        {
            base.OnStartHost();

            ChangeTo(_lobbyPanel);
            _backDelegate = StopHostClbk;
            SetServerInfo("Hosting", networkAddress);
        }

        public override void OnStopHost()
        {
            base.OnStopHost();

            offlineScene = null;
        }

        public void OnPlayersNumberModified(int count)
        {
            // Increase the actual number of players inside the lobby
            _playerNumber += count;

            // Check here if there are more than one local players
            int localPlayerCount = 1;
        }

        //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        //{
        //    CustomNetworkMessage message = extraMessageReader.ReadMessage<CustomNetworkMessage>();
        //    Character chosenCharacter = message.ChosenCharacter;

        //    Debug.Log(string.Format("The player with id: [{0}] has chosen: [{1}]", playerControllerId, chosenCharacter));

        //    LoadPlayerPrefab(conn, playerControllerId, chosenCharacter);
        //}

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            if (!_currentPlayers.ContainsKey(conn.connectionId))
            {
                _currentPlayers.Add(conn.connectionId, 0);
            }

            GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

            CustomLobbyPlayer newPlayer = obj.GetComponent<CustomLobbyPlayer>();
            newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                CustomLobbyPlayer player = lobbySlots[i] as CustomLobbyPlayer;

                if (player != null)
                {
                    player.RpcUpdateRemoveButton();
                    player.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }

            return obj;
        }

        public void SetPlayerTypeLobby(NetworkConnection conn, int type)
        {
            if (_currentPlayers.ContainsKey(conn.connectionId))
            {
                _currentPlayers[conn.connectionId] = type;
            }
        }

        public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            int idx = _currentPlayers[conn.connectionId];

            GameObject playerPrefab = Instantiate(spawnPrefabs[idx], 
                                                  startPositions[conn.connectionId].position, 
                                                  Quaternion.identity);
            return playerPrefab;
        }

        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                CustomLobbyPlayer p = lobbySlots[i] as CustomLobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                CustomLobbyPlayer p = lobbySlots[i] as CustomLobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers >= minPlayers);
                }
            }

        }

        public override void OnLobbyServerPlayersReady()
        {
            bool allready = true;
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    allready &= lobbySlots[i].readyToBegin;
                }
            }

            if (allready)
            {
                StartCoroutine(ServerCountdownCoroutine());
            }
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //This hook allows you to apply state data from the lobby-player to the game-player
            //just subclass "LobbyHook" and add it to the lobby object.

            if (_lobbyHooks)
            {
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
            }

            return true;
        }

        #endregion
    }
}
