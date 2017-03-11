using System.Collections;
using System.Collections.Generic;
using nseh.GameManager;
using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Environment;
using nseh.Utils;
using UnityEngine;

namespace nseh.Gameplay.Gameflow
{
    public class ItemSpawn_Event : LevelEvent
    {
        #region Private Properties

        private List<GameObject> _spawnItemPoints;
        private float _spawnPeriod = Constants.Events.ItemSpawn_Event.SPAWN_PERIOD;
        private float _elapsedTime;
        private SpawnItemPoint _lastSpawnItemPoint;

        #endregion

        override public void Setup(LevelManager lvlManager)
        {
            base.Setup(lvlManager);
            _spawnItemPoints = new List<GameObject>();
        }

        public override void ActivateEvent()
        {
            IsActivated = true;
            _elapsedTime = 0;

            if(_lastSpawnItemPoint != null)
            {
                _lastSpawnItemPoint.flushText();
                _lastSpawnItemPoint = null;
            }

            foreach(GameObject _spawnItemPoint in _spawnItemPoints){
                _spawnItemPoint.GetComponent<SpawnItemPoint>().flushItem(); 
            }
        }

        public override void EventRelease()
        {
            _spawnItemPoints = new List<GameObject>();
            IsActivated = false;
        }

        public override void EventTick()
        {
            ChooseSpawnPoint();
        }

        #region Public Methods

        public void RegisterSpawnItemPoint(GameObject spawnToRegister)
        {
            _spawnItemPoints.Add(spawnToRegister);
        }

        #endregion

        #region Private Methods

        private void ChooseSpawnPoint()
        {
            _elapsedTime += Time.deltaTime;
            List<GameObject> _freeSpawnItemPoints;
            if (_elapsedTime >= _spawnPeriod)
            {
                Debug.Log("Choosing spawn point and spawning");
                _freeSpawnItemPoints = _spawnItemPoints.FindAll(FindFreeSpawnPoint);
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
