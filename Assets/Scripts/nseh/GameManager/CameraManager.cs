using System.Collections;
using System.Collections.Generic;
using nseh.GameManager.General;
using nseh.Gameplay.Base.Abstract.Gameflow;
using UnityEngine;

namespace nseh.GameManager
{
    public class CameraManager : LevelEvent
    {
        List<GameObject> _sceneCameras;
        Vector3 _player1position;
        Vector3 _player2position;
        GameObject _environmentCamera;
        GameObject _characterCamera;

        // Use this for initialization
        public override void Setup(LevelManager lvlManager)
        {
            base.Setup(lvlManager);
        }

        public override void ActivateEvent()
        {
            IsActivated = true;
            _sceneCameras = new List<GameObject>();
            //reminder: it would be nice to add a switch which controls the camera instantiation depending on LevelManager states.
            _environmentCamera = Object.Instantiate(Resources.Load("LevelCamera"), Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
            _characterCamera = Object.Instantiate(Resources.Load("CharacterCamera"), Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
            _sceneCameras.Add(_environmentCamera);
            _sceneCameras.Add(_characterCamera);
        }

        public override void EventTick()
        {
            switch (GameManager.Instance._numberPlayers)
            {
                case 1:
                    _player1position = LvlManager.getPlayer1().transform.position;
                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player1position);
                    }
                    break;

                case 2:
                    _player1position = LvlManager.getPlayer1().transform.position;
                    _player2position = LvlManager.getPlayer2().transform.position;
                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player2position);
                    }
                    break;
            }
        }

        public override void EventRelease()
        {
            foreach(GameObject thisCamera in _sceneCameras)
            {
                GameObject.Destroy(thisCamera);
            }
            _sceneCameras = new List<GameObject>();
            IsActivated = false;
        }
    }
}