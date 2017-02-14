using UnityEngine;

namespace nseh.GameManager
{
    public class GameManager_GameOver : MonoBehaviour
    {
        private GameManager_Master gameManagerMaster;
        public GameObject panelGameOver;

        void OnEnable()
        {
            SetInitialPreferences();
            gameManagerMaster.GameOverEvent += TurnOnGameOverPanel;
        }

        void OnDisable()
        {
            gameManagerMaster.GameOverEvent -= TurnOnGameOverPanel;
        }

        void SetInitialPreferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        } 

        void TurnOnGameOverPanel()
        {
            if(panelGameOver != null)
            {
                panelGameOver.SetActive(true);
            }
        }
    }

}
