using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Main;
using nseh.Managers.Level;

namespace nseh.Gameplay.Entities.Environment
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        void Start()
        {
            GameManager.Instance.Find<LevelManager>().RegisterPlayerSpawnPoint(this.gameObject);
        }
    }
}