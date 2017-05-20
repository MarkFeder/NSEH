using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Environment;
using nseh.Gameplay.Gameflow;
using nseh.Managers.Main;
using nseh.Managers.Pool;
using nseh.Managers.UI;
using nseh.Utils;
using nseh.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Inputs = nseh.Utils.Constants.Input;
using LevelHUDConstants = nseh.Utils.Constants.InLevelHUD;

namespace nseh.Managers.Level
{
    public class LevelManager : Service
    {
        #region Public Properties

        public enum States { LevelEvent, LoadingMinigame, Minigame, LoadingLevel, Boss, LoadingBoss};
        public States _currentState;

        #endregion

        #region Private Properties

        private States _nextState;

        private Text _clock;

        private GameObject _canvasPausedObj;
        private GameObject _canvasClockObj;
        private GameObject _canvasGameOverObj;
        private GameObject _canvasPlayersHUDObj;
        private GameObject _canvasItemsObj;
        //private GameObject _canvasProgressObj;

        private GameObject _canvasPausedMinigameObj;
        private GameObject _canvasClockMinigameObj;
        private GameObject _canvasGameOverMinigameObj;
        private GameObject _canvasPausedBossObj;

        private CanvasPausedHUDManager _canvasPausedManager;
        private CanvasClockHUDManager _canvasClockManager;
        private CanvasClockMinigameHUDManager _canvasClockMinigameManager;
        private CanvasPausedMinigameHUDManager _canvasPauseMinigameManager;
        private CanvasGameOverMinigameHUDManager _canvasGameOverMinigameManager;
        private CanvasGameOverHUDManager _canvasGameOverManager;
        private CanvasPlayersHUDManager _canvasPlayersManager;
        private CanvasItemsHUDManager _canvasItemsManager;
        private CanvasProgressHUDManager _canvasProgressManager;
        private CanvasPausedBossHUDManager _canvasPauseBossManager;

        private List<PlayerManager> _players;
        private List<Vector3> _playersPos;
        private List<Vector3> _playersRots;
        private List<GameObject> _playerSpawnPoints;

        private List<LevelEvent> _eventsList;

        private ParticlesManager _particlesManager;
        private ObjectPoolManager _objectPoolManager;

        private bool _isGameOver;
        private bool _isPaused;
        private bool _canvasLoaded;

        private float _timeRemaining;
        private int _numPlayers;

        #endregion

        #region Public C# Properties

        public CanvasPausedHUDManager CanvasPausedManager
        {
            get { return _canvasPausedManager; }
        }

        public CanvasPausedMinigameHUDManager CanvasPausedMinigameManager
        {
            get { return _canvasPauseMinigameManager; }
        }

        public CanvasClockHUDManager CanvasClockManager
        {
            get { return _canvasClockManager; }
        }

        public CanvasClockMinigameHUDManager CanvasClockMinigameManager
        {
            get { return _canvasClockMinigameManager; }
        }

        public CanvasGameOverHUDManager CanvasGameOverManager
        {
            get { return _canvasGameOverManager; }
        }

        public CanvasGameOverMinigameHUDManager CanvasGameOverMinigameManager
        {
            get { return _canvasGameOverMinigameManager; }
        }

        public CanvasPausedBossHUDManager CanvasPausedBossManager
        {
            get { return _canvasPauseBossManager; }
        }

        public CanvasPlayersHUDManager CanvasPlayersManager
        {
            get { return _canvasPlayersManager; }
        }

        public CanvasItemsHUDManager CanvasItemsManager
        {
            get { return _canvasItemsManager; }
        }

        public CanvasProgressHUDManager CanvasProgressManager
        {
            get { return _canvasProgressManager; }
        }


        public List<PlayerManager> Players
        {
            get { return _players; }
        }

        public int numPlayers
        {
            get { return _numPlayers; }
        }


        #endregion

        #region Cached SubManagers

        public ParticlesManager ParticlesManager
        {
            get { return _particlesManager; }
        }

        public ObjectPoolManager ObjectPoolManager
        {
            get { return _objectPoolManager; }
        }

        #endregion

        #region Private Methods

