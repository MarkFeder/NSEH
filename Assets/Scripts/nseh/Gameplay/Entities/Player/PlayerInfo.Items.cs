using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        public IEnumerator AddItem(string name, float time, GameObject particle)
        {
            ItemsList auxItem = new ItemsList();
            auxItem.name = name;
            auxItem.time = Time.time + time;
            auxItem.particle = particle;

            foreach (ItemsList aux in items)
            {
                if (aux.name == name)
                {
                    Destroy(aux.particle);
                    items.Remove(aux);
                    break;
                }
            }

            items.Add(auxItem);

            yield return new WaitForSeconds(time);

            RemoveItem(name);
        }

        public void RemoveItem(string name)
        {
            foreach (ItemsList aux in items)
            {
                if (aux.name == name && Time.time >= aux.time)
                {
                    Destroy(aux.particle);
                    items.Remove(aux);
                    break;
                }
            }
        }

        public void ResetItemsList()
        {
            foreach (ItemsList aux in items)
            {
                Destroy(aux.particle);
            }

            items = new List<ItemsList>();

        }
    }
}
