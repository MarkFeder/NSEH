using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Managers.Main;
using nseh.Managers.Level;

namespace nseh.Gameplay.Entities.Environment
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        void Start()
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                Debug.Log("Registro SpawnPoint");
                Debug.Log(GameManager.Instance.Find<LevelManager>());
                GameManager.Instance.Find<LevelManager>().RegisterPlayerSpawnPoint(this.gameObject);
            }
        }
    }
}