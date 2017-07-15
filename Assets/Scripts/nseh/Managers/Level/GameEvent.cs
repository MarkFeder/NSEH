using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using nseh.Managers.Main;
using nseh.Utils;
using nseh.Gameplay.Misc;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.UI;
using nseh.Gameplay.Entities.Environment;
using nseh.Managers.General;
using nseh.Gameplay.Base.Interfaces;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Managers.Level
{
    public class GameEvent : Service
    {
        #region Private Properties

        private float _timeRemaining;
        private Text _clock;
        private Text _ready;
        private GameObject _canvasPause;
        private CanvasPlayersHUDManager _canvasPlayers;
        private List <GameObject> _spawnPoints;
        private GameObject _loading;
        private List<IEvent> _events;
        private List<GameObject> _players;
        private Dictionary<string, GameObject> _playerPrefabs;
        private CameraComponent _camera;
        private List<AudioSource> _ambientSounds;

        #endregion

        #region Public C# Properties

        public Text Clock
        {
            set
            {
                _clock = value;
            }
        }

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

        public List<GameObject> SpawnPoints
        {
            get
            {
                return _spawnPoints;
            }

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

        public List<IEvent> Events
        {
            set
            {
                _events = value;
            }
        }

        public CanvasPlayersHUDManager CanvasPlayers
        {
            get
            {
                return _canvasPlayers;
            }

            set
            {
                _canvasPlayers = value;
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
            SpawnPoints = new List<GameObject>();
            _events = new List<IEvent>();
            _players = new List<GameObject>();
        }

        public override void Activate()
        {
            MyGame.isPaused = true;
            _isActivated = true;
            _timeRemaining = -1;
            _camera = Camera.main.GetComponent<CameraComponent>();
            CanvasPlayers.DisableAllHuds();
            SpawnPlayers();
            _canvasPause.SetActive(false);
            GameManager.Instance.StartCoroutine(StartGame());
        }

        public override void Tick()
        {    
            if (_timeRemaining > 0)
            {
                UpdateClock();
            }

            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))))
            {
                GameManager.Instance.TogglePause(_canvasPause);
            }
        }

        public override void Release()
        {
            foreach (GameObject character in _players)
             {
                 GameManager.Instance._score[(character.GetComponent<PlayerInfo>().GamepadIndex) - 1, 0] = character.GetComponent<PlayerInfo>().CurrentScore;
             }

            if (_events != null)
            {
                foreach (IEvent events in _events)
                {
                    events.EventRelease();
                }
            }

            Time.timeScale = 1;
            _players = new List<GameObject>();
            _isActivated = false;
        }

        #endregion

        #region Public Methods

        public void Restart()
        {
            _loading.SetActive(true);
            _canvasPause.SetActive(false);

            if (_events != null)
            {
                foreach (IEvent events in _events)
                {
                    events.EventRelease();
                }
            }

            _clock.text = "";
            ResetPlayerSpawnPoint();
            ResetPlayerPrefab();
            GameManager.Instance.StartCoroutine(CanvasLoadingWait());
            MyGame.isPaused = true;
            GameManager.Instance.StartCoroutine(StartGame());
        }

        public T Find<T>() where T : class
        {
            foreach (IEvent thisEvent in _events)
            {
                if (thisEvent.GetType() == typeof(T))
                    return thisEvent as T;
            }

            return null;
        }

        public void UpdateClock()
        {
            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining > 0 && _timeRemaining > 10)
            {
                _clock.text = _timeRemaining.ToString("f0");
            }

            else if (_timeRemaining > 0 && _timeRemaining < 10)
            {
                _clock.text = _timeRemaining.ToString("f2");
            }

            else
            {
                _clock.text = "";
                GameManager.Instance.StartCoroutine(StopGame());
            }
        }

        public void ResetFromDeath(GameObject deadPlayer)
        {
            List<GameObject> freePlayerSpawnPoints = new List<GameObject>();
            freePlayerSpawnPoints = _spawnPoints.FindAll(FindFreePlayerSpawnPoint);
            if (freePlayerSpawnPoints.Count != 0)
            {
                int randomSpawn = (int)UnityEngine.Random.Range(0, freePlayerSpawnPoints.Count);
                deadPlayer.transform.position = freePlayerSpawnPoints[randomSpawn].transform.position;
                deadPlayer.transform.rotation = freePlayerSpawnPoints[randomSpawn].transform.rotation;
                freePlayerSpawnPoints[randomSpawn].GetComponent<PlayerSpawnPoint>().ParticleAnimation(deadPlayer.transform);
                deadPlayer.GetComponent<PlayerMovement>().IsFacingRight = (deadPlayer.transform.localEulerAngles.y == 270.0f) ? true : false;
            }

            else
            {
                Debug.Log("There are problems with player's respawn");
            }

            deadPlayer.GetComponent<PlayerInfo>().ResetHealth();
            deadPlayer.GetComponent<PlayerInfo>().Animator.Play("Idle");
        }

        #endregion

        #region Private Methods

        private bool FindFreePlayerSpawnPoint(GameObject playerSpawnPoint)
        {
            return playerSpawnPoint.GetComponent<PlayerSpawnPoint>().IsFree;
        }

        private IEnumerator StartGame()
        {          
            yield return new WaitForSeconds(1);
            _loading.SetActive(false);
            GameManager.Instance.SoundManager.PlayAudioMusic(Camera.main.GetComponent<AudioSource>());
            GameManager.Instance.SoundManager.PlayAmbientSounds(_ambientSounds);
            Ready.text = "FIGHT THIS STRANGE PEOPLE! SAVE THE LAVA PIT!";
            yield return new WaitForSeconds(3);
            Ready.text = "FIIIIIIGHT!!!";
            yield return new WaitForSeconds(3);
            foreach (GameObject player in _players)
            {
                player.GetComponent<PlayerInfo>().PlayerMovement.EnableMovement();
                //habilitar combate
            }
            Ready.text = "";
            MyGame.isPaused = !MyGame.isPaused;
            _timeRemaining = Constants.LevelManager.TIME_REMAINING;

            if (_events != null)
            {
                foreach (IEvent events in _events)
                {
                    events.ActivateEvent();
                }
            }
        }

        private IEnumerator CanvasLoadingWait()
        {
            yield return new WaitForSeconds(1);
            _loading.SetActive(false);
        }

        private IEnumerator StopGame()
        {
            Time.timeScale = 0.5f;
            Ready.text = "THE LAVA IS RISING! RUN TO THE VOLCANO!";
            foreach (PlayerInfo element in _players.Select(t => t.GetComponent<PlayerInfo>()))
            {
                element.HealthMode = HealthMode.Invulnerability;
            }
                yield return new WaitForSeconds(3);
            _loading.SetActive(true);
            Release();
            yield return new WaitForSeconds(1);
            
            GameManager.Instance.ChangeState(GameManager.States.Minigame);
        }

        private void SpawnPlayers()
        {
            GameObject _aux;
            Image auxPortrail;
            for (int i = 0; i < GameManager.Instance._numberPlayers; i++)
            {
                _aux = Object.Instantiate(_playerPrefabs[MyGame._characters[i]]);
                _aux.transform.position = _spawnPoints[i].transform.position;
                _aux.transform.rotation = _spawnPoints[i].transform.rotation;
                _aux.GetComponent<PlayerInfo>().GamepadIndex = i + 1;
                _aux.transform.GetChild(4).GetComponent<PlayerText>().playerText = i + 1;
                _camera.positions.Add(_aux.transform);
                CanvasPlayers.EnableHud(i + 1);
                auxPortrail = CanvasPlayers.GetPortraitForPlayer(i+1);
                auxPortrail.sprite = _aux.GetComponent<PlayerInfo>().CharacterPortrait;
                PlayerInfo _playerInfo = _aux.GetComponent<PlayerInfo>();
                _playerInfo.HealthBar = CanvasPlayers.GetHealthBarComponentForPlayer(i+1);
                _playerInfo.EnergyBar = CanvasPlayers.GetEnergyBarComponentForPlayer(i + 1);
                _playerInfo.PlayerMovement.DisableMovement(0);
                //deshabilitar combate
                _players.Add(_aux);
            }
        }

        private void ResetPlayerSpawnPoint()
        {
            foreach (GameObject playerSpawnPoint in _spawnPoints)
            {
                PlayerSpawnPoint spawnPointComponent = playerSpawnPoint.GetComponent<PlayerSpawnPoint>();
                if (spawnPointComponent.IsFree == false)
                {
                    spawnPointComponent.SetFree();
                }
            }
        }

        private void ResetPlayerPrefab()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].transform.position = _spawnPoints[i].transform.position;
                _players[i].transform.rotation = _spawnPoints[i].transform.rotation;
                _players[i].GetComponent<PlayerMovement>().IsFacingRight = (_players[i].transform.localEulerAngles.y == 270.0f) ? true : false;
            }
        }

        #endregion

    }
}