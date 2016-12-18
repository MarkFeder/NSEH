using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using nseh.GameManager;

namespace nseh.General
{
    public class TimesUp_Clock : MonoBehaviour
    {
        private float timeRemaining = 5;
        public Text timerText;
        private bool isPaused;

        // Use this for initialization
        void Start()
        { 
            
        }

        // Update is called once per frame
        void Update()
        {
            timeRemaining -= Time.deltaTime;
        }

        void OnGUI()
        {
            if (timeRemaining > 0 && timeRemaining > 10)
            {
                timerText.text = timeRemaining.ToString("f0");
            }

            else if (timeRemaining > 0 && timeRemaining < 10)
            {
                timerText.text = timeRemaining.ToString("f2");
            }
            else
            {
               
                    
                    timerText.text = "";
                    GetComponent<GameManager_Master>().CallEventTimesUp();
                    isPaused = false;
                   
             
            }
        }
    }
}
