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

		private List<Collider> _colliders;

		#endregion

		#region Private Methods

		private void Awake()
		{
			_colliders = gameObject.GetSafeComponentsInChildren<Collider>().Where(c => c.tag.Equals(Tags.WEAPON)).ToList();
		}

		#endregion

		#region Animation Events

		/// <summary>
		/// Activate the collider. This event is triggered by the animation.
		/// </summary>
		/// <param name="index">The weapon to be activated.</param>
		public void ActivateCollider(int index)
		{
			if (_colliders != null && _colliders.Count > 0)
			{
				// Deactivate other colliders
				for (int i = 0; i < _colliders.Count; i++)
				{
					Collider collider = _colliders[i];
					CollisionHandler weaponCollision = collider.GetComponent<CollisionHandler>();

					if (weaponCollision.Index != index)
					{
						collider.enabled = false;
						weaponCollision.enabled = false;
					}
					else
					{
						collider.enabled = true;
						weaponCollision.enabled = true;
					}
				}
			}
			else
			{
				Debug.Log(String.Format("ActivateCollider({0}): colliders are 0 or null", index));
			}
		}

		/// <summary>
		/// Deactivate the collider. This event is triggered by the animation.
		/// </summary>
		/// <param name="index">The weapon to be deactivated.</param>
		public void DeactivateCollider(int index)
		{
			if (_colliders != null && _colliders.Count > 0)
			{
				// Deactivate all the colliders
				for (int i = 0; i < _colliders.Count; i++)
				{
					Collider collider = _colliders[i];
					CollisionHandler weaponCollision = collider.GetComponent<CollisionHandler>();
					collider.enabled = false;
					weaponCollision.enabled = false;
				}
			}
			else
			{
				Debug.Log(String.Format("DeactivateCollider({0}): colliders are 0 or null", index));
			}
		}

		#endregion
	}
}
