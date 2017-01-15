using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using nseh.Utils;

namespace nseh.General
{
    public class Main_Menu : MonoBehaviour
    {

        public void PlayGame()
        {
            SceneManager.LoadScene(Constants.Scenes.SCENE_01);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }

}
