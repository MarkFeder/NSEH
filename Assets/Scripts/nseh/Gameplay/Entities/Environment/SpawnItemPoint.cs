using System.Collections;
using System.Collections.Generic;
using nseh.GameManager;
using nseh.Gameplay.Gameflow;
using nseh.Gameplay.Entities.Environment.Items;
using UnityEngine;

namespace nseh.Gameplay.Entities.Environment
{
    public class SpawnItemPoint : MonoBehaviour
    {

        // Use this for initialization
        
        [SerializeField]
        private List<GameObject> StandardItems;
        [SerializeField]
        private List<GameObject> SpecialItems;
        [SerializeField]
        private List<GameObject> DisadvantageItems;
        
        /*
        [SerializeField]
        private GameObject standardChest;
        [SerializeField]
        private GameObject specialChest;
        [SerializeField]
        private GameObject disadvantageChest;
        */
        private GameObject instancedItem;
        private bool instanced;
        private ItemSpawn_Event _itemSpawnEvent;


        void Start()
        {
            instanced = false;
            GameObject _spawnItemPoint = this.gameObject;
            _itemSpawnEvent = GameManager.GameManager.Instance.Find<LevelManager>().Find<ItemSpawn_Event>();
            _itemSpawnEvent.RegisterSpawnItemPoint(_spawnItemPoint);
        }

        // Update is called once per frame
        void Update()
        {

            if (instanced == true && instancedItem == null)
            {
                Debug.Log("Item catched, another one can be spawned now. Instanced = " + instanced);
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
                //standardChest.GetComponent<StandardChest>().chestType = GetRandomEnum<StandardChestType>();
                //instancedItem = Instantiate(standardChest, this.transform.position, this.transform.rotation);
                int randomStandardItem = (int)Random.Range(0, StandardItems.Count);
                instancedItem = Instantiate(StandardItems[randomStandardItem], this.transform.position, this.transform.rotation);
            }

            if (0.8f < dice && dice <= 0.9f)
            {
                //Special buffs
                //specialChest.GetComponent<SpecialChest>().chestType = GetRandomEnum<SpecialChestType>();
                //instancedItem = Instantiate(specialChest, this.transform.position, this.transform.rotation);
                int randomSpecialItem = (int)Random.Range(0, SpecialItems.Count);
                instancedItem = Instantiate(SpecialItems[randomSpecialItem], this.transform.position, this.transform.rotation);
            }

            if (0.9f < dice && dice <= 1)
            {
                //Debuffs
                //disadvantageChest.GetComponent<DisadvantageChest>().chestType = GetRandomEnum<DisadvantageChestType>();
                //instancedItem = Instantiate(disadvantageChest, this.transform.position, this.transform.rotation);
                int randomDisadvantageItem = (int)Random.Range(0, DisadvantageItems.Count);
                instancedItem = Instantiate(DisadvantageItems[randomDisadvantageItem], this.transform.position, this.transform.rotation);
            }
            Debug.Log("Item spawned");
            _itemSpawnEvent.toggleSpawn();
            instanced = true;
        }

        public void flushItem()
        {
            if(instancedItem != null)
            {
                Destroy(instancedItem);
                instanced = false;
            }
        }

        private T GetRandomEnum<T>()
        {
            var states = System.Enum.GetValues(typeof(T));
            T chosenState = (T)states.GetValue(Random.Range(0, states.Length));
            return chosenState;
        }
    }
}
