using System.Collections;
using UnityEngine;
using nseh.Managers.Main;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Minigames;
using nseh.Managers.Level;

namespace nseh.Gameplay.Gameflow
{
    public class MinigameVolcano : MonoBehaviour, IEvent
    {

        #region Private Properties

        private Vector3 _gravity;
        private int _auxCount;
        private MinigameEvent _minigameEvent;

        #endregion

        #region Public Properties

        public GameObject _CubeDeath;
        public GameObject _Goal;
        public GameObject _fireGenerators;

        #endregion

        #region Service Management
        // Use this for initialization
        public void ActivateEvent()
        {
            _auxCount = 0;
            _gravity = Physics.gravity;
            _minigameEvent = GameManager.Instance.MinigameEvent;
            GameManager.Instance.StartCoroutine(CountDown());
            
        }

        // Update is called once per frame

        public void EventRelease()
        {
            Physics.gravity = _gravity;
        }
        #endregion

        #region Private Methods

        private IEnumerator CountDown()
        {

            _minigameEvent.Ready.text = "MASH X BUTTON TO AVOID THE LAVA!";
            yield return new WaitForSeconds(3);
            _minigameEvent.Ready.text = "USE THE JOYSTICK TO DODGE THE FIREBALLS!";
            yield return new WaitForSeconds(3);
            _minigameEvent.Ready.text = "READY";
            yield return new WaitForSeconds(1);
            _minigameEvent.Ready.text = "STEADY";
            yield return new WaitForSeconds(1);
            _minigameEvent.Ready.text = "RUUUUUUN!!!";
            yield return new WaitForSeconds(1);
            _minigameEvent.Ready.text = "";
            _minigameEvent.started = true;
            Physics.gravity = new Vector3(0, 0, -10);
            Camera.main.GetComponent<CameraScript>().started = true;
            _fireGenerators.GetComponent<FireballsGenerator>().started = true;
            foreach (GameObject character in _minigameEvent.Players)
            {
                character.GetComponent<MinigameMovement>().started = true;
                character.GetComponent<Animator>().SetBool("Start", true);
            }
        }

        #endregion

        #region Public Methods

        public void StopMinigame()
        {
            _minigameEvent.Ready.text = "SAFE!";
            Camera.main.GetComponent<CameraScript>().started = false;
            _fireGenerators.GetComponent<FireballsGenerator>().started = false;
        }

        public void AddPuntuation(int amount, int index)
        {
            _minigameEvent.Puntuation[index] = amount;
            _auxCount++;
            if (_auxCount == GameManager.Instance._characters.Count && Camera.main.GetComponent<CameraScript>().started == true)
            {
                _minigameEvent.Ready.text = "THIS NEVER HAPPENED...";
                Camera.main.GetComponent<CameraScript>().started = false;
                _fireGenerators.GetComponent<FireballsGenerator>().started = false;
                GameManager.Instance.StartCoroutine(_minigameEvent.ChangeStage());
            }

            else if (_auxCount == GameManager.Instance._characters.Count && Camera.main.GetComponent<CameraScript>().started == false)
            {
                GameManager.Instance.StartCoroutine(_minigameEvent.ChangeStage());
            }
        }

        #endregion
    }

    
}
