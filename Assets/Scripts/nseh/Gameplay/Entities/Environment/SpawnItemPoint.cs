using System.Collections;
using System.Collections.Generic;
using nseh.GameManager;
using UnityEngine;

public class SpawnItemPoint : MonoBehaviour {

    // Use this for initialization

    [SerializeField]
    private List<GameObject> StandardItems;
    [SerializeField]
    private List<GameObject> SpecialItems;
    [SerializeField]
    private List<GameObject> SpecialBuffs;
    private GameObject instancedItem;
    

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
        float dice = Random.value;
        if (dice <= 0.8)
        {
            //Standard buffs
            int randomStandardItem = (int)Random.Range(0, StandardItems.Count);
            instancedItem = Instantiate(StandardItems[randomStandardItem], this.transform.position, this.transform.rotation);
        }

        if(0.8f < dice && dice <= 0.9f)
        {
            //Special buffs
            int randomSpecialItem = (int)Random.Range(0, StandardItems.Count);
            instancedItem = Instantiate(StandardItems[randomSpecialItem], this.transform.position, this.transform.rotation);
        }

        if(0.9f < dice && dice <= 1)
        {
            //Debuffs
            int randomDebuffItem = (int)Random.Range(0, StandardItems.Count);
            instancedItem = Instantiate(StandardItems[randomDebuffItem], this.transform.position, this.transform.rotation);
        }
    }
}
