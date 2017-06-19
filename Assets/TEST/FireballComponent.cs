using System.Collections.Generic;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Audio;

public class FireballComponent : MonoBehaviour {


    private List<GameObject> _enemies;
    [SerializeField]
    private int _damage;

    public List<AudioClip> fireballClip;

    private void Start()
    {
        _enemies = new List<GameObject>();
        SoundManager.Instance.PlayAudioFX(fireballClip[UnityEngine.Random.Range(0, fireballClip.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);

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
                enemyInfo.PlayerHealth.TakeDamage(_damage);
            }
        }
    }

}
