using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Managers.Main;
using nseh.Managers.Level;

namespace nseh.Gameplay.Entities.Environment
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        #region Private Properties
        [SerializeField] //Just debug purposes. Don't change its value on Unity Inspector please.
        private bool isFree;
        #endregion

        #region Public C# Properties
        public bool IsFree
        {
            get
            {
                return this.isFree;
            }
        }
        #endregion

        void Start()
        {
            this.isFree = true;

            if (SceneManager.GetActiveScene().name == "Game")
            {
                Debug.Log("PlayerSpawnPoint registered");
                GameManager.Instance.Find<LevelManager>().RegisterPlayerSpawnPoint(this.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("PlayerBody"))
            {
                this.isFree = false;
                Debug.Log("Character inside spawn Point. Property IsFree = " + this.isFree);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PlayerBody"))
            {
                this.isFree = true;
                Debug.Log("Character has left the spawn Point. Property IsFree = " + this.isFree);
            }
        }

        #region Public Methods
        public void SetFree()
        {
            this.isFree = true;
        }
        #endregion

        
    }
}