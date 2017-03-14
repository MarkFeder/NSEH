using nseh.Gameplay.Entities.Player;
using nseh.Gameplay.Entities.Environment;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Base.Abstract.Entities
{
	public abstract class Chest : MonoBehaviour
	{
		#region Public Properties

		public AnimationClip animation;
		public AudioClip sound;
		public GameObject particlePrefab;
        [NonSerialized]
        public GameObject canvasItemText;
        [NonSerialized]
        public SpawnItemPoint spawnItemPoint;

		public float destructionTime;
		public float timeToDisplayText;

		public int uses;
		public string displayName;

		#endregion

		#region Protected Properties

		protected bool isVisible = false;
		protected int currentUses;

		protected GameObject target;
		protected PlayerInfo particlesSpawnPoints;
		protected Collider collider;
		protected Renderer renderer;
		protected Text itemText;

		#endregion

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
			if (other.CompareTag(Tags.PLAYER_BODY))
			{
				if (this.currentUses < this.uses)
				{
					this.currentUses++;
					this.target = other.gameObject;
					this.particlesSpawnPoints = this.target.GetComponent<PlayerInfo>();
                    switch (this.particlesSpawnPoints.Player)
                    {
                        case 1:
                            itemText = canvasItemText.transform.GetChild(1).GetComponent<Text>();
                            break;
                        case 2:
                            itemText = canvasItemText.transform.GetChild(2).GetComponent<Text>();
                            break;
                    }

					this.PlaySoundAtPlayer(this.sound);
					this.Activate();

					// TODO: activate other properties
				}
				else
				{
					Debug.Log(String.Format("The number of uses is more than or equal to {0}", this.uses));
				}
			}
		}

		protected void ParticleAnimation(GameObject particle, float timeToDisplayParticles, Transform particlesPos)
		{
			GameObject particleGameObject = Instantiate(particle, particlesPos.position, particlesPos.rotation, this.target.transform);
			particleGameObject.GetComponent<ParticleSystem>().Play();

			Destroy(particleGameObject, timeToDisplayParticles);
		}

		protected void PlaySoundAtPlayer(AudioClip clip)
		{
			AudioSource.PlayClipAtPoint(clip, this.target.transform.position, 1);
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
