using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Combat.Weapon;

namespace nseh.Gameplay.Entities.Enemies
{
    public class EnemyCombat : MonoBehaviour
    {

		#region Private Properties
        
        [SerializeField]
		private List<Collider> _weapons;

		#endregion

		#region Animation Events

		public void ActivateCollider(int index)
		{
            _weapons[index].enabled = true;
            
            WeaponCollisionEnemy weapon =_weapons[index].GetComponent<WeaponCollisionEnemy>();
            weapon.enabled = true;
        }

		public void DeactivateCollider(int index)
		{
            _weapons[index].enabled = false;
            WeaponCollisionEnemy weapon = _weapons[index].GetComponent<WeaponCollisionEnemy>();
            weapon.enabled = false;
        }

        public void ResetList(int index)
        {
            WeaponCollisionEnemy weapon = _weapons[index].GetComponent<WeaponCollisionEnemy>();
            weapon.ResetList();
        }

        #endregion

    }
}
