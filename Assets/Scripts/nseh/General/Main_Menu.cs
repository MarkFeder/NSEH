using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using nseh.Utils;

namespace nseh.General
{
    public class Main_Menu : MonoBehaviour
    {
        public GameObject current;

        public void OneNumberCharacter(GameObject newCanvas)
        {
            Game.thisGame.Find<MenuManager>().ChangePlayers(1);
            ChangeCanvas(newCanvas);
        }

        public void TwoNumberCharacter(GameObject newCanvas)
        {
            Game.thisGame.Find<MenuManager>().ChangePlayers(2);
            ChangeCanvas(newCanvas);
        }

        public void ChangeCanvas(GameObject newCanvas)
        {
            current.SetActive(false);
            current = newCanvas;
            current.SetActive(true);

        }

      

        public void SaveChanges()
        {

        }

        public void PlayGame(GameObject newCanvas)
        {
            //ChangeCanvas(newCanvas);
            Game.thisGame.Find<MenuManager>().ChangeStates();

        }
        
        public void Exit()
        {
            Game.thisGame.Find<MenuManager>().ExitGame();
      
        }
    }

}
