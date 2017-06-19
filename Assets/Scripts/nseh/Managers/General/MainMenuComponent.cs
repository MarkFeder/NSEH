﻿using nseh.Managers.Main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Managers.General
{
    public class MainMenuComponent : MonoBehaviour
    {

        #region Private Properties
        private MenuManager _MenuManager;
        private GameObject _wrarr;
        private GameObject _prospector;
        private int adding;
        #endregion

        #region Public Properties
        public GameObject current;
        public Text playerTurnText;
        public Button play;
        public Button wrarr;
        public Button prospector;
        public Button granhilda;
        public Button musho;
        public Button anthea;
        public Button harley;
        public Button myson;
        public Button random;
        public EventSystem eventSystem;
        public GameObject selectedGameObject;
        public AudioClip start;
        public AudioClip back;
        public AudioClip select;
        private AudioSource audiosource;
        private bool started;
        #endregion

        #region Public Methods
        public void Start()
        {
            started = false;
            _MenuManager = GameManager.Instance.Find<MenuManager>();
            adding = 0;
            audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.spatialBlend = 0;
            audiosource.volume = 0.5f;
            playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
        }

        public void Update()
        {
            if(Input.GetButtonDown(String.Format("{0}{1}", Inputs.JUMP,1)) && !started)
            {
                started = true;
                current.SetActive(true);
            }

        }

        

        public void OneNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(1);
            audiosource.clip = start;
            audiosource.Play();
            ChangeCanvas(newCanvas);
        }

        public void TwoNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(2);
            audiosource.clip = start;
            audiosource.Play();
            ChangeCanvas(newCanvas);
        }

        public void FourNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(4);
            audiosource.clip = start;
            audiosource.Play();
            ChangeCanvas(newCanvas);
        }

        public void Back(GameObject newCanvas)
        {
            audiosource.clip = back;
            audiosource.Play();
            ChangeCanvas(newCanvas);
        }

        public void ChangeCanvas(GameObject newCanvas)
        {
           

            if (current.name == "Canvas_PickingCharacters")
            {
                adding = 0;
                playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
                wrarr.interactable = true;
                prospector.interactable = true;
                granhilda.interactable = true;
                musho.interactable = true;
                anthea.interactable = true;
                harley.interactable = true;
                myson.interactable = true;
                random.interactable = true;
                play.interactable = false;
            }
            current.SetActive(false);
            current = newCanvas;
            current.SetActive(true);


        }

        public void AddingWrarr()
        {
            _wrarr = Resources.Load("Wrarr") as GameObject;
            _MenuManager.Adding(_wrarr);
            adding++;
            audiosource.clip = select;
            audiosource.Play();
            _MenuManager.SetPlayerChoice("Wrarr", adding);

            if (adding == GameManager.Instance._numberPlayers)
            {
                playerTurnText.text = "READY?";
                wrarr.interactable = false;
                prospector.interactable = false;
                granhilda.interactable = false;
                musho.interactable = false;
                anthea.interactable = false;
                harley.interactable = false;
                myson.interactable = false;
                random.interactable = false;
                play.interactable = true;
                eventSystem.SetSelectedGameObject(selectedGameObject);
       
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + 1;
                
            }
            else
            {
                playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
                
               eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_"+(adding+1).ToString();
               eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + (adding + 1).ToString();
               eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + (adding + 1).ToString();
               
            }
        }

      

        public void AddingProspector()
        {
            _prospector = Resources.Load("SirProspector") as GameObject;
            _MenuManager.Adding(_prospector);
            adding++;
            audiosource.clip = select;
            audiosource.Play();
            _MenuManager.SetPlayerChoice("SirProspector", adding);

            if (adding == GameManager.Instance._numberPlayers)
            {
                playerTurnText.text = "READY?";
                wrarr.interactable = false;
                prospector.interactable = false;
                granhilda.interactable = false;
                musho.interactable = false;
                anthea.interactable = false;
                harley.interactable = false;
                myson.interactable = false;
                random.interactable = false;
                play.interactable = true;
                eventSystem.SetSelectedGameObject(selectedGameObject);
       
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + 1;
                
            }
            else
            {
                playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
         
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + (adding + 1).ToString();
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + (adding + 1).ToString();
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + (adding + 1).ToString();
                
            }
        }


        public void SaveChanges()
        {

        }

        public void PlayGame(GameObject newCanvas)
        {
            //ChangeCanvas(newCanvas);
            audiosource.clip = start;
            audiosource.Play();
            ChangeCanvas(newCanvas);
            StartCoroutine(StartGame());
            
        }

        public void Exit()
        {
            audiosource.clip = back;
            audiosource.Play();
            _MenuManager.ExitGame();
        }


        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(1);
            _MenuManager.ChangeStates();
        }
            #endregion
        }
}


