using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverComponent : MonoBehaviour {

    LevelManager _LevelManager;

    public void RestartGame()
    {
        _LevelManager.Restart();
    }

    public void GoToMainMenu()
    {
        _LevelManager.GoToMainMenu();
    }

	// Use this for initialization
	void Start () {
        _LevelManager = GameManager.thisGame.Find<LevelManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
