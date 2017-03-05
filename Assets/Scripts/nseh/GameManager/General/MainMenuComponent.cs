﻿using nseh.GameManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MainMenuComponent : MonoBehaviour
{
    public GameObject current;
    MenuManager _MenuManager;
    GameObject _paladin;
    GameObject _demon;
    GameObject _prospector;
    int adding;
    public Text _playerTurnText;
    public Button play;
    public Button paladin;
    public Button demon;
    public Button prospector;
    public EventSystem eventSystem;
    public GameObject selectedGameObject;

    void Start()
    {
        _MenuManager = GameManager.Instance.Find<MenuManager>();
        adding = 0;
        _playerTurnText.text = "PLAYER "+ (adding+1).ToString()+ " TURN!";
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
    {   if (current.name == "Canvas_PickingCharacters")
        {
            adding = 0;
            _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN!";
            paladin.interactable = true;
            demon.interactable = true;
            prospector.interactable = true;
            play.interactable = false;
            _MenuManager.RestartingCharacters();
        }
        current.SetActive(false);
        current = newCanvas;
        current.SetActive(true);
        

    }

    public void AddingPaladin()
    {
        _paladin = Resources.Load("Paladin") as GameObject;
        _MenuManager.Adding(_paladin);
        adding++;

        _MenuManager.SetPlayerChoice("Paladin", adding);

        if (adding == GameManager.Instance._numberPlayers)
        {
            _playerTurnText.text = "READY?";
            paladin.interactable=false;
            demon.interactable = false;
            prospector.interactable = false;
            play.interactable = true;
            eventSystem.SetSelectedGameObject(selectedGameObject);
            // 
        }
        else
        {
            _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN!";
        }
    }

    public void AddingDemon()
    {
        _demon = Resources.Load("Demon") as GameObject;
        _MenuManager.Adding(_demon);
        adding++;

        _MenuManager.SetPlayerChoice("Demon", adding);

        if (adding == GameManager.Instance._numberPlayers)
        {
            _playerTurnText.text = "READY?";
            paladin.interactable = false;
            demon.interactable = false;
            prospector.interactable = false;
            play.interactable = true;
            eventSystem.SetSelectedGameObject(selectedGameObject);

        }
        else
        {
            _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN!";
        }
    }

    public void AddingProspector()
    {
        _prospector = Resources.Load("SirProspector") as GameObject;
        _MenuManager.Adding(_prospector);
        adding++;

        _MenuManager.SetPlayerChoice("SirProspector", adding);

        if (adding == GameManager.Instance._numberPlayers)
        {
            _playerTurnText.text = "READY?";
            paladin.interactable = false;
            demon.interactable = false;
            prospector.interactable = false;
            play.interactable = true;
            eventSystem.SetSelectedGameObject(selectedGameObject);

        }
        else
        {
            _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN!";
        }
    }


    public void SaveChanges()
    {

    }

    public void Update()
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


