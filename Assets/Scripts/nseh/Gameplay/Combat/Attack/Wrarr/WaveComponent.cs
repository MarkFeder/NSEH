using System.Collections.Generic;
using nseh.Gameplay.Entities.Enemies;
using nseh.Gameplay.Entities.Player;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Attack.Wrarr
{
    public class WaveComponent : MonoBehaviour
	{
		#region Private Properties

        private CapsuleCollider _collider;
		private List<GameObject> _enemies;
		private PlayerInfo _senderInfo;

        [SerializeField]
        private float _stepSize;
		private float _damage;

		#endregion

		#region Public Properties

		public float Damage
		{
			get { return _damage; }
			set { _damage = value; }
		}

		public PlayerInfo Sender
		{
			get { return _senderInfo; }
			set { _senderInfo = value; }
		}

		public CapsuleCollider WaveCollider
		{
			get { return _collider; }
		}

        #endregion

        #region Private Methods

        private void Start()
        {
			_collider = GetComponent<CapsuleCollider>();
			_collider.enabled = false;
            _collider.height = 0.0f;

            _enemies = new List<GameObject>();
		}

		private void OnEnable()
		{
			if (_enemies != null && _collider != null)
			{
				_enemies.Clear();
				_collider.height = 0.0f;
			}
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
			GameObject enemyObj = other.gameObject;
			string colTag = other.tag;

			if (colTag == Tags.PLAYER_BODY)
			{
				if (!_enemies.Contains(enemyObj))
				{
					PlayerInfo enemyInfo = enemyObj.GetComponent<PlayerInfo>();
					if (enemyInfo != null && enemyInfo.Player != _senderInfo.Player)
					{
						// Set score and energy on sender
						_senderInfo.PlayerScore.IncreaseScore((int)_damage);
						_senderInfo.PlayerEnergy.IncreaseEnergy(_damage / 2);

						// Set health
						enemyInfo.PlayerHealth.TakeDamage((int)_damage);

						// Add this enemy to the list so as not to cause damage again
						_enemies.Add(enemyObj);
					}
				}
			}
			else if (colTag == Tags.ENEMY)
			{
				if (!_enemies.Contains(enemyObj))
				{
					EnemyHealth enemyHealth = enemyObj.GetComponent<EnemyHealth>();
					if (enemyHealth != null)
					{
						// Set health
						enemyHealth.TakeDamage((int)_damage);

						// Add this enemy to the list so as not to cause damage again
						_enemies.Add(enemyObj);
					}
				}
			}
        }

		#endregion
	}
}
