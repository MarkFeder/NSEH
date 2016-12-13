using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace NSEH
{
    public class TimesUp_Clock : MonoBehaviour
    {
        private float timeRemaining = 15;
        public Text timerText;

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

            }
        }
    }
}
