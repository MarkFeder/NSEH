using UnityEngine;
using System.Collections;

namespace NSEH
{
    public class Main_Menu : MonoBehaviour
    {

        public void PlayGame()
        {
            Application.LoadLevel(1);
        }

        public void Exit()
        {
            Application.Quit();
        }
        
    }

}
