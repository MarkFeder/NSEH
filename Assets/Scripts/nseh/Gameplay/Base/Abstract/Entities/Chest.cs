using System;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Base.Abstract.Entities
{
    public abstract class Chest : MonoBehaviour
	{
		public AnimationClip animation;
		public AudioClip sound;
		public ParticleSystem particle;

		protected GameObject target;
		protected Collider collider;
		protected Renderer renderer;

		public float lifeTime;
		public float destructionTime;
		public int uses;
		public string displayName;

		protected bool isVisible = false;
		protected int currentUses;

		#region Protected Methods

		protected virtual void Start()
		{
			this.collider = this.GetComponent<Collider>();
			this.renderer = this.GetComponent<Renderer>();

			this.ResetUses();
		}

		protected void OnEnable()
		{
			this.ResetUses();
		}

		protected void ShowInScene()
		{
			this.SetVisibility(true);
		}

		protected void HideInScene()
		{
			this.SetVisibility(false);
		}

		protected void ResetUses()
		{
			this.currentUses = 0;
		}

		protected abstract void Activate();
		protected abstract void Deactivate();

		protected virtual void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(Tags.PLAYER))
			{
				if (this.currentUses <= this.uses)
				{
					this.currentUses++;
					this.target = other.gameObject;
					this.Activate();

					// TODO: activate other properties
					Destroy(this.gameObject, this.destructionTime);
				}
				else
				{
					Debug.Log(String.Format("The number of uses is more than or equal to {0}", this.uses));
				}
			}
		}

		#endregion

		#region Private Methods

		private void SetVisibility(bool isVisible)
		{
			this.isVisible = isVisible;
			this.renderer.enabled = this.isVisible;
			this.collider.enabled = this.isVisible;
		}

		#endregion

		#region Public Methods

		public virtual void SpawnAt(Vector3 position)
		{
			this.transform.position = position;
		}

		#endregion
	}
}
