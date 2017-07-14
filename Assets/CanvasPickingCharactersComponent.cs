using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.UI
{ 
    public class CanvasPickingCharactersComponent : MonoBehaviour
    {

        #region Public Properties

        public MyButton Wrarr;
        public MyButton Prospector;
        public MyButton Granhilda;
        public MyButton Musho;
        public MyButton Anthea;
        public MyButton Harley;
        public MyButton Myson;
        public MyButton Random;
        public Text playerTurnText;
        public MyButton play;
        public GameObject selectedGameObject;
        public MainMenuComponent mainMenu;

        #endregion

        #region Private Methods

        private void OnEnable()
        {
            mainMenu.wrarr = Wrarr;
            mainMenu.prospector = Prospector;
            mainMenu.granhilda = Granhilda;
            mainMenu.musho = Musho;
            mainMenu.anthea = Anthea;
            mainMenu.harley = Harley;
            mainMenu.myson = Myson;
            mainMenu.random = Random;
            mainMenu.play = play;
            mainMenu.playerTurnText = playerTurnText;
            mainMenu.selectedGameObject = selectedGameObject;
        }

        #endregion

    }
}
