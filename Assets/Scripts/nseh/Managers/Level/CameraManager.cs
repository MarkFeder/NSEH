using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.General;
using nseh.Managers.Main;
using System.Collections.Generic;
using UnityEngine;
using ResourcesPath = nseh.Utils.Constants.Resources;

namespace nseh.Managers.Level
{
    public class CameraManager : LevelEvent
    {
        #region Private Properties

        private List<GameObject> _sceneCameras;

        private Vector3 _player1position;
        private Vector3 _player2position;
        private Vector3 _player3position;
        private Vector3 _player4position;

        private GameObject _environmentCameraObj;
        private GameObject _characterCameraObj;
        private GameObject _textCameraObj;

        private Camera _environmentCamera;
        private Camera _characterCamera;
        private Camera _textCamera;

        #endregion

        #region Public Properties

        public List<GameObject> SceneCameras { get { return _sceneCameras; } }

        // Camera gameobjects

        public GameObject EnvironmentCameraObj { get { return _environmentCameraObj; } }

        public GameObject CharacterCameraObj { get { return _characterCameraObj; } }

        public GameObject TextCameraObj { get { return _textCameraObj; } }

        // Camera components

        public Camera EnvironmentCamera { get { return _environmentCamera; } }

        public Camera CharacterCamera { get { return _characterCamera; } }

        public Camera TextCamera { get { return _textCamera; } }

        #endregion

        #region Public Methods

        public override void Setup(LevelManager levelManager)
        {
            base.Setup(levelManager);
        }

        public override void ActivateEvent()
        {
            _isActivated = true;
            _sceneCameras = new List<GameObject>();

            // TODO: it would be nice to add a switch which controls the camera 
            // instantiation depending on LevelManager states.
            _environmentCameraObj = Object.Instantiate(Resources.Load(ResourcesPath.CAMERAS_LEVEL_CAMERA), Vector3.zero, Quaternion.Euler(0, 180, 0)) as GameObject;
            _characterCameraObj = Object.Instantiate(Resources.Load(ResourcesPath.CAMERAS_CHARACTER_CAMERA), Vector3.zero, Quaternion.Euler(0, 180, 0)) as GameObject;
            _textCameraObj = Object.Instantiate(Resources.Load(ResourcesPath.CAMERAS_TEXT_CAMERA), Vector3.zero, Quaternion.Euler(0, 180, 0)) as GameObject;

            _sceneCameras.Add(_environmentCameraObj);
            _sceneCameras.Add(_characterCameraObj);
            _sceneCameras.Add(_textCameraObj);

            _environmentCamera = _environmentCameraObj.GetComponent<Camera>();
            _characterCamera = _characterCameraObj.GetComponent<Camera>();
            _textCamera = _textCameraObj.GetComponent<Camera>();
        }

        public override void EventTick()
        {
            switch (GameManager.Instance._numberPlayers)
            {
                case 1:

                    _player1position = _levelManager.GetPlayer1().transform.position;

                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player1position, _player1position, _player1position);
                    }

                    break;

                case 2:

                    _player1position = _levelManager.GetPlayer1().transform.position;
                    _player2position = _levelManager.GetPlayer2().transform.position;

                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player2position, _player1position, _player2position);
                    }

                    break;

                case 4:

                    _player1position = _levelManager.GetPlayer1().transform.position;
                    _player2position = _levelManager.GetPlayer2().transform.position;
                    _player3position = _levelManager.GetPlayer3().transform.position;
                    _player4position = _levelManager.GetPlayer4().transform.position;

                    foreach (GameObject thisCamera in _sceneCameras)
                    {
                        thisCamera.GetComponent<CameraComponent>().RefreshCamera(_player1position, _player2position, _player3position, _player4position);
                    }

                    break;
            }
        }

        public override void EventRelease()
        {
            foreach (GameObject thisCamera in _sceneCameras)
            {
                GameObject.Destroy(thisCamera);
            }
            _sceneCameras = new List<GameObject>();
            _isActivated = false;
        }

        #endregion
    }
}