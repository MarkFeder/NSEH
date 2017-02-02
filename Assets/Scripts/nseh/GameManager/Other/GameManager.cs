using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using nseh.Utils;

//namespace nseh.GameManager.Other
//{
//    public class GameManager : MonoBehaviour
//    {
//        #region Initialization

//        private static GameManager _instance;
//        public static GameManager Instance
//        {
//            get
//            {
//                if (_instance != null)
//                {
//                    return _instance;
//                }

//                return null;
//            }
//        }

//        private void Awake()
//        {
//            if (_instance != null && _instance != this)
//            {
//                Destroy(this.gameObject);
//            }
//            else
//            {
//                _instance = this;
//                DontDestroyOnLoad(this.gameObject);
//            }
//        }

//        #endregion

//        #region Properties

//        public float startDelay = 0.3f;
//        public float endDelay = 0.3f;

//        public CameraControl cameraControl;
//        public Text messageText;
//        public GameObject[] objects;
//        public CharacterManager[] fighters;

//        private WaitForSeconds startWait;
//        private WaitForSeconds endWait;

//        private CharacterManager roundWinner;
//        private CharacterManager gameWinner;

//        #endregion

//        private void Start()
//        {
//            // Create the delays
//            startWait = new WaitForSeconds(startDelay);
//            endWait = new WaitForSeconds(endDelay);

//            // Spawn objects on the scene
//            this.SpawnAllCharacters();
//            this.SpawnEnemies();

//            // Set camera targets
//            //this.SetCameraTargets();
//            //this.SetCamera();

//            StartCoroutine(GameLoop());
//        }

//        private void SpawnAllCharacters()
//        {
//            for (int i = 0; i < this.fighters.Length; i++)
//            {
//                // TODO
//                //this.fighters[i].Instance = Instantiate();
//            }
//        }

//        private void SpawnEnemies()
//        {
//            // TODO
//        }

//        private void SetCameraTargets()
//        {
//            // Create a collection of transforms the same size as the number of elements at screen
//            Transform[] targets = new Transform[this.objects.Length];

//            for (int i = 0; i < targets.Length; i++)
//            {
//                // ... set it to the appropriate character transform.
//                targets[i] = this.fighters[i].Instance.transform;
//            }

//            // These are the targets the camera should follow.
//            this.cameraControl.targets = targets;
//        }

//        private IEnumerator GameLoop()
//        {
//            // This is called from start and will run each phrase of the game after another
            
//            // Run each routine in order

//            yield return StartCoroutine(this.RoundStarting());

//            yield return StartCoroutine(this.RoundPlaying());

//            yield return StartCoroutine(this.RoundEnding());

//            if (this.gameWinner != null)
//            {
//                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//            }else
//            {
//                StartCoroutine(GameLoop());
//            }
//        }

//        private IEnumerator RoundStarting()
//        {
//            // As soon as the round starts, reset the characters and other stuff and make sure they can't move
//            this.ResetAllCharacters();
//            this.ResetAllOtherStuff();
//            this.DisableCharactersControl();

//            // Snap the camera's zoom and position to something appropiate for the reset characters
//            this.cameraControl.SetStartPositionAndSize();

//            // Display text to the players
//            this.messageText.text = Constants.GameManager.ON_START_GAME;

//            // Wait for the specified 
//            yield return startWait;
//        }

//        private IEnumerator RoundPlaying()
//        {
//            // As soon as the round begins, let the players control their characters
//            // EnableCharactersControl();

//            this.messageText.text = String.Empty;

//            // While there is no character left ...
//            while (!this.CharacterLeft())
//            {
//                // ... return on the next frame
//                yield return null;
//            }
//        }

//        private IEnumerator RoundEnding()
//        {
//            this.DisableCharactersControl();

//            this.roundWinner = null;
//            this.roundWinner = this.GetRoundWinner();

//            if (this.roundWinner != null)
//            {
//                this.roundWinner.nWins++;
//            }

//            this.gameWinner = this.GetGameWinner();

//            string message = this.EndMessage();
//            this.messageText.text = message;

//            yield return endWait;
//        }

//        #region Characters Logic

//        private void DisableCharactersControl()
//        {
//            for (int i = 0; i < this.fighters.Length; i++)
//            {
//                this.fighters[i].DisableControl();
//            }
//        }

//        private void ResetAllOtherStuff()
//        {
//            throw new NotImplementedException();
//        }

//        private void ResetAllCharacters()
//        {
//            for (int i = 0; i < this.fighters.Length; i++)
//            {
//                this.fighters[i].Reset();
//            }
//        }

//        private bool CharacterLeft()
//        {
//            throw new NotImplementedException();
//        }

//        #endregion


//        #region Rounds Logic

//        private CharacterManager GetGameWinner()
//        {
//            throw new NotImplementedException();
//        }

//        private CharacterManager GetRoundWinner()
//        {
//            throw new NotImplementedException();
//        }

//        private string EndMessage()
//        {
//            throw new NotImplementedException();
//        }

//        #endregion
//    }
//}
