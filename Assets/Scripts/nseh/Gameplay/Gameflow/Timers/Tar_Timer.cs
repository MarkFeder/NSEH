using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using nseh.GameManager;

namespace nseh.Gameflow
{
    public class Tar_Timer : MonoBehaviour
    {
        //public GameObject tar;
        public Transform platformTarget;
        public Vector3 initialTarPosition;
        public Vector3 platformPosition;
        public Vector3 targetTarPosition;
        //private Vector3 velocity = new Vector3(1f,1f,1f);
        //float GameTime = 0;
        //int eventStartedAt = 0;
        float eventDuration = 10.0f;
        bool eventFinished = false;
        float elapsedTime;
        bool goingUp = true;
        

        // Use this for initialization
        void Start()
        {
            platformPosition = platformTarget.position;
            initialTarPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            /*
            GameTime += Time.deltaTime;
            //Controls when the tar should go up
            Debug.Log("estoy actualizando: " + Mathf.FloorToInt(GameTime)+ " ");
            if((Mathf.FloorToInt(GameTime) != 0 && Mathf.FloorToInt(GameTime) % 5 == 0) && !eventStarted)
            {
                Debug.Log("Empieza el evento");
                eventStartedAt = Mathf.FloorToInt(GameTime);
                eventStarted = true;
                Debug.Log("EventStarted: " + eventStarted);
                TarUp();
            }
            //Controls the time the tar should stay up and then turns it back to its initial position
            if ((Mathf.FloorToInt(GameTime) - eventStartedAt == eventDuration) && eventStarted)
            {
                if (eventDuration != 45)
                {
                    eventDuration += 5;
                }
                TarDown();
                eventStarted = false;
            }
            */
            elapsedTime += Time.deltaTime;
            //Controls when the tar should go up
            if (elapsedTime >= 5.0f && goingUp)
            {
                TarUp();
            }
            //Controls when the tar should go down
            else if (elapsedTime >= (5.0f + eventDuration) && !goingUp)
            {
                TarDown();
            }
            //Controls when the event cycle is completed and resets the involved variables
            else if(eventFinished)
            {
                if (eventDuration != 45.0f)
                {
                    eventDuration += 5.0f;
                }
                goingUp = !goingUp;
                eventFinished = !eventFinished;
                Debug.Log("Variables are reset and tar will remain up next time: " + eventDuration + " seconds.");
            }

        }

        void TarUp()
        {
            //tar.SetActive(true);
            targetTarPosition = new Vector3(transform.position.x, platformPosition.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetTarPosition, elapsedTime/80.0f);
            if(transform.position == targetTarPosition)
            {
                goingUp = !goingUp;
                Debug.Log("Tar is up (" + elapsedTime + ")");
            }
            //transform.position = Vector3.SmoothDamp(transform.position, targetTarPosition, ref velocity, 0.15f);
        }

        void TarDown()
        {
            targetTarPosition = new Vector3(transform.position.x, platformPosition.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, initialTarPosition, elapsedTime/120.0f);
            if(transform.position == initialTarPosition)
            {
                eventFinished = !eventFinished;
                Debug.Log("Tar is down (" + elapsedTime + ")");
                elapsedTime = 0;
            }
            //transform.position = Vector3.SmoothDamp(transform.position, initialTarPosition, ref velocity, 0.15f);
            //tar.SetActive(false);
        }
    }
}

