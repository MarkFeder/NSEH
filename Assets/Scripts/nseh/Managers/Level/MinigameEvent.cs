using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nseh.Gameplay.Minigames;
using nseh.Managers.Main;
using nseh.Gameplay.Base.Interfaces;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Managers.Level
{
    public class MinigameEvent : Service
    {

        #region Private Properties

        private Text _ready;
        private GameObject _canvasPause;
        private List<GameObject> _spawnPoints;
        private GameObject _loading;
        private List<int> _puntuation;
        private List<GameObject> _players;
        private List<IEvent> _events;
        private Dictionary<string, GameObject> _playerPrefabs;
        private List<AudioSource> _ambientSounds;

        #endregion

        #region Public Properties

        public bool started;

        #endregion

        #region Public C# Properties

        public Text Ready
        {
            get
            {
                return _ready;
            }

            set
            {
                _ready = value;
            }
        }

        public GameObject CanvasPause
        {
            set
            {
                _canvasPause = value;
            }
        }

        public List<IEvent> Events
        {
            set
            {
                _events = value;
            }
        }

        public List<GameObject> Players
        {
            get
            {
                return _players;
            }

            set
            {
                _players = value;
            }
        }

        public List<int> Puntuation
        {
            get
            {
                return _puntuation;
            }

            set
            {
                _puntuation = value;
            }
        }

        public List<GameObject> SpawnPoints
        {
            set
            {
                _spawnPoints = value;
            }
        }

        public GameObject Loading
        {
            set
            {
                _loading = value;
            }
        }

        public Dictionary<string, GameObject> PlayerPrefabs
        {
            set
            {
                _playerPrefabs = value;
            }
        }

        public List<AudioSource> AmbientSounds
        {
            set
            {
                _ambientSounds = value;
            }
        }

        #endregion

        #region Service Management

        public override void Setup(GameManager myGame)
        {
            base.Setup(myGame);
            _events = new List<IEvent>();
            _players = new List<GameObject>();
            _playerPrefabs = new Dictionary<string, GameObject>();
        }

        public override void Activate()
        {
            _isActivated = true;
            _canvasPause.SetActive(false);
            started = false;
            Players = new List<GameObject>();
            Puntuation = new List<int>();

            for (int i = 0; i < GameManager.Instance._numberPlayers; i++)
                Puntuation.Add(0);

            SpawnPlayers();

            if (_events != null)
            {
                foreach (IEvent events in _events)
                {
                    events.ActivateEvent();
                }
            }

            _loading.SetActive(false);
            GameManager.Instance.SoundManager.PlayAudioMusic(Camera.main.GetComponent<AudioSource>());

        }

        public override void Tick()
        {
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))) && started)
            {
                GameManager.Instance.TogglePause(_canvasPause);
            }
        }

        public override void Release()
        {
            foreach (GameObject character in Players)
            {
                GameManager.Instance._score[(character.GetComponent<MinigameMovement>().gamepadIndex) - 1, 1] = character.GetComponent<MinigameMovement>().puntuation;
            }

            if (_events != null)
            {
                foreach (IEvent events in _events)
                {
                    events.EventRelease();
                }
            }

            Puntuation = new List<int>(GameManager.Instance._numberPlayers);
            Players = new List<GameObject>();           
            _isActivated = false;
        }

        #endregion

        #region Private Methods

        public IEnumerator ChangeStage()
        {
            yield return new WaitForSeconds(1.5f);
            _loading.SetActive(true);
            Release();
            yield return new WaitForSeconds(1);
            GameManager.Instance.ChangeState(GameManager.States.Boss);
        }

        private void SpawnPlayers ()
        {
            GameObject _aux;
     
            for (int i = 0; i<GameManager.Instance._characters.Count; i++)
            {
                    _aux = Object.Instantiate(_playerPrefabs[MyGame._characters[i]]);
                    _aux.transform.position = _spawnPoints[i].transform.position;
                    _aux.transform.rotation = _spawnPoints[i].transform.rotation;
                    //_aux.transform.GetChild(1).GetComponent<TextMinigame>().playerText = i + 1;
                    _aux.GetComponent<MinigameMovement>().gamepadIndex = i + 1;
                    Players.Add(_aux);
            }

            PlayerPrefabs = null;
        }

        #endregion

    }
}