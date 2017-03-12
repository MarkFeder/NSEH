using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Gameplay.Movement;
using Constants = nseh.Utils.Constants.Animations.Movement;
using Inputs = nseh.Utils.Constants.Input;
using Layers = nseh.Utils.Constants.Layers;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Movement
{

    public class TeleportPoint : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> TeleportPoints;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {

        }

        private void OnTriggerStay(Collider other)
        {

            if ((other.GetComponent<PlayerInfo>().Vertical > 0 && other.GetComponent<PlayerInfo>().Horizontal == 0 && other.GetComponent<PlayerInfo>().Teletransported == false))
            {
                other.GetComponent<PlayerInfo>().Teletransported = true;
                StartCoroutine(Teleport(other));

            }


        }

        private void OnTriggerExit(Collider other)
        {
            
            if ((other.GetComponent<PlayerInfo>().Vertical == 0))
            {
                Debug.Log("Exit");
                other.GetComponent<PlayerInfo>().Teletransported = false;

            }
        }


        IEnumerator Teleport(Collider other)
        {
            yield return new WaitForSeconds(0.05f);
            int randomTeleportPoint = UnityEngine.Random.Range(0, TeleportPoints.Count);
            other.transform.position = new Vector3(TeleportPoints[randomTeleportPoint].transform.position.x, TeleportPoints[randomTeleportPoint].transform.position.y, other.transform.position.z);
        }

    }
}