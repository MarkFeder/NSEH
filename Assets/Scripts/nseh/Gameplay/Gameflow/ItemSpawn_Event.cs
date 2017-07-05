using nseh.Gameplay.Entities.Environment;
using nseh.Managers.Main;
using nseh.Utils;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Base.Interfaces;

namespace nseh.Gameplay.Gameflow
{
    public class ItemSpawn_Event : MonoBehaviour, IEvent
    {

        #region Public Properties

        public List<GameObject> spawnItemPoints;

        #endregion

        #region Private Properties

        private float _spawnPeriod = Constants.Events.ItemSpawn_Event.SPAWN_PERIOD;
        private float _elapsedTime;
        private SpawnItemPoint _lastSpawnItemPoint;
        private bool _isActivated;

        #endregion

        #region Public Methods

        public void ActivateEvent()
        {
            _isActivated = true;
            _elapsedTime = 0;

            if (_lastSpawnItemPoint != null)
            {
                _lastSpawnItemPoint.flushText();
                _lastSpawnItemPoint = null;
            }

            foreach (GameObject _spawnItemPoint in spawnItemPoints)
            {
                _spawnItemPoint.GetComponent<SpawnItemPoint>().flushItem();
            }

        }

        public void EventRelease()
        {
            foreach (GameObject _spawnItemPoint in spawnItemPoints)
            {
                _spawnItemPoint.GetComponent<SpawnItemPoint>().flushItem();
            }

            _isActivated = false;
        }

        public void Update()
        {
            if (_isActivated && !GameManager.Instance.isPaused)
                ChooseSpawnPoint();

        }

        public void RegisterSpawnItemPoint(GameObject spawnToRegister)
        {
            spawnItemPoints.Add(spawnToRegister);
        }

        #endregion

        #region Private Methods

        private void ChooseSpawnPoint()
        {
            _elapsedTime += Time.deltaTime;
            List<GameObject> _freeSpawnItemPoints;
            if (_elapsedTime >= _spawnPeriod)
            {
                _freeSpawnItemPoints = spawnItemPoints.FindAll(FindFreeSpawnPoint);

                if (_freeSpawnItemPoints.Count != 0)
                {
                    int randomSpawn = (int)Random.Range(0, _freeSpawnItemPoints.Count);
                    _lastSpawnItemPoint = _freeSpawnItemPoints[randomSpawn].GetComponent<SpawnItemPoint>();
                    _lastSpawnItemPoint.Spawn();
                }

                _elapsedTime = 0;
            }
        }

        private bool FindFreeSpawnPoint(GameObject spawnPoint)
        {
            return !spawnPoint.GetComponent<SpawnItemPoint>().hasItem;
        }

        #endregion

    }
}
