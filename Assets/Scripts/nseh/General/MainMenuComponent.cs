using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using nseh.Utils;
using nseh.GameManager;

public class MainMenuComponent : MonoBehaviour
{
    public GameObject current;
    MenuManager _MenuManager;

    void Start()
    {
        _MenuManager = GameManager.thisGame.Find<MenuManager>();
    }

    public void OneNumberCharacter(GameObject newCanvas)
    {
        _MenuManager.ChangePlayers(1);
        ChangeCanvas(newCanvas);
    }

    public void TwoNumberCharacter(GameObject newCanvas)
    {
        _MenuManager.ChangePlayers(2);
        ChangeCanvas(newCanvas);
    }

    public void ChangeCanvas(GameObject newCanvas)
    {
        current.SetActive(false);
        current = newCanvas;
        current.SetActive(true);

    }

    public void SaveChanges()
    {

    }

    public void PlayGame(GameObject newCanvas)
    {
    //ChangeCanvas(newCanvas);
        _MenuManager.ChangeStates();

    }
        
    public void Exit()
    {
        _MenuManager.ExitGame();
      
    }
}


