using UnityEngine;
using System.Collections;

namespace NSEH
{
    public class GameManager_TimesUp : MonoBehaviour
    {
        private GameManager_Master gameManagerMaster;
        public GameObject panelTimesUp;
       
        void OnEnable()
        {
            SetInitialPreferences();
            gameManagerMaster.TimesUpEvent += TurnOnTimesUpPanel;
        }

        void OnDisable()
        {
            gameManagerMaster.TimesUpEvent -= TurnOnTimesUpPanel;
        }

        void SetInitialPreferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        } 

        void TurnOnTimesUpPanel()
        {
            if(panelTimesUp != null)
            {
                Time.timeScale = 0;
                panelTimesUp.SetActive(true);

            }
        }
    } 

}
