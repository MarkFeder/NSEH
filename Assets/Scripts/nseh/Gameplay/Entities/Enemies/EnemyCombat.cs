using System;
using System.Collections.Generic;
using System.Linq;
using nseh.Gameplay.Combat.Attack.BavaDongo;
using nseh.Utils.Helpers;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Entities.Enemies
{
    public class EnemyCombat : MonoBehaviour
    {
		#region Private Properties
        
        [SerializeField]
		private List<Collider> _colliders;

		#endregion

		#region Private Methods

		#endregion

		#region Animation Events

		/// <summary>
		/// Activate the collider. This event is triggered by the animation.
		/// </summary>
		/// <param name="index">The weapon to be activated.</param>
		public void ActivateCollider(int index)
		{
            _colliders[index].enabled = true;
            
            CollisionHandler handler =_colliders[index].GetComponent<CollisionHandler>();
            Debug.Log(handler);
            handler.enabled = true;
            handler.ResetList();
        }

		/// <summary>
		/// Deactivate the collider. This event is triggered by the animation.
		/// </summary>
		/// <param name="index">The weapon to be deactivated.</param>
		public void DeactivateCollider(int index)
		{
            _colliders[index].enabled = false;
            CollisionHandler handler = _colliders[index].GetComponent<CollisionHandler>();
            handler.enabled = false;
        }


        public void ResetList(int index)
        {
            CollisionHandler handler = _colliders[index].GetComponent<CollisionHandler>();
            handler.ResetList();
        }

        #endregion
    }
}
