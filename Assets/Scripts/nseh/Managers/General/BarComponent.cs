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
            if (fillAmount != content.fillAmount)
            {
                content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
            }

            if (lerpColors)
            {
                content.color = Color.Lerp(lowColor, fullColor, fillAmount);
            }
        }

        private float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        #endregion

    }
}
