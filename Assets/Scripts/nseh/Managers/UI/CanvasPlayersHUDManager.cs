using nseh.Managers.General;
using nseh.Managers.Main;
using nseh.Managers.Level;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace nseh.Managers.UI
{
    public class CanvasPlayersHUDManager : MonoBehaviour
    {

        #region Public Properties

        [Header("Player HUDs")]
        public GameObject _p1Hud;
        public GameObject _p2Hud;
        public GameObject _p3Hud;
        public GameObject _p4Hud;

        [Space(20)]
        [Header("Character portraits")]
        public Image _p1Portrait;
        public Image _p2Portrait;
        public Image _p3Portrait;
        public Image _p4Portrait;

        [Space(20)]
        [Header("Player Lives")]
        public GameObject _p1LivesHUD;
        public GameObject _p2LivesHUD;
        public GameObject _p3LivesHUD;
        public GameObject _p4LivesHUD;

        [Space(10)]
        public List<Image> _p1Lives;
        public List<Image> _p2Lives;
        public List<Image> _p3Lives;
        public List<Image> _p4Lives;

        #endregion

        #region Private Properties

        private BarComponent _p1HealthBarComponent;
        private BarComponent _p2HealthBarComponent;
        private BarComponent _p3HealthBarComponent;
        private BarComponent _p4HealthBarComponent;

        private BarComponent _p1EnergyBarComponent;
        private BarComponent _p2EnergyBarComponent;
        private BarComponent _p3EnergyBarComponent;
        private BarComponent _p4EnergyBarComponent;

        #endregion

        #region Public C# Properties

        public BarComponent P1HealthBarComponent
        {
            get { return _p1HealthBarComponent; }
        }

        public BarComponent P2HealthBarComponent
        {
            get { return _p2HealthBarComponent; }
        }

        public BarComponent P3HealthBarComponent
        {
            get { return _p3HealthBarComponent; }
        }

        public BarComponent P4HealthBarComponent
        {
            get { return _p4HealthBarComponent; }
        }

        public BarComponent P1EnergyBarComponent
        {
            get { return _p1EnergyBarComponent; }
        }

        public BarComponent P2EnergyBarComponent
        {
            get { return _p2EnergyBarComponent; }
        }

        public BarComponent P3EnergyBarComponent
        {
            get { return _p3EnergyBarComponent; }
        }

        public BarComponent P4EnergyBarComponent
        {
            get { return _p4EnergyBarComponent; }
        }

        public Image P1Portrait
        {
            get { return _p1Portrait; }
        }

        public Image P2Portrait
        {
            get { return _p2Portrait; }
        }

        public Image P3Portrait
        {
            get { return _p3Portrait; }
        }

        public Image P4Portrait
        {
            get { return _p4Portrait; }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            GameEvent _gameEvent;
            BossEvent _bossEvent;

            if (!ValidatePlayersHuds())
            {
                Debug.Log("One or more of the players' huds are null");
                enabled = false;
                return;
            }

            if (!ValidatePlayersPortraits())
            {
                Debug.Log("One or more of the players' portraits are null");
                enabled = false;
                return;
            }

            if (!ValidatePlayerLivesHUD())
            {
                Debug.Log("One or more of the players' lives are null");
                enabled = false;
                return;
            }

            if (!ValidatePlayerLives())
            {
                Debug.Log("One or more of the players' lives are null");
                enabled = false;
                return;
            }

            SetupBarComponents();

            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _gameEvent = GameManager.Instance.GameEvent;
                    _gameEvent.CanvasPlayers = this;
                    break;

                case "Boss":
                    _bossEvent = GameManager.Instance.BossEvent;
                    _bossEvent.CanvasPlayers = this;
                    break;
            }
        }

        private bool ValidatePlayersPortraits()
        {
            return _p1Portrait && _p2Portrait && _p3Portrait && _p4Portrait; 
        }

        private bool ValidatePlayersHuds()
        {
            return _p1Hud && _p2Hud && _p3Hud && _p4Hud;
        }

        private bool ValidatePlayerLivesHUD()
        {
            return _p1LivesHUD && _p2LivesHUD && _p3LivesHUD && _p4LivesHUD;
        }

        private bool ValidatePlayerLives()
        {
            return _p1Lives.Any() && _p2Lives.Any() && _p3Lives.Any() && _p4Lives.Any(); 
        }

        private void SetupBarComponents()
        {
            //Health Bar
            _p1HealthBarComponent = _p1Hud.transform.GetChild(3).GetComponent<BarComponent>();
            _p2HealthBarComponent = _p2Hud.transform.GetChild(3).GetComponent<BarComponent>();
            _p3HealthBarComponent = _p3Hud.transform.GetChild(3).GetComponent<BarComponent>();
            _p4HealthBarComponent = _p4Hud.transform.GetChild(3).GetComponent<BarComponent>();
            //Energy Bar
            _p1EnergyBarComponent = _p1Hud.transform.GetChild(0).GetComponent<BarComponent>();
            _p2EnergyBarComponent = _p2Hud.transform.GetChild(0).GetComponent<BarComponent>();
            _p3EnergyBarComponent = _p3Hud.transform.GetChild(0).GetComponent<BarComponent>();
            _p4EnergyBarComponent = _p4Hud.transform.GetChild(0).GetComponent<BarComponent>();
        }

        private void OnEnable()
        {
            SetupBarComponents();
        }

        #endregion

        #region Public Methods

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }

        public void EnableHud(int player)
        {
            switch (player)
            {
                case 1:
                    _p1Hud.SetActive(true);
                    break;

                case 2:
                    _p2Hud.SetActive(true);
                    break;

                case 3:
                    _p3Hud.SetActive(true);
                    break;

                case 4:
                    _p4Hud.SetActive(true);
                    break;

                default:
                    return;
            }
        }

        public void DisableHud(int player)
        {
            switch (player)
            {
                case 1:
                    _p1Hud.SetActive(false);
                    break;

                case 2:
                    _p2Hud.SetActive(false);
                    break;

                case 3:
                    _p3Hud.SetActive(false);
                    break;

                case 4:
                    _p4Hud.SetActive(false);
                    break;

                default:
                    return;
            }
        }

        public void DisableAllHuds()
        {
            _p1Hud.SetActive(false);
            _p2Hud.SetActive(false);
            _p3Hud.SetActive(false);
            _p4Hud.SetActive(false);
        }

        public BarComponent GetHealthBarComponentForPlayer(int player)
        {
            switch (player)
            {
                case 1:
                    return _p1HealthBarComponent;

                case 2:
                    return _p2HealthBarComponent;

                case 3:
                    return _p3HealthBarComponent;

                case 4:
                    return _p4HealthBarComponent;

                default:
                    return null;
            }
        }

        public BarComponent GetEnergyBarComponentForPlayer(int player)
        {
            switch (player)
            {
                case 1:
                    return _p1EnergyBarComponent;

                case 2:
                    return _p2EnergyBarComponent;

                case 3:
                    return _p3EnergyBarComponent;

                case 4:
                    return _p4EnergyBarComponent;

                default:
                    return null;
            }
        }

        public Image GetPortraitForPlayer(int player)
        {
            switch (player)
            {
                case 1:
                    return _p1Portrait;

                case 2:
                    return _p2Portrait;

                case 3:
                    return _p3Portrait;

                case 4:
                    return _p4Portrait;

                default:
                    return null;
            }
        }

        public List<Image> GetLivesForPlayer(int player)
        {
            switch (player)
            {
                case 1:
                    return _p1Lives;
                case 2:
                    return _p2Lives;
                case 3:
                    return _p3Lives;
                case 4:
                    return _p4Lives;
                default:
                    return null;
            }
        }

        public void DisableAllLives()
        {
            _p1LivesHUD.SetActive(false);
            _p2LivesHUD.SetActive(false);
            _p3LivesHUD.SetActive(false);
            _p4LivesHUD.SetActive(false);
        }

        public void EnableAllLives()
        {
            _p1LivesHUD.SetActive(true);
            _p2LivesHUD.SetActive(true);
            _p3LivesHUD.SetActive(true);
            _p4LivesHUD.SetActive(true);
        }

        public void DisableLivesForPlayer(int player)
        {
            switch(player)
            {
                case 1:
                    _p1LivesHUD.SetActive(false);
                    break;
                case 2:
                    _p2LivesHUD.SetActive(false);
                    break;
                case 3:
                    _p3LivesHUD.SetActive(false);
                    break;
                case 4:
                    _p4LivesHUD.SetActive(false);
                    break;
                default:
                    return;
            }
        }

        public void EnableLivesForPlayer(int player)
        {
            switch (player)
            {
                case 1:
                    _p1LivesHUD.SetActive(true);
                    break;
                case 2:
                    _p2LivesHUD.SetActive(true);
                    break;
                case 3:
                    _p3LivesHUD.SetActive(true);
                    break;
                case 4:
                    _p4LivesHUD.SetActive(true);
                    break;
                default:
                    return;
            }
        }

        public void ChangePortrait(int player, Sprite sprite)
        {
            switch (player)
            {
                case 1:
                    _p1Portrait.sprite = sprite;
                    break;

                case 2:
                    _p2Portrait.sprite = sprite;
                    break;

                case 3:
                    _p3Portrait.sprite = sprite;
                    break;

                case 4:
                    _p4Portrait.sprite = sprite;
                    break;

                default:
                    return;
            }
        }
        #endregion

    }
}
