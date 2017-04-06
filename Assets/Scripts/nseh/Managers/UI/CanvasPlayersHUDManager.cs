using nseh.Managers.General;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{
    public class CanvasPlayersHUDManager : MonoBehaviour
    {
        #region Public Properties

        public GameObject p1Hud;
        public GameObject p2Hud;
        public GameObject p3Hud;
        public GameObject p4Hud;

        public Image p1Portrait;
        public Image p2Portrait;
        public Image p3Portrait;
        public Image p4Portrait;

        #endregion

        #region Private Properties

        private BarComponent p1BarComponent;
        private BarComponent p2BarComponent;
        private BarComponent p3BarComponent;
        private BarComponent p4BarComponent;

        #endregion

        #region Public C# Properties

        public BarComponent P1BarComponent
        {
            get { return p1BarComponent; }
        }

        public BarComponent P2BarComponent
        {
            get { return p2BarComponent; }
        }

        public BarComponent P3BarComponent
        {
            get { return p3BarComponent; }
        }

        public BarComponent P4BarComponent
        {
            get { return p4BarComponent; }
        }

        public Image P1Portrait
        {
            get { return p1Portrait; }
        }

        public Image P2Portrait
        {
            get { return p2Portrait; }
        }

        public Image P3Portrait
        {
            get { return p3Portrait; }
        }

        public Image P4Portrait
        {
            get { return p4Portrait; }
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

        private void Update()
        {
        }

        #region Private Methods

        private bool ValidatePlayersPortraits()
        {
            return p1Portrait && p2Portrait; // includes p3Portrait and p4Portrait when available
        }

        private bool ValidatePlayersHuds()
        {
            return p1Hud && p2Hud; // includes p3Hud and p4Hud when available
        }

        private void SetupBarComponents()
        {
            p1BarComponent = p1Hud.GetComponentInChildren<BarComponent>();
            p2BarComponent = p2Hud.GetComponentInChildren<BarComponent>();
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
                    return p1BarComponent;

                case 2:
                    return p2BarComponent;

                case 3:
                    return p3BarComponent;

                case 4:
                    return p4BarComponent;

                default:
                    return null;
            }
        }

        public Image GetPortraitForPlayer(int player)
        {
            switch (player)
            {
                case 1:
                    return p1Portrait;

                case 2:
                    return p2Portrait;

                case 3:
                    return p3Portrait;

                case 4:
                    return p4Portrait;

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
                    EnableP1Hud();
                    break;

                case 2:
                    EnableP2Hud();
                    break;

                case 3:
                    EnableP3Hud();
                    break;

                case 4:
                    EnableP4Hud();
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
                    DisableP1Hud();
                    break;

                case 2:
                    DisableP2Hud();
                    break;

                case 3:
                    DisableP3Hud();
                    break;

                case 4:
                    DisableP4Hud();
                    break;

                default:
                    return;
            }
        }

        public void DisableAllHuds()
        {
            DisableP1Hud();
            DisableP2Hud();
            // DisableP3Hud();
            // DisableP4Hud();
        }

        public void EnableP1Hud()
        {
            p1Hud.SetActive(true);
        }

        public void EnableP2Hud()
        {
            p2Hud.SetActive(true);
        }

        public void EnableP3Hud()
        {
            p3Hud.SetActive(true);
        }

        public void EnableP4Hud()
        {
            p4Hud.SetActive(true);
        }

        public void DisableP1Hud()
        {
            p1Hud.SetActive(false);
        }

        public void DisableP2Hud()
        {
            p2Hud.SetActive(false);
        }

        public void DisableP3Hud()
        {
            p3Hud.SetActive(false);
        }

        public void DisableP4Hud()
        {
            p4Hud.SetActive(false);
        }

        public void ChangePortrait(int player, Sprite sprite)
        {
            switch (player)
            {
                case 1:
                    ChangeP1PortraitSprite(sprite);
                    break;

                case 2:
                    ChangeP2PortraitSprite(sprite);
                    break;

                case 3:
                    ChangeP3PortraitSprite(sprite);
                    break;

                case 4:
                    ChangeP4PortraitSprite(sprite);
                    break;

                default:
                    return;
            }
        }

        public void ChangeP1PortraitSprite(Sprite sprite)
        {
            if (sprite)
            {
                p1Portrait.sprite = sprite;
            }
        }

        public void ChangeP2PortraitSprite(Sprite sprite)
        {
            if (sprite)
            {
                p2Portrait.sprite = sprite;
            }
        }

        public void ChangeP3PortraitSprite(Sprite sprite)
        {
            if (sprite)
            {
                p3Portrait.sprite = sprite;
            }
        }

        public void ChangeP4PortraitSprite(Sprite sprite)
        {
            if (sprite)
            {
                p4Portrait.sprite = sprite;
            }
        } 

        #endregion
    }
}
