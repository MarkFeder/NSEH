using nseh.Managers.General;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

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
        public List<Image> _p1Lives;
        public List<Image> _p2Lives;
        public List<Image> _p3Lives;
        public List<Image> _p4Lives;

        #endregion

        #region Private Properties

        private BarComponent _p1BarComponent;
        private BarComponent _p2BarComponent;
        private BarComponent _p3BarComponent;
        private BarComponent _p4BarComponent;

        #endregion

        #region Public C# Properties

        public BarComponent P1BarComponent
        {
            get { return _p1BarComponent; }
        }

        public BarComponent P2BarComponent
        {
            get { return _p2BarComponent; }
        }

        public BarComponent P3BarComponent
        {
            get { return _p3BarComponent; }
        }

        public BarComponent P4BarComponent
        {
            get { return _p4BarComponent; }
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

        private void Start()
        {
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

            if(!ValidatePlayerLives())
            {
                Debug.Log("One or more of the players' lives are null");
                enabled = false;
                return;
            }

            SetupBarComponents();

            // DisableAllHuds();
        }

        #region Private Methods

        private bool ValidatePlayersPortraits()
        {
            return _p1Portrait && _p2Portrait; // includes p3Portrait and p4Portrait when available
        }

        private bool ValidatePlayersHuds()
        {
            return _p1Hud && _p2Hud; // includes p3Hud and p4Hud when available
        }

        private bool ValidatePlayerLives()
        {
            return _p1Lives.Any() && _p2Lives.Any(); //includes _p3Lives.Any() and _p4Lives.Any() when available
        }

        private void SetupBarComponents()
        {
            _p1BarComponent = _p1Hud.GetComponentInChildren<BarComponent>();
            _p2BarComponent = _p2Hud.GetComponentInChildren<BarComponent>();
            // p3BarComponent = p3Hud.GetComponentInChildren<BarComponent>();
            // p4BarComponent = p4Hud.GetComponentInChildren<BarComponent>();
        }

        private void OnEnable()
        {
            // Keep the reference again
            Debug.Log("OnEnable CanvasPlayersHUDManager");
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
            // DisableP3Hud();
            // DisableP4Hud();
        }

        public BarComponent GetBarComponentForPlayer(int player)
        {
            switch (player)
            {
                case 1:
                    return _p1BarComponent;

                case 2:
                    return _p2BarComponent;

                case 3:
                    return _p3BarComponent;

                case 4:
                    return _p4BarComponent;

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
        /*
        public void DisableLife(int player, int index)
        {
            switch (player)
            {
                case 1:
                    _p1Lives[index - 1].enabled = false;
                    break;
                case 2:
                    _p2Lives[index - 1].enabled = false;
                    break;
                case 3:
                    _p3Lives[index - 1].enabled = false;
                    break;
                case 4:
                    _p4Lives[index - 1].enabled = false;
                    break;
                default:
                    return;
            }
        }

        public void RestoreAllLives()
        {
            foreach(Image life in _p1Lives)
            {
                if(life.enabled == false)
                {
                    life.enabled = true;
                }
            }

            foreach (Image life in _p2Lives)
            {
                if (life.enabled == false)
                {
                    life.enabled = true;
                }
            }

            //include loops for _p3 and _p4 when available
        }
        */

        #endregion
    }
}
