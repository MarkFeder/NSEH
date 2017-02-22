using System.Collections;
using System.Collections.Generic;
using nseh.GameManager;
using UnityEngine;

public class SpawnItemPoint : MonoBehaviour {

    // Use this for initialization

    public List<GameObject> Items;

	void Start () {
        GameObject _spawnItemPoint = this.gameObject;
        GameManager.Instance.Find<LevelManager>().Find<ItemSpawn_Event>().RegisterSpawnItemPoint(_spawnItemPoint);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool Spawn()
    {
        return false;
    }

    void Choose()
    {
        if(Random.value <= 0.8)
        {
            //Standard buffs
        }

        if(Random.value <= 0.1)
        {
            //Special buffs
        }

        if(Random.value <= 0.1)
        {
            //Debuffs
        }
    }
}
