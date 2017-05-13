using nseh.Utils.Helpers;
using UnityEngine;

namespace nseh.Gameplay.Animations.Receivers.SirProspector
{
    public class SirProspectorAnimationEventReceiver : MonoBehaviour
    {
        #region Private Properties

        private AnimationEventDelegate _onShowSwordCallback;
        private AnimationEventDelegate _onHideSwordCallback;

        private AnimationEventDelegate _onActivateItemCallback;
        private AnimationEventDelegate _onDeactivateItemCallback;

        private const string errorMessage = "AnimationEventReceiver callback delegate has not been set for animation event"; 

        #endregion

        #region Public Properties

        public AnimationEventDelegate OnShowSwordCallback
        {
            get { return _onShowSwordCallback; }
            set { _onShowSwordCallback = value; }
        }

        public AnimationEventDelegate OnHideSwordCallback
        {
            get { return _onHideSwordCallback; }
            set { _onHideSwordCallback = value; }
        }

        public AnimationEventDelegate OnActivateItemCallback
        {
            get { return _onActivateItemCallback; }
            set { _onActivateItemCallback = value; }
        }

        public AnimationEventDelegate OnDeactivateItemCallback
        {
            get { return _onDeactivateItemCallback; }
            set { _onDeactivateItemCallback = value; }
        }

        #endregion

        #region Public Methods

        public void OnHideSword(AnimationEvent animationEvent)
        {
            if (_onHideSwordCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onHideSwordCallback(animationEvent);
        }

        public void OnShowSword(AnimationEvent animationEvent)
        {
            if (_onShowSwordCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onShowSwordCallback(animationEvent);
        }

        public void OnActivateItem(AnimationEvent animationEvent)
        {
            if (_onActivateItemCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onActivateItemCallback(animationEvent);
        }

        public void OnDeactivateItem(AnimationEvent animationEvent)
        {
            if (_onDeactivateItemCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onDeactivateItemCallback(animationEvent);
        }

        #endregion
    }
}
