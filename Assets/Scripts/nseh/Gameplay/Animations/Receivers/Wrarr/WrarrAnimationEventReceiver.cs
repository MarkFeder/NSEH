using nseh.Gameplay.Base.Abstract.Animations;
using nseh.Utils.Helpers;
using UnityEngine;

namespace nseh.Gameplay.Animations.Receivers.Wrarr
{
    public class WrarrAnimationEventReceiver : AnimationEventReceiver
    {
        #region Private Properties

        private AnimationEventDelegate _onStartRoarCallback;
        private AnimationEventDelegate _onEndRoarCallback;
        private AnimationEventDelegate _onStartLaunchRockCallback;
        private AnimationEventDelegate _onStopLaunchRockCallback;

        private const string errorMessage = "AnimationEventReceiver callback delegate has not been set for animation event"; 

        #endregion

        #region Public Properties

        public AnimationEventDelegate OnStartRoarCallback
        {
            get { return _onStartRoarCallback; }
            set { _onStartRoarCallback = value; }
        }

        public AnimationEventDelegate OnEndRoarCallback
        {
            get { return _onEndRoarCallback; }
            set { _onEndRoarCallback = value; }
        }

        public AnimationEventDelegate OnStartLaunchRockCallback
        {
            get { return _onStartLaunchRockCallback; }
            set { _onStartLaunchRockCallback = value; }
        }

        public AnimationEventDelegate OnStopLaunchRockCallback
        {
            get { return _onStopLaunchRockCallback; }
            set { _onStopLaunchRockCallback = value; }
        }

        #endregion

        #region Private Methods

        private void OnStartRoar(AnimationEvent animationEvent)
        {
            if (_onStartRoarCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onStartRoarCallback(animationEvent);
        }

        private void OnEndRoar(AnimationEvent animationEvent)
        {
            if (_onEndRoarCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onEndRoarCallback(animationEvent);
        }

        private void OnStartLaunchRock(AnimationEvent animationEvent)
        {
            if (_onStartLaunchRockCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onStartLaunchRockCallback(animationEvent);
        }

        private void OnStopLaunchRock(AnimationEvent animationEvent)
        {
            if (_onStopLaunchRockCallback == null)
            {
                Debug.Log(errorMessage);
                return;
            }

            _onStopLaunchRockCallback(animationEvent);
        }

        #endregion
    }
}
