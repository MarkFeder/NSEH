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
        float eventStart = Constants.Events.Tar_Event.EVENT_START;
        private float elapsedTime;
        public bool canSpawn;

        override public void Setup(LevelManager lvlManager)
        {
            base.Setup(lvlManager);
            _spawnItemPoints = new List<GameObject>();
        }

        public override void ActivateEvent()
        {
            IsActivated = true;
            canSpawn = true;
        }

        public override void EventRelease()
        {
            IsActivated = false;
        }

        public override void EventTick()
        {
            if (canSpawn)
            {
                ChooseSpawnPoint();
            }
        }

        public void RegisterSpawnItemPoint(GameObject lightToRegister)
        {
            _spawnItemPoints.Add(lightToRegister);
        }

        public void toggleSpawn()
        {
            canSpawn = !canSpawn;
        }

        void ChooseSpawnPoint()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= Constants.Events.Tar_Event.EVENT_START)
            {
                int randomSpawn = (int)Random.Range(0, _spawnItemPoints.Count);
                _spawnItemPoints[randomSpawn].GetComponent<SpawnItemPoint>().Spawn();
                elapsedTime = 0;
            }
        }
    }
}
