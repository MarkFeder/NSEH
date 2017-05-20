using nseh.Gameplay.Entities.Player;
using nseh.Managers.General;
using nseh.Gameplay.Gameflow;
using nseh.Gameplay.Entities.Environment;
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
        private List<GameObject> _spawnPoints;

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

        public void Setup(GameObject prefab, Vector3 pos, Vector3 rot, List<GameObject> spawnPoints, int playerNumber, BarComponent playerHealthBarComponent, BarComponent playerEnergyBarComponent, List<Image> playerLives/*, LevelProgress lvlProgress*/)
        {
            // Setup prefab
            _playerPrefab = GameObject.Instantiate(prefab, pos, Quaternion.Euler(rot));
            _playerNumber = playerNumber;
            _spawnPosition = pos;
            _spawnRotation = rot;
            _spawnPoints = spawnPoints;

            // Get references to the components
            _playerInfo = _playerPrefab.GetComponent<PlayerInfo>();

            // Set player number to be consistent across the scripts
            _playerInfo.GamepadIndex = playerNumber;
            _playerInfo.Player = playerNumber;
            // Setup bar components
            _playerInfo.PlayerHealth.HealthBar = playerHealthBarComponent;
            _playerInfo.PlayerEnergy.EnergyBar = playerEnergyBarComponent;

            // Setup player lives
            _playerInfo.PlayerHealth.PlayerLives = playerLives;

            // Let the player moves
            _playerInfo.PlayerMovement.EnableMovement();

            // Activate collider
            _playerInfo.PlayerCollider.enabled = true;

            // Set player to idle state
            _playerInfo.Animator.Play(_playerInfo.IdleHash);
        }

        public void Setup(GameObject prefab, Vector3 pos, Vector3 rot, List<GameObject> spawnPoints, int playerNumber, BarComponent playerHealthBarComponent, BarComponent playerEnergyBarComponent, List<Image> playerLives, String Tag/*, LevelProgress lvlProgress*/)
        {
            // Setup prefab
            _playerPrefab = GameObject.Instantiate(prefab, pos, Quaternion.Euler(rot));
            _playerNumber = playerNumber;
            _spawnPosition = pos;
            _spawnRotation = rot;
            _spawnPoints = spawnPoints;

            // Get references to the components
            _playerInfo = _playerPrefab.GetComponent<PlayerInfo>();

            // Set player number to be consistent across the scripts
            _playerInfo.GamepadIndex = playerNumber;
            _playerInfo.Player = playerNumber;
            // Setup bar components
            _playerInfo.PlayerHealth.HealthBar = playerHealthBarComponent;
            _playerInfo.PlayerEnergy.EnergyBar = playerEnergyBarComponent;

            // Setup player lives
            _playerInfo.PlayerHealth.PlayerLives = playerLives;

            // Let the player moves
            _playerInfo.PlayerMovement.EnableMovement();

            // Activate collider
            _playerInfo.PlayerCollider.enabled = true;

            // Set player to idle state
            _playerInfo.Animator.Play(_playerInfo.IdleHash);

            _playerPrefab.tag = Tag;
        }

        public void Reset()
        {
            _playerPrefab.transform.position = _spawnPosition;
            _playerPrefab.transform.rotation = Quaternion.Euler(_spawnRotation);

            _playerPrefab.SetActive(false);
            _playerPrefab.SetActive(true);

            _playerInfo.PlayerMovement.EnableMovement();
            _playerInfo.PlayerHealth.ResetHealth();
            _playerInfo.PlayerHealth.ResetDeathCounter();
            _playerInfo.PlayerEnergy.ResetEnergy();
            //_playerInfo.PlayerHealth.RestoreAllLives();
            _playerInfo.PlayerCollider.enabled = true;
            _playerInfo.Animator.Play(_playerInfo.IdleHash);
        }

        public void ResetFromDeath()
        {
            List<GameObject> freePlayerSpawnPoints = new List<GameObject>();
            freePlayerSpawnPoints = _spawnPoints.FindAll(FindFreePlayerSpawnPoint);
            Debug.Log("Number of Spawn Points" + _spawnPoints.Count);
            if (freePlayerSpawnPoints.Count != 0)
            {
                int randomSpawn = (int)UnityEngine.Random.Range(0, freePlayerSpawnPoints.Count);

                
                _playerPrefab.transform.position = freePlayerSpawnPoints[randomSpawn].transform.position;
                freePlayerSpawnPoints[randomSpawn].GetComponent<PlayerSpawnPoint>().ParticleAnimation(_playerPrefab.transform);
            }
            else
            {
                Debug.Log("There are problems with player's respawn");
            }

            _playerPrefab.transform.rotation = Quaternion.Euler(_spawnRotation);

            _playerPrefab.SetActive(false);
            _playerPrefab.SetActive(true);

            _playerInfo.PlayerMovement.EnableMovement();
            _playerInfo.PlayerHealth.ResetHealth();
            _playerInfo.PlayerCollider.enabled = true;
            _playerInfo.Animator.Play(_playerInfo.IdleHash);
        }

        private bool FindFreePlayerSpawnPoint(GameObject playerSpawnPoint)
        {
            return playerSpawnPoint.GetComponent<PlayerSpawnPoint>().IsFree;
        }

        #endregion
    }
}
