using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomLobby
{
    public class CustomLobbyPlayerList : MonoBehaviour
    {
        #region Public Properties

        public static CustomLobbyPlayerList Instance
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

        public RectTransform PlayerListContentTransform
        {
            get
            {
                return _playerListContentTransform;
            }
        }

        #endregion

        private static CustomLobbyPlayerList _instance;

        [SerializeField]
        private RectTransform _playerListContentTransform;

        #region Protected Properties

        protected VerticalLayoutGroup _layout;
        protected List<CustomLobbyPlayer> _players;

        #endregion

        #region Public Methods

        public void OnEnable()
        {
            _instance = this;

            _players = new List<CustomLobbyPlayer>();
            _layout = _playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        }

        public void AddPlayer(CustomLobbyPlayer player)
        {
            if (_players.Contains(player))
            {
                Debug.LogWarning(string.Format("The player: {0} is already in this lobby", player));
                return;
            }

            _players.Add(player);

            player.transform.SetParent(_playerListContentTransform, false);
            PlayerListModified();
        }

        public void RemovePlayer(CustomLobbyPlayer player)
        {
            if (!_players.Contains(player))
            {
                Debug.LogWarning(string.Format("The player: {0} is not anymore in this lobby", player));
                return;
            }

            _players.Remove(player);
            PlayerListModified();
        }

        public void PlayerListModified()
        {
            int i = 0;
            foreach (CustomLobbyPlayer player in _players)
            {
                // See changes when updating player list
                player.OnPlayerListChanged(i);
                ++i;
            }
        }

        #endregion

        #region Private Methods

        private void Update()
        {
            // dirty layout sync problem

            if(_layout)
            {
                _layout.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
            }
        }

        #endregion
    }
}
