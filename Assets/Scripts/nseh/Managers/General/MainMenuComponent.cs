using nseh.Managers;
using nseh.Managers.Main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace nseh.Managers.General
{
    public class MainMenuComponent : MonoBehaviour
    {
        public GameObject current;
        MenuManager _MenuManager;
        GameObject _wrarr;
        GameObject _demon;
        GameObject _prospector;
        int adding;
        public Text _playerTurnText;
        public Button play;
        public Button wrarr;
        public Button prospector;
        public Button granhilda;
        public Button musho;
        public Button anthea;
        public Button harley;
        public Button myson;
        public Button random;
        public EventSystem eventSystem;
        public GameObject selectedGameObject;

        void Start()
        {
            _MenuManager = GameManager.Instance.Find<MenuManager>();
            adding = 0;
            _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
        }

        public void OneNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(1);
            ChangeCanvas(newCanvas);
        }

        public void TwoNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(2);
            ChangeCanvas(newCanvas);
        }

        public void FourNumberCharacter(GameObject newCanvas)
        {
            _MenuManager.ChangePlayers(4);
            ChangeCanvas(newCanvas);
        }

        public void ChangeCanvas(GameObject newCanvas)
        {
            if (current.name == "Canvas_PickingCharacters")
            {
                adding = 0;
                _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
                wrarr.interactable = true;
                prospector.interactable = true;
                granhilda.interactable = true;
                musho.interactable = true;
                anthea.interactable = true;
                harley.interactable = true;
                myson.interactable = true;
                random.interactable = true;
                play.interactable = false;
            }
            current.SetActive(false);
            current = newCanvas;
            current.SetActive(true);


        }

        public void AddingWrarr()
        {
            _wrarr = Resources.Load("Wrarr") as GameObject;
            _MenuManager.Adding(_wrarr);
            adding++;

            _MenuManager.SetPlayerChoice("Wrarr", adding);

            if (adding == GameManager.Instance._numberPlayers)
            {
                _playerTurnText.text = "READY?";
                wrarr.interactable = false;
                prospector.interactable = false;
                granhilda.interactable = false;
                musho.interactable = false;
                anthea.interactable = false;
                harley.interactable = false;
                myson.interactable = false;
                random.interactable = false;
                play.interactable = true;
                eventSystem.SetSelectedGameObject(selectedGameObject);
                /*PLEASE DONT REMOVE
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + 1;
                */
            }
            else
            {
                _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
                /*PLEASE DONT REMOVE
               eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_"+(adding+1).ToString();
               eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + (adding + 1).ToString();
               eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + (adding + 1).ToString();
               */
            }
        }

      

        public void AddingProspector()
        {
            _prospector = Resources.Load("SirProspector") as GameObject;
            _MenuManager.Adding(_prospector);
            adding++;

            _MenuManager.SetPlayerChoice("SirProspector", adding);

            if (adding == GameManager.Instance._numberPlayers)
            {
                _playerTurnText.text = "READY?";
                wrarr.interactable = false;
                prospector.interactable = false;
                granhilda.interactable = false;
                musho.interactable = false;
                anthea.interactable = false;
                harley.interactable = false;
                myson.interactable = false;
                random.interactable = false;
                play.interactable = true;
                eventSystem.SetSelectedGameObject(selectedGameObject);
                /*PLEASE DONT REMOVE
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + 1;
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + 1;
                */
            }
            else
            {
                _playerTurnText.text = "PLAYER " + (adding + 1).ToString() + " TURN !";
                /*PLEASE DONT REMOVE
                eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_" + (adding + 1).ToString();
                eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_" + (adding + 1).ToString();
                eventSystem.GetComponent<StandaloneInputModule>().submitButton = "Jump_" + (adding + 1).ToString();
                */
            }
        }


        public void SaveChanges()
        {

        }

        public void Update()
        {

        }

        public void PlayGame(GameObject newCanvas)
        {
            //ChangeCanvas(newCanvas);
            _MenuManager.ChangeStates();
        }

        public void Exit()
        {
            _MenuManager.ExitGame();
        }
    } 
}


