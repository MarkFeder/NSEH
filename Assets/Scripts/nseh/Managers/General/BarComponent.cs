using UnityEngine;
using UnityEngine.UI;

namespace nseh.Managers.General
{
    public class BarComponent : MonoBehaviour
    {
        #region Private Properties

        private float fillAmount;
        [SerializeField]
        private float lerpSpeed;
        [SerializeField]
        private Image content;
        [SerializeField]
        private Color fullColor;
        [SerializeField]
        private Color lowColor;

		//Set this to "true" if you want color variation depending on the fill value of the bar
		[SerializeField]
        private bool lerpColors; 

        #endregion

        #region Public Properties

        public float MaxValue
        {
            get;
            set;
        }

        public float Value
        {
            set
            {
                fillAmount = Map(value, 0, MaxValue, 0, 1);
            }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            if (lerpColors)
            {
                content.color = fullColor;
            }
        }

        private void Update()
        {
            HandleBar();
        }

        private void HandleBar()
        {
            //Bar fill increases or decreases in a smooth way
            if (fillAmount != content.fillAmount)
            {
                content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
            }

            //Color variation depending on fill value of the bar
            if (lerpColors)
            {
                content.color = Color.Lerp(lowColor, fullColor, fillAmount);
            }
        }

        /// <summary>
        /// Map the specified value, inMin, inMax, outMin and outMax.
        /// </summary>
        /// <returns>The map.</returns>
        /// <param name="value">Current health/mana value.</param>
        /// <param name="inMin">Min health/mana value.</param>
        /// <param name="inMax">Max health/mana value.</param>
        /// <param name="outMin">Min fillAmount.</param>
        /// <param name="outMax">Max fillAmount.</param>
        private float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        #endregion
    }
}
