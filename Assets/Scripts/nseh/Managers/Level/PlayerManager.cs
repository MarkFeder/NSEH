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

        private GameObject playerPrefab;

        private int playerNumber;

        private Vector3 spawnPosition;
        private Vector3 spawnRotation;

        private PlayerInfo playerInfo;

        #endregion

        #region Public C# Properties

        public GameObject PlayerRunTime
        {
            get { return playerPrefab; }
        }

        public PlayerInfo PlayerRunTimeInfo
        {
            get { return playerInfo; }
        }

        #endregion

        #region Public Methods

        public void Setup(GameObject prefab, Vector3 pos, Vector3 rot, int playerNumber, BarComponent playerBarComponent)
        {
            // Setup prefab
            playerPrefab = GameObject.Instantiate(prefab, pos, Quaternion.Euler(rot));
            this.playerNumber = playerNumber;
            spawnPosition = pos;
            spawnRotation = rot;

            // Get references to the components
            playerInfo = playerPrefab.GetComponent<PlayerInfo>();

            // Set player number to be consistent across the scripts
            playerInfo.GamepadIndex = playerNumber;
            playerInfo.Player = playerNumber;

            // Setup bar component
            playerInfo.PlayerHealth.HealthBar = playerBarComponent;

            // Let the player moves
            EnableMovement();
        }

        public void EnableMovement()
        {
            playerInfo.PlayerMovement.enabled = true;
            playerInfo.Body.isKinematic = false;
        }

        public void DisableMovement()
        {
            playerInfo.PlayerMovement.enabled = false;
            playerInfo.Body.isKinematic = true;
        }

        public void Reset()
        {
            playerPrefab.transform.position = spawnPosition;
            playerPrefab.transform.rotation = Quaternion.Euler(spawnRotation);

            playerPrefab.SetActive(false);
            playerPrefab.SetActive(true);

            EnableMovement();
        }

        #endregion
    }
}
