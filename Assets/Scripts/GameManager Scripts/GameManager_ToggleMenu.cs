using UnityEngine;
using System.Collections;

namespace NSEH
{
    public class GameManager_ToggleMenu : MonoBehaviour {

        private GameManager_Master gameManagerMaster;
        public GameObject menu;

	    // Use this for initialization
	    void Start ()
        {
            ToggleMenu();
	    }
	
	    // Update is called once per frame
	    void Update ()
        {
            CheckForMenuToggleRequest();
	    }

        void OnEnable()
        {
            SetInitialPreferences();
            gameManagerMaster.GameOverEvent += ToggleMenu;
        }

        void OnDisable()
        {
            gameManagerMaster.GameOverEvent -= ToggleMenu;
        }

        void SetInitialPreferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void CheckForMenuToggleRequest()
        {
            if(Input.GetKeyUp(KeyCode.Escape)&& !gameManagerMaster.isGameOver)
            {
                ToggleMenu();
            }
        }

        void ToggleMenu()
        {
            if (menu != null)
            {
                menu.SetActive(!menu.activeSelf);
                gameManagerMaster.isMenuOn = !gameManagerMaster.isMenuOn;
                gameManagerMaster.CallEventMenuToogle();
            }
            else
            {
                Debug.LogWarning("You need to assing UI GameObject to the Toggle Menu script in the inspector.");
            }
        }
    }
}


