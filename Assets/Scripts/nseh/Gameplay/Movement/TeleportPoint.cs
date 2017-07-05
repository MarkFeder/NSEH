using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Entities.Player;
using Tags = nseh.Utils.Constants.Tags;
using nseh.Managers.Main;


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

        private void Start()
        {
            _sprite = transform.GetChild(0).gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            _sprite.SetActive(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if ((other.CompareTag(Tags.PLAYER_BODY) && other.GetComponent<PlayerInfo>().InteractPressed && other.GetComponent<PlayerInfo>().Teletransported == false))
            {
                other.GetComponent<PlayerInfo>().Teletransported = true;
                StartCoroutine(Teleport(other));
                GameManager.Instance.SoundManager.PlayAudioFX(audio, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _sprite.SetActive(false);
        }

        private IEnumerator Teleport(Collider other)
        {
            int randomTeleportPoint = UnityEngine.Random.Range(0, TeleportPoints.Count);
            other.transform.position = new Vector3(TeleportPoints[randomTeleportPoint].transform.position.x, TeleportPoints[randomTeleportPoint].transform.position.y, other.transform.position.z);
            yield return new WaitForSeconds(1f);
            other.GetComponent<PlayerInfo>().Teletransported = false;
        }

        #endregion

    }
}