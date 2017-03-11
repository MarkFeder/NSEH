using System.Collections;
using System.Collections.Generic;
using nseh.GameManager;
using nseh.Gameplay.Gameflow;
using nseh.Gameplay.Entities.Environment.Items;
using nseh.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Gameplay.Entities.Environment
{
    public class SpawnItemPoint : MonoBehaviour
    {

        // Use this for initialization
        #region Private Properties

        [SerializeField]
        private List<GameObject> _standardItems;
        [SerializeField]
        private List<GameObject> _specialItems;
        [SerializeField]
        private List<GameObject> _disadvantageItems;
        [SerializeField]
        private Text _spawnText;
        private GameObject _instancedItem;
        private ItemSpawn_Event _itemSpawnEvent;
        private float _elapsedTime;
        private float _internalCD;

        #endregion

        #region Public Properties

        [System.NonSerialized]
        public bool hasItem;

        #endregion


        void Start()
        {
            hasItem = false;
            _elapsedTime = 0;
            _internalCD = Constants.Events.ItemSpawn_Event.SPAWNPOINT_INTERNALCD;
            _spawnText.gameObject.SetActive(false);
            _itemSpawnEvent = GameManager.GameManager.Instance.Find<LevelManager>().Find<ItemSpawn_Event>();
            _itemSpawnEvent.RegisterSpawnItemPoint(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {

            if (hasItem == true && _instancedItem == null)
            {
                SetSpawnItemPointFree();
            }

        }

        #region Public Methods

        public void Spawn()
        {
            float dice = Random.value;

            if (dice <= 0.8)
            {
                //Standard buffs
                int randomStandardItem = (int)Random.Range(0, _standardItems.Count);
                _instancedItem = Instantiate(_standardItems[randomStandardItem], this.transform.position, this.transform.rotation);
            }

            if (0.8f < dice && dice <= 0.9f)
            {
                //Special buffs
                int randomSpecialItem = (int)Random.Range(0, _specialItems.Count);
                _instancedItem = Instantiate(_specialItems[randomSpecialItem], this.transform.position, this.transform.rotation);
            }

            if (0.9f < dice && dice <= 1)
            {
                //Debuffs
                int randomDisadvantageItem = (int)Random.Range(0, _disadvantageItems.Count);
                _instancedItem = Instantiate(_disadvantageItems[randomDisadvantageItem], this.transform.position, this.transform.rotation);
            }
            Debug.Log("Item spawned");
            StartCoroutine(DisplayText(_spawnText, Constants.Events.ItemSpawn_Event.SPAWN_ALERT, 2));
            hasItem = true;
        }

        
        public void flushItem()
        {
            if(_instancedItem != null)
            {
                Destroy(_instancedItem);
                hasItem = false;
            }
        }

        public void flushText()
        {
            if (_spawnText.gameObject.activeSelf)
            {
                _spawnText.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Private Methods

        private T GetRandomEnum<T>()
        {
            var states = System.Enum.GetValues(typeof(T));
            T chosenState = (T)states.GetValue(Random.Range(0, states.Length));
            return chosenState;
        }

        private IEnumerator DisplayText(Text text, string content, float time)
        {
            text.gameObject.SetActive(true);
            text.text = content;
            yield return new WaitForSeconds(time);
            text.gameObject.SetActive(false);
        }
        

        private void SetSpawnItemPointFree()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _internalCD)
            {
                Debug.Log("Spawn Point Free. Has item = " + hasItem);
                hasItem = false;
            }
        }

        #endregion
    }
}
