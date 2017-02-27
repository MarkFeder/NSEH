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
    private bool instanced;
    private ItemSpawn_Event _itemSpawnEvent;
    

	void Start () {
        GameObject _spawnItemPoint = this.gameObject;
        _itemSpawnEvent = GameManager.Instance.Find<LevelManager>().Find<ItemSpawn_Event>();
        _itemSpawnEvent.RegisterSpawnItemPoint(_spawnItemPoint);
    }
	
	// Update is called once per frame
	void Update () {

		if(instanced = true && instancedItem == null)
        {
            instanced = false;
            _itemSpawnEvent.toggleSpawn();
        }

	}

    public void Spawn()
    {
        float dice = Random.value;

        if (dice <= 0.8)
        {
            //Standard buffs
            int randomStandardItem = (int)Random.Range(0, StandardItems.Count);
            instancedItem = Instantiate(StandardItems[randomStandardItem], this.transform.position, this.transform.rotation);
        }

        if (0.8f < dice && dice <= 0.9f)
        {
            //Special buffs
            int randomSpecialItem = (int)Random.Range(0, StandardItems.Count);
            instancedItem = Instantiate(StandardItems[randomSpecialItem], this.transform.position, this.transform.rotation);
        }

        if (0.9f < dice && dice <= 1)
        {
            //Debuffs
            int randomDebuffItem = (int)Random.Range(0, StandardItems.Count);
            instancedItem = Instantiate(StandardItems[randomDebuffItem], this.transform.position, this.transform.rotation);
        }

        _itemSpawnEvent.toggleSpawn();
        instanced = true;
    }
}
