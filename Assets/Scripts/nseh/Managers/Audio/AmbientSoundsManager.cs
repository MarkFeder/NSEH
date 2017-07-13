using UnityEngine;
using System.Collections.Generic;
using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine.SceneManagement;

public class AmbientSoundsManager : MonoBehaviour {

    public List<AudioSource> list;

	// Use this for initialization
	void Start ()
    {
        GameEvent _gameEvent;
        MinigameEvent _minigameEvent;
        BossEvent _bossEvent;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Game":
                _gameEvent = GameManager.Instance.GameEvent;
                _gameEvent.AmbientSounds = list;
                break;

            case "Minigame":
                _minigameEvent = GameManager.Instance.MinigameEvent;
                _minigameEvent.AmbientSounds = list;
                break;

            case "Boss":
                _bossEvent = GameManager.Instance.BossEvent;
                _bossEvent.AmbientSounds = list;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
