using UnityEngine;
using System.Collections;

namespace NSEH
{
    public class GameManager_GoToMenuScene : MonoBehaviour
    {
        private GameManager_Master gameManagerMaster;
      
        void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.GoToMenuSceneEvent+= GoToMenuScene;
        }

        void OnDisable()
        {
            gameManagerMaster.GoToMenuSceneEvent -= GoToMenuScene;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void GoToMenuScene()
        {
            Debug.LogWarning("sadasdsa");
            Application.LoadLevel(0);

        }
    }


}

