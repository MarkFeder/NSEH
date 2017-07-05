using System.Collections.Generic;
using nseh.Gameplay.Entities.Player;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Special.Wrarr
{
    public class WaveComponent : MonoBehaviour
	{

		#region Private Properties

        private CapsuleCollider _collider;
        [SerializeField]
        private float _stepSize;
        private List<GameObject> _enemies;

		#endregion

        #region Private Methods

        private void Awake()
        {
			_collider = GetComponent<CapsuleCollider>();
			_collider.enabled = false;
            _collider.height = 0.0f;
            _enemies = new List<GameObject>();
		}

		private void OnEnable()
		{
            _collider.enabled = true;
            _enemies.Add(this.gameObject.transform.root.gameObject);
        }

        private void OnDisable()
        {
            _collider.enabled = false;
            _collider.height = 0.0f;
            _enemies.Clear();
        }

        private void Update()
        {
            if (_collider.enabled)
            {
                _collider.height += _stepSize;
                _collider.isTrigger = !_collider.isTrigger;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((other.transform.root.tag == Tags.PLAYER_BODY) && !_enemies.Contains(other.transform.root.gameObject))
            {
                PlayerMovement _auxPlayerMovement = other.transform.root.gameObject.GetComponent<PlayerMovement>();
                _enemies.Add(other.transform.root.gameObject);
                StartCoroutine(_auxPlayerMovement.PenalizationAgilityForSeconds(3,5));
            }
        }

        #endregion

    }
}
