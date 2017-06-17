using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;
using nseh.Gameplay.Entities.Player;

public class FireballComponent : MonoBehaviour {


    private List<GameObject> _enemies;
    [SerializeField]
    private int _damage;

    public List<AudioClip> fireballClip;

    private void Start()
    {
        _enemies = new List<GameObject>();
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = fireballClip[UnityEngine.Random.Range(0, fireballClip.Count)];
        audio.Play();

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
