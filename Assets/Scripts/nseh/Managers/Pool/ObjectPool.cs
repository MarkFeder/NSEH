using System.Collections.Generic;
using UnityEngine;

namespace nseh.Managers.Pool
{
    public class ObjectPool
    {
        #region Private Properties

        private List<GameObject> _pooledObjects;
        private GameObject _pooledObj;

        private int _poolSize;

        #endregion

        /// <summary>
        /// ObjectPool Constructor
        /// </summary>
        /// <param name="obj">The object to be pooled</param>
        /// <param name="poolSize">The maximum size of the pool</param>
        public ObjectPool(GameObject obj, int poolSize)
        {
            // Instantiate a new list of pooled objects
            _pooledObjects = new List<GameObject>();

            // Size of the pool
            _poolSize = poolSize;

            // The type of object we want to make a pool from
            _pooledObj = obj;
        }

        #region Public Methods

        /// <summary>
        /// Init the pool. Call this function after allocating memory for ObjectPool
        /// </summary>
        public void InitObjectPool()
        {
            if (_pooledObj != null && _poolSize > 0)
            {
                for (int i = 0; i < _poolSize; i++)
                {
                    GameObject nObj = GameObject.Instantiate(_pooledObj, Vector3.zero, Quaternion.identity) as GameObject;

                    // Deactivate object
                    nObj.SetActive(false);

                    // Add object to the list
                    _pooledObjects.Add(nObj);

                    // Don't destroy on load, so we can manage it centrally
                    GameObject.DontDestroyOnLoad(nObj);
                }
            }
            else
            {
                Debug.LogError("InitObjectPool failed!");
            }
        }

        /// <summary>
        /// Get the next object in the pool.
        /// </summary>
        /// <returns></returns>
        public GameObject GetObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeSelf)
                {
                    _pooledObjects[i].SetActive(true);

                    return _pooledObjects[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Add deactivated object to the list again, so we do not have to allocate/deallocate memory.
        /// </summary>
        /// <param name="obj"></param>
        public void DestroyObject(GameObject obj)
        {
            obj.SetActive(false);
            _pooledObjects.Add(obj);
        }

        /// <summary>
        /// Clear pool and destroy all its objects
        /// </summary>
        public void ClearPool()
        {
            for (int i = _pooledObjects.Count - 1; i > 0; i--)
            {
                GameObject obj = _pooledObjects[i];
                GameObject.Destroy(obj);

                _pooledObjects.RemoveAt(i);
            }

            _pooledObjects.Clear();
        }

        #endregion
    }
}
