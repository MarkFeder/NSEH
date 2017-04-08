using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Gameflow;
using nseh.Managers.UI;
using nseh.Managers.Main;
using nseh.Utils;
using nseh.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LevelHUDConstants = nseh.Utils.Constants.InLevelHUD;

namespace nseh.Managers.Level
{
    public class LevelManager : Service
    {
        #region Public Properties

        public enum States { LevelEvent, Minigame };
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

        private CanvasPausedHUDManager _canvasPausedManager;
        private CanvasClockHUDManager _canvasClockManager;
        private CanvasGameOverHUDManager _canvasGameOverManager;
        private CanvasPlayersHUDManager _canvasPlayersManager;
        private CanvasItemsHUDManager _canvasItemsManager;

        private List<PlayerManager> _players;
        private List<Vector3> _playersPos;
        private List<Vector3> _playersRots;

        private List<LevelEvent> _eventsList;

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

        public CanvasClockHUDManager CanvasClockManager
        {
            get { return _canvasClockManager; }
        }

        public CanvasGameOverHUDManager CanvasGameOverManager
        {
            get { return _canvasGameOverManager; }
        }

        public CanvasPlayersHUDManager CanvasPlayersManager
        {
            get { return _canvasPlayersManager; }
        }

        public CanvasItemsHUDManager CanvasItemsManager
        {
            get { return _canvasItemsManager; }
        }

        #endregion

        #region Private Methods

        private void Add<T>() where T : new()
        {
            LevelEvent serviceToAdd = new T() as LevelEvent;
            serviceToAdd.Setup(this);
            _eventsList.Add(serviceToAdd);
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

                        Find<Tar_Event>().ActivateEvent();
                        Find<CameraManager>().ActivateEvent();
                        Find<ItemSpawn_Event>().ActivateEvent();

                        _currentState = _nextState;

                        break;

                    case States.Minigame:

                        Find<Tar_Event>().EventRelease();
                        Find<CameraManager>().EventRelease();
                        Find<ItemSpawn_Event>().EventRelease();

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
                Time.timeScale = 0;

                _canvasGameOverManager.GameOverText.text = "Time's Up";
                _canvasGameOverManager.EnableCanvas();
                _isGameOver = true;

            }
        }

        public void Restart()
        {
            // Refresh variables
            _isGameOver = false;
            _isPaused = false;
            _timeRemaining = Constants.LevelManager.TIME_REMAINING;
            Time.timeScale = 1;
                
            // Deactivate some canvas
            _canvasGameOverManager.DisableCanvas();
            _canvasPausedManager.DisableCanvas();

            // Activate events
            Find<Tar_Event>().ActivateEvent();
            Find<ItemSpawn_Event>().ActivateEvent();

            // Release managers
            Find<CameraManager>().EventRelease();

            // Respawn all the players again without loading prefabs again
            RespawnAllPlayers();

            // Reactivate camera again
            Find<CameraManager>().ActivateEvent();
        }

        public void GoToMainMenu()
        {
            _isPaused = false;
            _canvasLoaded = false;
            MyGame.ChangeState(Main.GameManager.States.MainMenu);
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

        #endregion

        #region LevelEvent Public Methods

        public override void Setup(Main.GameManager myGame)
        {
            base.Setup(myGame);

            // Setup lists
            _eventsList = new List<LevelEvent>();
            _players = new List<PlayerManager>();
            _playersPos = new List<Vector3>();
            _playersRots = new List<Vector3>();

            _currentState = States.LevelEvent;
            _isPaused = false;
            _canvasLoaded = false;

            // Submit managers and events
            Add<Tar_Event>();
            Add<CameraManager>();
            Add<ItemSpawn_Event>();
        }

        public override void Activate()
        {
            IsActivated = true;

            // Fill some variables
            _numPlayers = Main.GameManager.Instance._numberPlayers;
            _isGameOver = false;
            Time.timeScale = 1;
            _timeRemaining = Constants.LevelManager.TIME_REMAINING;

            // Check if canvas were loaded
            if (!_canvasLoaded)
            {
                SetupLevelCanvas();
            }

            // Disable some canvas
            _canvasPausedManager.DisableCanvas();
            _canvasGameOverManager.DisableCanvas();
            _clock = _canvasClockManager.ClockText;

            // Activate events
            Find<Tar_Event>().ActivateEvent();
            Find<ItemSpawn_Event>().ActivateEvent();
            Find<CameraManager>().ActivateEvent();

            Debug.Log("The number of players is: " + Main.GameManager.Instance._numberPlayers + " " + (Main.GameManager.Instance._characters[0].name));

            // Activate some canvas
            _canvasPlayersManager.EnableCanvas();
            _canvasPlayersManager.DisableAllHuds();
            _canvasClockManager.EnableCanvas();
            _canvasItemsManager.EnableCanvas();

            // Setup players on screen
            SetupPlayersTransforms();
            SpawnAllPlayers();
        }

        //This is where the different events are triggered in a similar way to a state machine. This method is very similar to MonoBehaviour.Update()
        public override void Tick()
        {
            if (_timeRemaining > 0 && !_isGameOver)
            {
                Clock();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !_isGameOver)
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
            IsActivated = false;
            Find<ItemSpawn_Event>().EventRelease();

            _canvasGameOverManager.DisableCanvas();
            _canvasPausedManager.DisableCanvas();
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
                        new Vector3(-12, 1, 2)
                    };

                    _playersRots = new List<Vector3>()
                    {
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
                _players[i].Setup(Main.GameManager.Instance._characters[i], _playersPos[i],
                                  _playersRots[i], i + 1, _canvasPlayersManager.GetBarComponentForPlayer(i + 1));

                // Change player's portrait from hud manager
                _canvasPlayersManager.ChangePortrait(i + 1, _players[i].PlayerRunTimeInfo.CharacterPortrait);
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

        #endregion
    } 
}
