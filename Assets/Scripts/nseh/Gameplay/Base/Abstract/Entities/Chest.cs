using nseh.Gameplay.Entities.Environment;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;
using System;
using UnityEngine;
using UnityEngine.UI;
using Inputs = nseh.Utils.Constants.Input;
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
		protected PlayerInfo _particlesSpawnPoints;
		protected Collider _collider;
		protected Renderer _renderer;
		protected GameObject _sprite;
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
			if (other.CompareTag(Tags.PLAYER_BODY))
			{
				_sprite.SetActive(true);
			}
		}

		protected virtual void OnTriggerStay(Collider other)
		{
			if (other.CompareTag(Tags.PLAYER_BODY) && Input.GetButtonDown(String.Format("{0}{1}", Inputs.INTERACT, other.GetComponent<PlayerInfo>().GamepadIndex)))
			{
				SetVisibility(false);
				_sprite.SetActive(false);
				if (_currentUses < _uses)
				{
					_currentUses++;
					_target = other.gameObject;
					_particlesSpawnPoints = _target.GetComponent<PlayerInfo>();

					switch (_particlesSpawnPoints.Player)
					{
						case 1:
							_itemText = GameManager.Instance.LevelManager.CanvasItemsManager.P1ItemText;
							break;
						case 2:
							_itemText = GameManager.Instance.LevelManager.CanvasItemsManager.P2ItemText;
							break;
						case 3:
							_itemText = GameManager.Instance.LevelManager.CanvasItemsManager.P3ItemText;
							break;
						case 4:
							_itemText = GameManager.Instance.LevelManager.CanvasItemsManager.P4ItemText;
							break;
					}

					PlaySoundAtPlayer(_sound);
					Activate();
				}
				else
				{
					Debug.Log(String.Format("The number of uses is more than or equal to {0}", _uses));
				}
			}
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			if (other.CompareTag(Tags.PLAYER_BODY))
			{
				_sprite.SetActive(false);
			}
		}

		protected void ParticleAnimation(GameObject particle, float timeToDisplayParticles, Transform particlesPos)
		{
			GameObject particleGameObject = Instantiate(particle, particlesPos.position, particlesPos.rotation, _target.transform);
			particleGameObject.GetComponent<ParticleSystem>().Play();

			Destroy(particleGameObject, timeToDisplayParticles);
		}

		protected void PlaySoundAtPlayer(AudioClip clip)
		{
			AudioSource.PlayClipAtPoint(clip, _target.transform.position, 1);
		}

		#endregion

		#region Private Methods

		private void SetVisibility(bool isVisible)
		{
			_isVisible = isVisible;
			_renderer.enabled = isVisible;
			_collider.enabled = isVisible;
		}

		#endregion

		#region Virtual Methods

		protected virtual void Start()
		{
			_collider = GetComponent<Collider>();
			_renderer = GetComponent<Renderer>();
			_sprite = transform.GetChild(0).gameObject;

			ResetUses();
		}

		public virtual void SpawnAt(Vector3 position)
		{
			transform.position = position;
		}

		#endregion
	}
}
