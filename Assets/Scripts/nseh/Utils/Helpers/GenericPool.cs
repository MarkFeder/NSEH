using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nseh.Utils.Helpers
{
    public class GenericPool : MonoBehaviour
    {
        #region Private Properties

        private List<GameObject> pool; 

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor to initialize the pool
        /// </summary>
        /// <param name="size">the size of the pool</param>
        /// <param name="prefab">the prefab that this pool stores</param>
        public GenericPool(int size, GameObject prefab)
        {
            this.pool = new List<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = (GameObject)Instantiate(prefab);
                obj.SetActive(false);

                this.pool.Add(obj);
            }
        }

        /// <summary>
        /// Get the next object in the pool. User should call SetActive 
        /// after retrieving object
        /// </summary>
        /// <returns></returns>
        public GameObject GetObject()
        {
            if (this.pool.Count() > 0)
            {
                GameObject obj = this.pool[0];
                this.pool.RemoveAt(0);
                return obj;
            }

            return null;
        }

        /// <summary>
        /// Add object to the list again, so we do not have to allocate/deallocate memory
        /// </summary>
        /// <param name="obj"></param>
        public void DestroyObjectPool(GameObject obj)
        {
            this.pool.Add(obj);
            obj.SetActive(false);
        }

        /// <summary>
        /// Clear pool and destroy all its objects
        /// </summary>
        public void ClearPool()
        {
            for (int i = this.pool.Count - 1; i > 0; i--)
            {
                GameObject obj = this.pool[i];
                Destroy(obj);

                this.pool.RemoveAt(i);
            }

            this.pool = null;
        } 

        #endregion
    }
}
