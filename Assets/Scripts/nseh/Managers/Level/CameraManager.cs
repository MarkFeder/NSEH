﻿using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.General;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Managers.Level
{
    public class CameraManager : LevelEvent
    {
        List<GameObject> _sceneCameras;
        Vector3 _player1position;
        Vector3 _player2position;
        Vector3 _player3position;
        Vector3 _player4position;
        GameObject _environmentCamera;
        GameObject _characterCamera;
        GameObject _TextCamera;

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
            _environmentCamera = Object.Instantiate(Resources.Load("LevelCamera"), Vector3.zero, Quaternion.Euler(0,180,0)) as GameObject;
            _characterCamera = Object.Instantiate(Resources.Load("CharacterCamera"), Vector3.zero, Quaternion.Euler(0, 180, 0)) as GameObject;
            _TextCamera = Object.Instantiate(Resources.Load("TextCamera"), Vector3.zero, Quaternion.Euler(0, 180, 0)) as GameObject;
            _sceneCameras.Add(_environmentCamera);
            _sceneCameras.Add(_characterCamera);
            _sceneCameras.Add(_TextCamera);
        }

        public override void EventTick()
        {
            switch (nseh.Managers.Main.GameManager.Instance._numberPlayers)
            {
                case 1:
                    _player1position = LvlManager.GetPlayer1().transform.position;
                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player1position, _player1position, _player1position);
                    }
                    break;

                case 2:
                    _player1position = LvlManager.GetPlayer1().transform.position;
                    _player2position = LvlManager.GetPlayer2().transform.position;
                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player2position, _player1position, _player2position);
                    }
                    break;

                case 4:
                    _player1position = LvlManager.GetPlayer1().transform.position;
                    _player2position = LvlManager.GetPlayer2().transform.position;
                    _player3position = LvlManager.GetPlayer3().transform.position;
                    _player4position = LvlManager.GetPlayer4().transform.position;
                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player2position, _player3position, _player4position);
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