using nseh.Gameplay.Entities.Environment;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;
using System;
using UnityEngine;
using UnityEngine.UI;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Base.Abstract.Entities
{
	public abstract class Chest : MonoBehaviour
	{

		#region Private Properties

		[SerializeField]
		private AnimationClip _animation;
		[SerializeField]
		private AudioClip _sound;

		[SerializeField]
		private int _uses;
		[SerializeField]
		private string _displayName;

		#endregion

		#region Protected Properties

		[NonSerialized]
		protected GameObject _canvasItemText;
		[NonSerialized]
		protected SpawnItemPoint _spawnItemPoint;

		[SerializeField]
		protected GameObject _particlePrefab;
		[SerializeField]
		protected float _destructionTime;
		[SerializeField]
		protected float _timeToDisplayText;

		protected bool _isVisible = false;
		protected int _currentUses;

		protected GameObject _target;
		protected PlayerInfo _playerInfo;
		protected Collider _collider;
		protected Renderer[] _renderer;
		protected GameObject _sprite;
        protected GameObject _text;
        protected Text _itemText;

		#endregion

		#region Public Properties

		public GameObject CanvasItemText { get { return _canvasItemText; } set { _canvasItemText = value; } }

		public SpawnItemPoint SpawnItemPoint { get { return _spawnItemPoint; } set { _spawnItemPoint = value; } }

		#endregion

		#region Protected Methods

		protected void OnEnable()
		{
			ResetUses();
		}

		protected void ShowInScene()
		{
			SetVisibility(true);
		}

		protected void HideInScene()
		{
			SetVisibility(false);
		}

		protected void ResetUses()
		{
			_currentUses = 0;
		}

		protected abstract void Activate();
		protected abstract void Deactivate();

		protected virtual void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(Tags.PLAYER_BODY) && other.GetComponent<PlayerInfo>().items.Count < 2)
			{
				_sprite.SetActive(true);
			}
		}

		protected virtual void OnTriggerStay(Collider other)
		{
			if (other.CompareTag(Tags.PLAYER_BODY) && other.GetComponent<PlayerInfo>().InteractPressed && other.GetComponent<PlayerInfo>().items.Count<2)
			{

				SetVisibility(false);
				_sprite.SetActive(false);
                _text.SetActive(true);
				_target = other.gameObject;
				_playerInfo = _target.GetComponent<PlayerInfo>();
                GameManager.Instance.SoundManager.PlayAudioFX(_sound, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
                Activate();
	
			}
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			if (other.CompareTag(Tags.PLAYER_BODY))
			{
				_sprite.SetActive(false);
			}
		}

		protected GameObject ParticleAnimation(GameObject particle, float timeToDisplayParticles, Transform particlesPos)
		{

            /*foreach (Transform aux in particlesPos)
            {
                if(aux.name.Contains(particle.name))
                {
                    Destroy(aux.gameObject);
                    break;
                }
            }*/

			GameObject particleGameObject = Instantiate(particle, particlesPos.position, particlesPos.rotation, particlesPos.transform);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            return particleGameObject;

			//Destroy(particleGameObject, timeToDisplayParticles);
		}

		#endregion

		#region Private Methods

		private void SetVisibility(bool isVisible)
		{
			_isVisible = isVisible;
            foreach (Renderer aux in _renderer)
			    aux.enabled = isVisible;
			_collider.enabled = isVisible;
		}

		#endregion

		#region Virtual Methods

		protected virtual void Start()
		{
			_collider = GetComponent<Collider>();
			_renderer = transform.GetChild(2).GetComponentsInChildren<Renderer>();
            _sprite = transform.GetChild(0).gameObject;
            _text = transform.GetChild(1).gameObject;
            ResetUses();
		}


        public virtual void SpawnAt(Vector3 position)
		{
			transform.position = position;
		}

		#endregion
        
	}
}
