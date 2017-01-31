using UnityEngine;
using System.Collections;
/*
namespace nseh.GameManager
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
           gameManagerMaster.TimesUpEvent += ToggleMenu;
        }

        void OnDisable()
        {
            gameManagerMaster.TimesUpEvent -= ToggleMenu;
        }

        void SetInitialPreferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void CheckForMenuToggleRequest()
        {
            if(Input.GetKeyUp(KeyCode.Escape) && !gameManagerMaster.isTimesUp)
            //if (Input.GetKeyUp(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        void ToggleMenu()
        {
            if (menu != null)
            {
                menu.SetActive(!menu.activeSelf);
                //gameManagerMaster.isMenuOn = !gameManagerMaster.isMenuOn;
                gameManagerMaster.CallEventMenuToogle();
            }
            else
            {
                Debug.LogWarning("You need to assing UI GameObject to the Toggle Menu script in the inspector.");
            }
        }
    }
}


    */