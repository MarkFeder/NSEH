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

		public float destructionTime;
		public int uses;
		public string displayName;

		#endregion

		#region Protected Properties

		protected bool isVisible = false;
		protected int currentUses;

		protected GameObject target;
		protected Collider collider;
		protected Renderer renderer;
        protected Text itemText;

		#endregion

		#region Protected Methods

		protected virtual void Start()
		{
			this.collider = this.GetComponent<Collider>();
			this.renderer = this.GetComponent<Renderer>();
            this.itemText = GameObject.Find("CanvasItems/Text").GetComponent<Text>();
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
				if (this.currentUses < this.uses)
				{
					this.currentUses++;
					this.target = other.gameObject;
                    PlaySoundAtPlayer(sound); //PLACEHOLDER SOUND
					this.Activate();

					// TODO: activate other properties
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

        public void PlaySoundAtPlayer(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, target.transform.position, 1);
        }

        public IEnumerator DisplayText(Text text, string content, float time)
        {
            text.gameObject.SetActive(true);
            text.text = content;
            yield return new WaitForSeconds(time);
            text.gameObject.SetActive(false);
        }
        /*
        public IEnumerator ParticleAnimation(GameObject particle, float time)
        {
            GameObject particleGameObject = Instantiate(particle, target.transform.position, target.transform.rotation, target.transform);
            particleGameObject.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(time);
            Debug.Log("Particles must be destroyed now");
            particleGameObject.GetComponent<ParticleSystem>().Stop();
            particleGameObject.transform.parent = null;
            Destroy(particleGameObject);
        }*/

        public void ParticleAnimation(GameObject particle, float time)
        {
            GameObject particleGameObject = Instantiate(particle, target.transform.position, target.transform.rotation, target.transform);
            particleGameObject.GetComponent<ParticleSystem>().Play();
            Destroy(particleGameObject, time);
        }

        //TODO: Maybe it will be worth to do a overload on ParticleAnimation for items based on "times".
        #endregion
    }
}