        private void Add<T>()
            where T : new()
        {
            LevelEvent serviceToAdd = new T() as LevelEvent;
            serviceToAdd.Setup(this);
            _eventsList.Add(serviceToAdd);
            Debug.Log(serviceToAdd);
        }

        #endregion

        #region Public Methods

        public T Find<T>() where T : class
        {
            foreach (LevelEvent thisEvent in _eventsList)
            {
                if (thisEvent.GetType() == typeof(T))
                    return thisEvent as T;
            }

            return null;
        }

        public void PauseGame()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                _canvasPausedManager.EnableCanvas();
                Time.timeScale = 0;
            }
            else
            {
                _canvasPausedManager.DisableCanvas();
                Time.timeScale = 1;
            }
        }

        public void ChangeState(States newState)
        {
            _nextState = newState;
            if (_nextState != _currentState)
            {
                switch (_nextState)
                {
                    case States.LevelEvent:
                        Find<MinigameEvent>().EventRelease();
                        _canvasLoaded = false;
                        Activate();
                        //Restart();
                        /*
                        Find<Tar_Event>().ActivateEvent();
                        Find<CameraManager>().ActivateEvent();
                        Find<ItemSpawn_Event>().ActivateEvent();
                        Find<LevelProgress>().ActivateEvent();
                        */

                        _currentState = _nextState;

                        break;

                    case States.LoadingMinigame:

                        //Time.timeScale = 0;
                        Find<Tar_Event>().EventRelease();
                        Find<CameraManager>().EventRelease();
                        Find<ItemSpawn_Event>().EventRelease();
                        _playerSpawnPoints = new List<GameObject>();
                        foreach (PlayerManager character in Players)
                        {
                            MyGame._score[character.PlayerRunTimeInfo.Player - 1, 0] = character.PlayerRunTimeInfo.Score;
                        }
                        SceneManager.LoadScene("Minigame");
                        Find<LoadingEvent>().ActivateEvent();
                        _currentState = _nextState;

                        break;

                    case States.Minigame:
                        Time.timeScale = 1;
                        SetupMinigameCanvas();
                        Find<MinigameEvent>().ActivateEvent();



                        _currentState = _nextState;

                        break;

                    case States.LoadingLevel:
                        Find<MinigameEvent>().EventRelease();
                        _players = new List<PlayerManager>();
                        _playerSpawnPoints = new List<GameObject>();
                        SceneManager.LoadScene("Game");
                        Find<LoadingEvent>().ActivateEvent();

                        _currentState = _nextState;
                        break;


                    case States.LoadingBoss:
                        Find<MinigameEvent>().EventRelease();
                        SceneManager.LoadScene("Boss");
                        Find<LoadingEvent>().ActivateEvent();
                        _currentState = _nextState;
                        break;


                    case States.Boss:
                        Time.timeScale = 1;
                        SetupBossCanvas();
                        Find<BossEvent>().ActivateEvent();
                        _currentState = _nextState;
                        break;

                }
            }
        }

        public void Clock()
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
                _canvasClockManager.ClockText.text = "";
                _canvasClockManager.DisableCanvas();
                ChangeState(LevelManager.States.LoadingMinigame);
                //Time.timeScale = 0;

                //_canvasGameOverManager.GameOverText.text = "Time's Up";
                //_canvasGameOverManager.EnableCanvas();
                //_isGameOver = true;
            }
        }

        public void Restart()
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                // Refresh variables
                _isGameOver = false;
                _isPaused = false;
                _timeRemaining = Constants.LevelManager.TIME_REMAINING;
                Time.timeScale = 1;

                // Deactivate some canvas
                _canvasGameOverManager.DisableCanvas();
                _canvasPausedManager.DisableCanvas();

                // Activate some canvas
                _canvasClockManager.EnableCanvas();

                // Activate events
                Find<Tar_Event>().ActivateEvent();
                Find<ItemSpawn_Event>().ActivateEvent();

                // Release managers
                Find<CameraManager>().EventRelease();
                Find<MinigameEvent>().EventRelease();
                // Reset all player spawn points
                ResetPlayerSpawnPoint();

                // Respawn all the players again without loading prefabs again
                RespawnAllPlayers();

                // Reactivate events again
                Find<CameraManager>().ActivateEvent();
            }
            else
            {
                ChangeState(LevelManager.States.LoadingLevel);
            }
        }

        public void GoToMainMenu()
        {
            _isPaused = false;

            _canvasLoaded = false;
            MyGame.ChangeState(Main.GameManager.States.MainMenu);
        }


        public void GoToMainMenuScore()
        {
            _isPaused = false;

            _canvasLoaded = false;
            MyGame.ChangeState(Main.GameManager.States.Score);

            /*
            foreach (PlayerManager character in Players)
            {
            MyGame._score[character.PlayerRunTimeInfo.Player - 1, 2] = 2;
            }*/
        }

        public GameObject GetPlayer1()
        {
            return _players[0].PlayerRunTime;
        }

        public GameObject GetPlayer2()
        {
            return _players[1].PlayerRunTime;
        }

        public GameObject GetPlayer3()
        {
            return _players[2].PlayerRunTime;
        }

        public GameObject GetPlayer4()
        {
            return _players[3].PlayerRunTime;
        }

        public void RespawnPlayerFromDeath(int player)
        {
            if (_players.Count() > 0)
            {
                _players[player - 1].ResetFromDeath();
            }
            else
            {
                Debug.Log("RespawnAllPlayers(): the number of players is 0 or less than 0");
            }
        }

        public void RegisterPlayerSpawnPoint(GameObject spawnToRegister)
        {
            Debug.Log(spawnToRegister);
            _playerSpawnPoints.Add(spawnToRegister);
            Debug.Log(spawnToRegister);
        }

        #endregion

        #region LevelEvent Public Methods

        public override void Setup(GameManager myGame)
        {
            base.Setup(myGame);

            // Setup lists
            _eventsList = new List<LevelEvent>();
            _players = new List<PlayerManager>();
            _playersPos = new List<Vector3>();
            _playersRots = new List<Vector3>();
            _playerSpawnPoints = new List<GameObject>();

            _currentState = States.LevelEvent;
            _isPaused = false;
            _canvasLoaded = false;

            // Submit managers and events
            Add<Tar_Event>();
            Add<CameraManager>();
            Add<ItemSpawn_Event>();
            Add<MinigameEvent>();
            Add<LoadingEvent>();
            Add<ParticlesManager>();
            Add<ObjectPoolManager>();
            Add<BossEvent>();

            // Cached submanagers
            _particlesManager = Find<ParticlesManager>();
            _objectPoolManager = Find<ObjectPoolManager>();
        }

        public override void Activate()
        {
            _isActivated = true;

            // Activate submanagers
            _particlesManager.ActivateEvent();
            _objectPoolManager.ActivateEvent();

            // Fill some variables
            _numPlayers = Main.GameManager.Instance._numberPlayers;
            _isGameOver = false;
            Time.timeScale = 1;
            _timeRemaining = Constants.LevelManager.TIME_REMAINING;

            // Check if all canvas were loaded
            if (!_canvasLoaded)
            {
                SetupLevelCanvas();
            }

            // Disable some canvas
            _canvasPausedManager.DisableCanvas();
            _canvasGameOverManager.DisableCanvas();
            _clock = _canvasClockManager.ClockText;

            // Activate some canvas
            _canvasPlayersManager.EnableCanvas();
            _canvasPlayersManager.DisableAllHuds();
            _canvasClockManager.EnableCanvas();
            _canvasItemsManager.EnableCanvas();

            // Setup players on screen
            SetupPlayersTransforms();
            SpawnAllPlayers();

            Debug.Log("The number of players is: " + GameManager.Instance._numberPlayers + " " + (GameManager.Instance._characters[0].name));

            // Activate events
            Find<Tar_Event>().ActivateEvent();
            Find<ItemSpawn_Event>().ActivateEvent();
            Find<CameraManager>().ActivateEvent();
        }

        public override void Tick()
        {
            if (_timeRemaining > 0 /*&& !_isGameOver*/ && SceneManager.GetActiveScene().name == "Game")
            {
                Clock();
            }

            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(System.String.Format("{0}{1}", Inputs.OPTIONS, 1))) &&
                SceneManager.GetActiveScene().name == "Game")
            {
                PauseGame();
            }

            if (!_isPaused)
            {
                foreach (LevelEvent thisEvent in _eventsList)
                {
                    if (thisEvent.IsActivated)
                        thisEvent.EventTick();
                }
            }
        }

        public override void Release()
        {
            _isActivated = false;

            // Set current state
			_currentState = States.LevelEvent;

            // Release events
            Find<ItemSpawn_Event>().EventRelease();
			Find<MinigameEvent>().EventRelease();
			_particlesManager.EventRelease();
            _objectPoolManager.EventRelease();

            // Clear lists so as to avoid conflicts
            _players.Clear();
			_playerSpawnPoints.Clear();

            //_canvasGameOverManager.DisableCanvas();
            //_canvasPausedManager.DisableCanvas();
        }

        #endregion

        #region Private Methods

        private void SetupLevelCanvas()
        {
            _canvasLoaded = true;

            // Load canvas from prefabs
            _canvasPausedObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_PAUSED_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasClockObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_CLOCK_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasGameOverObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_GAME_OVER_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasPlayersHUDObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_PLAYERS_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasItemsObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_ITEMS_HUD), Vector3.zero, Quaternion.identity) as GameObject;

            // Load canvas managers
            _canvasPausedManager = _canvasPausedObj.GetComponent<CanvasPausedHUDManager>();
            _canvasClockManager = _canvasClockObj.GetComponent<CanvasClockHUDManager>();
            _canvasGameOverManager = _canvasGameOverObj.GetComponent<CanvasGameOverHUDManager>();
            _canvasPlayersManager = _canvasPlayersHUDObj.GetComponent<CanvasPlayersHUDManager>();
            _canvasItemsManager = _canvasItemsObj.GetComponent<CanvasItemsHUDManager>();
        }

        private void SetupMinigameCanvas()
        {
            _canvasLoaded = true;

            // Load canvas from prefabs
            _canvasPausedMinigameObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_PAUSED_MINIGAME_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasClockMinigameObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_CLOCK_MINIGAME_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            //_canvasGameOverMinigameObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_GAME_OVER_MINIGAME_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasGameOverObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_GAME_OVER_MINIGAME_HUD), Vector3.zero, Quaternion.identity) as GameObject;

            // Load canvas managers

            _canvasPauseMinigameManager = _canvasPausedMinigameObj.GetComponent<CanvasPausedMinigameHUDManager>();
            _canvasClockMinigameManager = _canvasClockMinigameObj.GetComponent<CanvasClockMinigameHUDManager>();
            _canvasGameOverMinigameManager = _canvasGameOverObj.GetComponent<CanvasGameOverMinigameHUDManager>();
        }


        private void SetupBossCanvas()
        {
            _canvasLoaded = true;

            _canvasPausedBossObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_PAUSED_BOSS_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            //_canvasProgressObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_PROGRESS_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasPlayersHUDObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_PLAYERS_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            _canvasItemsObj = Object.Instantiate(Resources.Load(LevelHUDConstants.CANVAS_ITEMS_HUD), Vector3.zero, Quaternion.identity) as GameObject;
            
            _canvasPauseBossManager = _canvasPausedBossObj.GetComponent<CanvasPausedBossHUDManager>();
            //_canvasProgressManager = _canvasProgressObj.GetComponent<CanvasProgressHUDManager>();
            _canvasPlayersManager = _canvasPlayersHUDObj.GetComponent<CanvasPlayersHUDManager>();
            _canvasItemsManager = _canvasItemsObj.GetComponent<CanvasItemsHUDManager>();
            _canvasPlayersManager.DisableAllHuds();
            SetupPlayersTransformsBoss();
            SpawnAllPlayersBoss();
            
        }

        private void SetupPlayersTransforms()
        {
            switch (_numPlayers)
            {
                case 1:

                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(0, 1, 2)
                    };

                    _playersRots = new List<Vector3>()
                    {
                        new Vector3(0, -90, 0)
                    };

                    break;

                case 2:

                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(12, 1, 2),
                        new Vector3(-15, 1, 2)
                    };

                    _playersRots = new List<Vector3>()
                    {
                        new Vector3(0, -90, 0),
                        new Vector3(0, 90, 0)
                    };

                    break;

                case 4:

                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(12, 1, 2),
                        new Vector3(-15, 1, 2),
                        new Vector3(12, 10, 2),
                        new Vector3(-18, 12, 2)
                    };

                    _playersRots = new List<Vector3>()
                    {
                        new Vector3(0, -90, 0),
                        new Vector3(0, 90, 0),
                        new Vector3(0, -90, 0),
                        new Vector3(0, 90, 0)
                    };



                    break;

                    // Handle other cases here
            }
        }

        private void SetupPlayersTransformsBoss()
        {
            switch (_numPlayers)
            {
                case 1:

                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(0, 0, 0)
                    };

                    _playersRots = new List<Vector3>()
                    {
                        new Vector3(0, -90, 0)
                    };

                    break;

                case 2:

                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(1, 1, 0),
                        new Vector3(-15, 1, 0)
                    };

                    _playersRots = new List<Vector3>()
                    {
                        new Vector3(0, -90, 0),
                        new Vector3(0, 90, 0)
                    };

                    break;

                case 4:

                    _playersPos = new List<Vector3>()
                    {
                        new Vector3(1, 1, 0),
                        new Vector3(-15, 1, 0),
                        new Vector3(-5, 5, 0),
                        new Vector3(-10, 5, 0)
                    };

                    _playersRots = new List<Vector3>()
                    {
                        new Vector3(0, -90, 0),
                        new Vector3(0, 90, 0),
                        new Vector3(0, -90, 0),
                        new Vector3(0, 90, 0)
                    };

                    break;

                    // Handle other cases here
            }
        }

        private void SpawnAllPlayers()
        {
            for (int i = 0; i < _numPlayers; i++)
            {
                // Enable each hud
                _canvasPlayersManager.EnableHud(i + 1);

                // Add new player manager
                _players.AddNotDuplicate(new PlayerManager());
                _players[i].Setup(GameManager.Instance._characters[i], _playersPos[i],
                                  _playersRots[i], _playerSpawnPoints, i + 1, _canvasPlayersManager.GetHealthBarComponentForPlayer(i + 1),
                                  _canvasPlayersManager.GetEnergyBarComponentForPlayer(i + 1), _canvasPlayersManager.GetLivesForPlayer(i + 1));

                // Change player's portrait from hud manager
                _canvasPlayersManager.ChangePortrait(i + 1, _players[i].PlayerRunTimeInfo.CharacterPortrait);
                _canvasPlayersManager.DisableLivesForPlayer(i + 1);
            }
        }

        private void SpawnAllPlayersBoss()
        {
            for (int i = 0; i < _numPlayers; i++)
            {
                // Enable each hud
                _canvasPlayersManager.EnableHud(i + 1);

                // Add new player manager
                _players.AddNotDuplicate(new PlayerManager());

                _players[i].Setup(GameManager.Instance._characters[i], _playersPos[i],
                                  _playersRots[i], _playerSpawnPoints, i + 1, _canvasPlayersManager.GetHealthBarComponentForPlayer(i + 1),
                                  _canvasPlayersManager.GetEnergyBarComponentForPlayer(i + 1), _canvasPlayersManager.GetLivesForPlayer(i + 1), "Player");

                // Change player's portrait from hud manager
                _canvasPlayersManager.ChangePortrait(i + 1, _players[i].PlayerRunTimeInfo.CharacterPortrait);
                _canvasPlayersManager.DisableLivesForPlayer(i + 1);
            }
        }

        private void RespawnAllPlayers()
        {
            if (_players.Count() > 0)
            {
                for (int i = 0; i < _numPlayers; i++)
                {
                    _players[i].Reset();
                }
            }
            else
            {
                Debug.Log("RespawnAllPlayers(): the number of players is 0 or less than 0");
            }
        }

        private void ResetPlayerSpawnPoint()
        {
            foreach (GameObject playerSpawnPoint in _playerSpawnPoints)
            {
                PlayerSpawnPoint spawnPointComponent = playerSpawnPoint.GetComponent<PlayerSpawnPoint>();
                if (spawnPointComponent.IsFree == false)
                {
                    spawnPointComponent.SetFree();
                }
            }
        }

        #endregion
    }
}
