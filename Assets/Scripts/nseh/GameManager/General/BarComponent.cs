using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.GameManager.General
{
    public class BarComponent : MonoBehaviour {

        private float fillAmount;
        [SerializeField]
        private float lerpSpeed;
        [SerializeField]
        private Image content;
        [SerializeField]
        private Color fullColor;
        [SerializeField]
        private Color lowColor;
        [SerializeField]
        private bool lerpColors; //Set this to "true" if you want color variation depending on the fill value of the bar
        public float MaxValue { get; set; }
        public float Value
        {
            set
            {
                fillAmount = Map(value, 0, MaxValue, 0, 1);
            }
        }
        // Use this for initialization
        void Start() {
            if (lerpColors)
            {
                content.color = fullColor;
            }
        }

        // Update is called once per frame
        void Update() {
            HandleBar();
        }

        private void HandleBar()
        {
            //Bar fill increases or decreases smoothy
            if (fillAmount != content.fillAmount)
            {
                //content.fillAmount = fillAmount;
                content.fillAmount = Mathf.Lerp(content.fillAmount,fillAmount,Time.deltaTime*lerpSpeed);
            }

            //Color variation depending on fill value of the bar
            if (lerpColors)
            {
                content.color = Color.Lerp(lowColor, fullColor, fillAmount);
            }
        }

        //value = current health/mana value; inMin = min health/mana value; inMax = max health/mana value; outMin = min fillAmount; outMax = max fillAmount;
        private float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
    }
}
