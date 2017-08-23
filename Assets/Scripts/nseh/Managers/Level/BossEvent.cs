using nseh.Managers.Main;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using nseh.Gameplay.Misc;
using nseh.Gameplay.Entities.Player;
using nseh.Gameplay.Base.Interfaces;
using nseh.Managers.UI;
using UnityEngine.UI;
using nseh.Gameplay.Entities.Environment;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Managers.Level
{
    public class BossEvent : Service
    {

        #region Private Properties

        private bool _isPaused;
        private GameObject _loading;
        private GameObject _boss;
        private GameObject _canvasPause;
        private CanvasPlayersHUDManager _canvasPlayers;
        private Text _ready;
        private List<IEvent> _events;
        private List<GameObject> _spawnPoints;
        private List<GameObject> _players;
        private Dictionary<string, GameObject> _playerPrefabs;
        private List<AudioSource> _ambientSounds;

        #endregion

        #region Public Properties

        public bool death;
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

        public Dictionary<string, GameObject> PlayerPrefabs
        {
            get
            {
                return _playerPrefabs;
            }

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

        public List<GameObject> Players
        {
            get
            {
                return _players;
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
            _playerPrefabs = new Dictionary<string, GameObject>();
        }

        public override void Activate()
        {
            _isActivated = true;
            started = false;
            _isPaused = false;
            _canvasPause.SetActive(false);
            CanvasPlayers.DisableAllHuds();
            death = false;
            SpawnPlayers();

            if (_events != null)
            {
                foreach (IEvent events in _events)
                {
                    events.ActivateEvent();
                }
            }

            MyGame.StartCoroutine(StartBoss());
        }

        public override void Tick()
        {
            if (started)
            {
                if (death)
                {
                    death = false;
                    MyGame.StartCoroutine(StopBoss());
                }

                else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))))
                {
                    MyGame.TogglePause(_canvasPause);
                }
            }
        }

        public override void Release()
        {
            if (_events != null)
            {
                foreach (IEvent events in _events)
                {
                    events.EventRelease();
                }
            }

            death = false;

            foreach (GameObject character in _players)
            {
                GameManager.Instance._score[(character.GetComponent<PlayerInfo>().GamepadIndex) - 1, 2] = character.GetComponent<PlayerInfo>().CurrentScore;
            }
            _players = new List<GameObject>();
            _isActivated = false;
        }

        #endregion

        #region Public Methods

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

        private IEnumerator ScoreMenu()
        {
            yield return new WaitForSeconds(5);
            MyGame.ChangeState(GameManager.States.Score);
        }

        private IEnumerator StartBoss()
        {
            yield return new WaitForSeconds(1);
            _loading.SetActive(false);
            GameManager.Instance.SoundManager.PlayAudioMusic(Camera.main.GetComponent<AudioSource>());
            Ready.text = "DEFEAT THE BOSS TOGETHER!";
            yield return new WaitForSeconds(3);
            Ready.text = "";
            started = true;
            MyGame.canPaused = true;
        }

        private IEnumerator StopBoss()
        {
            Ready.text = "BAVA DONGO IS DOWN! YOU WIN!";
            MyGame.canPaused = false;
            yield return new WaitForSeconds(5);
            Ready.text = "";
            _loading.SetActive(true);
            yield return new WaitForSeconds(1);
            Release();
            MyGame.StartCoroutine(ScoreMenu());
        }
       
        private void SpawnPlayers()
        { 
            GameObject _aux;
            Image auxPortrail;
            for (int i = 0; i < MyGame._numberPlayers; i++)
            {
                _aux = Object.Instantiate(PlayerPrefabs[MyGame._characters[i]]);
                _aux.transform.position = SpawnPoints[i].transform.position;
                _aux.transform.rotation = SpawnPoints[i].transform.rotation;
                _aux.GetComponent<PlayerInfo>().GamepadIndex = i + 1;
                //_aux.transform.GetChild(4).GetComponent<PlayerText>().playerText = i + 1;
                CanvasPlayers.EnableHud(i + 1);
                auxPortrail = CanvasPlayers.GetPortraitForPlayer(i + 1);
                auxPortrail.sprite = _aux.GetComponent<PlayerInfo>().CharacterPortrait;
                PlayerInfo _playerInfo = _aux.GetComponent<PlayerInfo>();
                _playerInfo.HealthBar = CanvasPlayers.GetHealthBarComponentForPlayer(i + 1);
                _playerInfo.EnergyBar = CanvasPlayers.GetEnergyBarComponentForPlayer(i + 1);
                _aux.layer = 13 + i;
                _playerInfo.PlayerMovement.DisableMovement(0);
                _players.Add(_aux);

            }

            PlayerPrefabs = null;
        }

        #endregion

    }
}