using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.GameManager;

public class PausedComponent : MonoBehaviour {

    LevelManager _LevelManager;

    public void RestartGame()
    {
        _LevelManager.Restart();
    }

    public void GoToMainMenu()
    {
        _LevelManager.GoToMainMenu();
    }

    public void Resume()
    {
        _LevelManager.PauseGame();
    }

    // Use this for initialization
    void Start()
    {
        _LevelManager = GameManager.Instance.Find<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}