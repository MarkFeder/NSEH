using System;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Managers.Pool
{
    public class ObjectPoolManager : Service
    {
        #region Private Properties

        // look up list of various object pools
        private Dictionary<String, ObjectPool> _objectPools;

        #endregion

        #region Public C# Properties

        public Dictionary<String, ObjectPool> ObjectPools
        {
            get { return _objectPools; }
        }

        #endregion

        #region Service Methods

        public override void Activate()
        {
            IsActivated = true;

            _objectPools = new Dictionary<string, ObjectPool>();
        }

        public override void Tick()
        {
        }

        public override void Release()
        {
        }

        #endregion

        #region Public Methods

        public bool CreatePool(string poolName, GameObject objToPool, int poolSize)
        {
            if (_objectPools.ContainsKey(poolName))
            {
                Debug.Log(string.Format("Pool with name <{0}> exists!", poolName));

                // let the caller knows that it already exists; just use the pool out there
                return false;
            }
            else
            {
                // Create new pool and initialize it
                ObjectPool nPool = new ObjectPool(objToPool, poolSize);
                nPool.InitObjectPool();

                // After creating, insert it into the look up
                _objectPools.Add(poolName, nPool);

                return true;
            }
        }

        public ObjectPool GetPool(string poolName)
        {
            // Return the pool if it already exists

            if (_objectPools.ContainsKey(poolName))
            {
                return _objectPools[poolName];
            }
            else
            {
                Debug.Log(string.Format("Pool with name <{0}> does not exist!", poolName));
                return null;
            }
        }

        #endregion
    }
}
