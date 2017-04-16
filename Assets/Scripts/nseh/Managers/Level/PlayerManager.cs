using nseh.Gameplay.Entities.Player;
using nseh.Managers.General;
using nseh.Gameplay.Gameflow;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public void Setup(GameObject prefab, Vector3 pos, Vector3 rot, int playerNumber, BarComponent playerBarComponent, List<Image> playerLives, LevelProgress lvlProgress)
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

            // Setup player lives
            _playerInfo.PlayerHealth.PlayerLives = playerLives;

            // Setup level progress
            _playerInfo.PlayerHealth.LvlProgress = lvlProgress;

            // Let the player moves
            _playerInfo.PlayerMovement.EnableMovement();

            // Activate collider
            _playerInfo.PlayerCollider.enabled = true;

            // Set player to idle state
            _playerInfo.Animator.Play(_playerInfo.IdleHash);
        }

        public void Reset()
        {
            _playerPrefab.transform.position = _spawnPosition;
            _playerPrefab.transform.rotation = Quaternion.Euler(_spawnRotation);

            _playerPrefab.SetActive(false);
            _playerPrefab.SetActive(true);

            _playerInfo.PlayerMovement.EnableMovement();
            _playerInfo.PlayerHealth.ResetHealth();
            _playerInfo.PlayerHealth.RestoreAllLives();
            _playerInfo.PlayerCollider.enabled = true;
            _playerInfo.Animator.Play(_playerInfo.IdleHash);
        }

        public void ResetFromDeath()
        {
            _playerPrefab.transform.position = _spawnPosition;
            _playerPrefab.transform.rotation = Quaternion.Euler(_spawnRotation);

            _playerPrefab.SetActive(false);
            _playerPrefab.SetActive(true);

            _playerInfo.PlayerMovement.EnableMovement();
            _playerInfo.PlayerHealth.ResetHealth();
            _playerInfo.PlayerCollider.enabled = true;
            _playerInfo.Animator.Play(_playerInfo.IdleHash);
        }

        #endregion
    }
}
