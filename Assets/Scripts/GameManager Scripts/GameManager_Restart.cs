using UnityEngine;
using System.Collections;

namespace NSEH
{
    public class GameManager_Restart : MonoBehaviour
    {
        
    private GameManager_Master gameManagerMaster;

    void OnEnable()
    {
        SetInitialReferences();
        gameManagerMaster.RestartEvent += Restart;
    }

    void OnDisable()
    {
            gameManagerMaster.RestartEvent -= Restart;
        }

    void SetInitialReferences()
    {
        gameManagerMaster = GetComponent<GameManager_Master>();
    }

    void Restart()
    {
        Application.LoadLevel(1);

    }
}


}



