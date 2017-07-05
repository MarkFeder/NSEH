using System.Collections.Generic;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;

namespace nseh.Gameplay.Boss
{
    public class FireballComponent : MonoBehaviour
    {

        #region Private Properties

        private List<GameObject> _enemies;
        [SerializeField]
        private int _damage;

        #endregion

        #region Public Properties

        public List<AudioClip> fireballClip;

        #endregion

        #region Private Methods

        private void Start()
        {
            _enemies = new List<GameObject>();
            GameManager.Instance.SoundManager.PlayAudioFX(fireballClip[UnityEngine.Random.Range(0, fireballClip.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);

        }

        private void OnTriggerEnter(Collider other)
        {
            GameObject enemy = other.gameObject;
            string colTag = other.tag;

            if (colTag == Tags.PLAYER && !_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
                PlayerInfo enemyInfo = enemy.GetComponent<PlayerInfo>();

                if (enemyInfo != null)
                {
                    enemyInfo.TakeDamage(_damage);
                }
            }
        }

        #endregion

    }
}
