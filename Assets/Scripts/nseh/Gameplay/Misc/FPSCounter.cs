﻿using UnityEngine;
using UnityEngine.UI;

namespace nseh.Gameplay.Misc
{
    [RequireComponent(typeof(Text))]
    public class FPSCounter : MonoBehaviour
    {
        public float updateInterval = 0.5F;

        private float accum = 0; // FPS accumulated over the interval
        private int frames = 0; // Frames drawn over the interval
        private float timeleft; // Left time for current interval

        private Text guiText;

        private void Start()
        {
            guiText = GetComponent<Text>();

            if (!guiText)
            {
                Debug.Log("FPSCounter needs a Text component!");
                enabled = false;
                return;
            }

            timeleft = updateInterval;
        }

        private void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                float fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);
                guiText.text = format;

                if (fps < 30)
                {
                    guiText.color = Color.yellow;
                }
                else
                {
                    if (fps < 10)
                    {
                        guiText.color = Color.red;
                    }
                    else
                    {
                        guiText.color = Color.green;
                    }
                }
                
                //	DebugConsole.Log(format,level);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}
