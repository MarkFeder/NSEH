using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using nseh.Gameplay.Gameflow;
using nseh.Gameplay.Movement;
using Constants = nseh.Utils.Constants.Animations.Movement;
using Inputs = nseh.Utils.Constants.Input;
using Layers = nseh.Utils.Constants.Layers;
using nseh.Gameplay.Entities.Player;
using Tags = nseh.Utils.Constants.Tags;


namespace nseh.Gameplay.Movement
{

    public class TeleportPoint : MonoBehaviour
    {
        #region Private Properties
        [SerializeField]
        private List<GameObject> TeleportPoints;
        private GameObject _sprite;
        #endregion

        #region Public Properties
        public AudioClip audio;
        #endregion

        #region Private Methods
        private void OnTriggerEnter(Collider other)
        {
            _sprite.SetActive(true);
        }

        private void OnTriggerStay(Collider other)
        {

            if ((other.CompareTag(Tags.PLAYER_BODY) && Input.GetButtonDown(String.Format("{0}{1}", Inputs.INTERACT, other.GetComponent<PlayerInfo>().GamepadIndex)) && other.GetComponent<PlayerInfo>().Teletransported == false))
            {
                other.GetComponent<PlayerInfo>().Teletransported = true;
                StartCoroutine(Teleport(other));
                AudioSource.PlayClipAtPoint(audio, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 1);
            }
        }

        private void OnTriggerExit(Collider other)
        {

            _sprite.SetActive(false);
            if ((other.CompareTag(Tags.PLAYER_BODY) && other.GetComponent<PlayerInfo>().Vertical == 0))
            {
                Debug.Log("Exit");
            }
        }

        private IEnumerator Teleport(Collider other)
        {
           
            int randomTeleportPoint = UnityEngine.Random.Range(0, TeleportPoints.Count);
            other.transform.position = new Vector3(TeleportPoints[randomTeleportPoint].transform.position.x, TeleportPoints[randomTeleportPoint].transform.position.y, other.transform.position.z);
            yield return new WaitForSeconds(1f);
            other.GetComponent<PlayerInfo>().Teletransported = false;
        }
        #endregion

        #region Private Methods
        public void Start()
        {
            _sprite = transform.GetChild(0).gameObject;
        }
        #endregion
    }
}