using nseh.Gameplay.Entities.Player;
using nseh.Managers.General;
using System;
using UnityEngine;

namespace nseh.Managers.Level
{
    [Serializable]
    public class PlayerManager
    {
        #region Private Properties

        private GameObject _playerPrefab;
        private PlayerInfo _playerInfo;

        private int _playerNumber;

        private Vector3 _spawnPosition;
        private Vector3 _spawnRotation;

        #endregion

        #region Public C# Properties

        public GameObject PlayerRunTime
        {
            get { return _playerPrefab; }
        }

        public PlayerInfo PlayerRunTimeInfo
        {
            get { return _playerInfo; }
        }

        #endregion

        #region Public Methods

        public void Setup(GameObject prefab, Vector3 pos, Vector3 rot, int playerNumber, BarComponent playerBarComponent)
        {
            // Setup prefab
            _playerPrefab = GameObject.Instantiate(prefab, pos, Quaternion.Euler(rot));
            _playerNumber = playerNumber;
            _spawnPosition = pos;
            _spawnRotation = rot;

            // Get references to the components
            _playerInfo = _playerPrefab.GetComponent<PlayerInfo>();

            // Set player number to be consistent across the scripts
            _playerInfo.GamepadIndex = playerNumber;
            _playerInfo.Player = playerNumber;

            // Setup bar component
            _playerInfo.PlayerHealth.HealthBar = playerBarComponent;

            // Let the player moves
            _playerInfo.PlayerMovement.EnableMovement();

            // Activate collider
            _playerInfo.PlayerCollider.enabled = true;
        }

        public void Reset()
        {
            _playerPrefab.transform.position = _spawnPosition;
            _playerPrefab.transform.rotation = Quaternion.Euler(_spawnRotation);

            _playerPrefab.SetActive(false);
            _playerPrefab.SetActive(true);

            _playerInfo.PlayerMovement.EnableMovement();
            _playerInfo.PlayerHealth.ResetHealth();
            _playerInfo.PlayerCollider.enabled = true;
        }

        #endregion
    }
}
