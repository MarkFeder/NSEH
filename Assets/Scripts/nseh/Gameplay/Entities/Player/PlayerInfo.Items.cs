using UnityEngine;
using System.Collections;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        public IEnumerator AddItem(string name, float time)
        {
            ItemsList auxItem = new ItemsList();
            auxItem.name = name;
            auxItem.time = Time.time + time;

            foreach (ItemsList aux in items)
            {
                if (aux.name == name)
                {
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
            foreach(ItemsList aux in items)
            {
                if(aux.name == name && Time.time >= aux.time)
                {
                    items.Remove(aux);
                    break;
                }
            }
        }
    }
}
