using nseh.Managers.Main;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Managers.UI
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
            current.SetActive(false);
            current = newCanvas;
            current.SetActive(true);

            if (current.name.Contains("Canvas_PickingCharacters"))
            {
                adding = 0;
                playerTurnText.text = "UNEMPLOYMENT OFFICE";
                play.interactable = false;
            }

        }

        public void AddingWrarr()
        {
            SelectingCharacter("Wrarr", wrarr);
        }  

        public void AddingProspector()
        {
            SelectingCharacter("SirProspector", prospector);         
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

        private void SelectingCharacter (string name, MyButton button)
        {
            /*
            if (GameManager.Instance._characters[button.player] == "")
                adding++;
            _MenuManager.Adding(name, button.player);
            */

            _MenuManager.Adding(name, adding);
            adding++;

            GameManager.Instance.SoundManager.PlayAudioFX(select, 0.5f, false, Vector3.zero, 0);

            if (adding == GameManager.Instance._numberPlayers)
            {
                playerTurnText.text = "READY?";
                play.interactable = true;
                eventSystem[0].SetSelectedGameObject(selectedGameObject);

            }
        }

        #endregion

    }
}


