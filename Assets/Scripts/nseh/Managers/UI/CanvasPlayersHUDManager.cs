using nseh.Managers.General;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasPlayersHUDManager : MonoBehaviour
    {
        #region Public Properties

        public GameObject _p1Hud;
        public GameObject _p2Hud;
        public GameObject _p3Hud;
        public GameObject _p4Hud;

        public Image _p1Portrait;
        public Image _p2Portrait;
        public Image _p3Portrait;
        public Image _p4Portrait;

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

        public BarComponent GetBarComponentForPlayer(int player)
        {
            switch(player)
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
