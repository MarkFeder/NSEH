using nseh.Managers.Main;
using UnityEngine;
using System.Collections.Generic;
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
        private int adding;
        private bool started;

        #endregion

        #region Public Properties

        public GameObject current;
        public Text playerTurnText;
        public MyButton play;
        public MyButton wrarr;
        public MyButton prospector;
        public MyButton granhilda;
        public MyButton musho;
        public MyButton anthea;
        public MyButton harley;
        public MyButton myson;
        public MyButton random;
        public List<MyEventSystem> eventSystem;
        public GameObject selectedGameObject;
        public AudioClip start;
        public AudioClip back;
        public AudioClip select;

        #endregion

        #region Public Methods

        public void Start()
        {
            started = false;
            _MenuManager = GameManager.Instance.Find<MenuManager>();
            adding = 0;
            playerTurnText.text = "UNEMPLOYMENT OFFICE";
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
            GameManager.Instance.SoundManager.PlayAudioFX(start, 0.5f, false, Vector3.zero, 0);
            ChangeCanvas(newCanvas);
        }

        public void TwoNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(2);
            GameManager.Instance.SoundManager.PlayAudioFX(start, 0.5f, false, Vector3.zero, 0);
            ChangeCanvas(newCanvas);
        }

        public void FourNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(4);
            GameManager.Instance.SoundManager.PlayAudioFX(start, 0.5f, false, Vector3.zero, 0);
            ChangeCanvas(newCanvas);
        }

        public void Back(GameObject newCanvas)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(back, 0.5f, false, Vector3.zero, 0);
            ChangeCanvas(newCanvas);
        }

        public void ChangeCanvas(GameObject newCanvas)
        {
            if (current.name == "Canvas_PickingCharacters")
            {
                adding = 0;
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
            if (GameManager.Instance._characters[wrarr.player] == "")
                adding++;
            _MenuManager.Adding("Wrarr", wrarr.player);
            GameManager.Instance.SoundManager.PlayAudioFX(select, 0.5f, false, Vector3.zero, 0);


            if (adding == GameManager.Instance._numberPlayers)
            {
                playerTurnText.text = "READY?";
                play.interactable = true;
                eventSystem[0].SetSelectedGameObject(selectedGameObject);
            }
            /*
                wrarr.interactable = false;
                prospector.interactable = false;
                granhilda.interactable = false;
                musho.interactable = false;
                anthea.interactable = false;
                harley.interactable = false;
                myson.interactable = false;
                random.interactable = false;*/
                
       /*
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + 1;
                */
                
            /*
            else
            {
                playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
             /*   
               eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_"+(adding+1).ToString();
               eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + (adding + 1).ToString();
               eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + (adding + 1).ToString();
               
            }*/
        }

      

        public void AddingProspector()
        {
            if (GameManager.Instance._characters[prospector.player] == "")
                adding++;
            _MenuManager.Adding("SirProspector", prospector.player);
            
            GameManager.Instance.SoundManager.PlayAudioFX(select, 0.5f, false, Vector3.zero, 0);

            if (adding == GameManager.Instance._numberPlayers)
            {
                playerTurnText.text = "READY?";
                play.interactable = true;
                eventSystem[0].SetSelectedGameObject(selectedGameObject);
            }
            /*
                wrarr.interactable = false;
                prospector.interactable = false;
                granhilda.interactable = false;
                musho.interactable = false;
                anthea.interactable = false;
                harley.interactable = false;
                myson.interactable = false;
                random.interactable = false;*/
            
        /*
            else
            {
                playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
         /*
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + (adding + 1).ToString();
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + (adding + 1).ToString();
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + (adding + 1).ToString();
               
            }*/ 
        }
        
        public void HandleMasterVolume(float value)
        {
            GameManager.Instance.SoundManager.SetMasterVolume(value);
        }

        public void HandleFXVolume(float value)
        {   
            GameManager.Instance.SoundManager.SetFXVolume(value);
        }

        public void HandleMusicVolume(float value)
        {
            GameManager.Instance.SoundManager.SetMusicVolume(value);
        }

        public void PlayGame(GameObject newCanvas)
        {
            GameManager.Instance.SoundManager.PlayAudioFX(start, 0.5f, false, Vector3.zero, 0);
            ChangeCanvas(newCanvas);
            StartCoroutine(StartGame());    
        }

        public void Exit()
        {
            GameManager.Instance.SoundManager.PlayAudioFX(back, 0.5f, false, Vector3.zero, 0);
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


