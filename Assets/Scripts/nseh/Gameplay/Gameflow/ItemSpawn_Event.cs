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
        List<GameObject> _spawnItemPoints;
        float eventStart = 3.0f;
        private float elapsedTime;
        private SpawnItemPoint lastSpawnItemPoint;
        private bool canSpawn;

        override public void Setup(LevelManager lvlManager)
        {
            base.Setup(lvlManager);
            _spawnItemPoints = new List<GameObject>();
        }

        public override void ActivateEvent()
        {
            IsActivated = true;
            elapsedTime = 0;
            canSpawn = true;
        }

        public override void EventRelease()
        {
            lastSpawnItemPoint.flushItem();
            IsActivated = false;
        }

        public override void EventTick()
        {
            if (canSpawn)
            {
                ChooseSpawnPoint();
            }
        }

        public void RegisterSpawnItemPoint(GameObject spawnToRegister)
        {
            _spawnItemPoints.Add(spawnToRegister);
        }

        public void toggleSpawn()
        {
            canSpawn = !canSpawn;
        }

        void ChooseSpawnPoint()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= eventStart)
            {
                Debug.Log("Spawn now");
                int randomSpawn = (int)Random.Range(0, _spawnItemPoints.Count);
                _spawnItemPoints[randomSpawn].GetComponent<SpawnItemPoint>().Spawn();
                lastSpawnItemPoint = _spawnItemPoints[randomSpawn].GetComponent<SpawnItemPoint>();
                elapsedTime = 0;
            }
        }
    }
}
