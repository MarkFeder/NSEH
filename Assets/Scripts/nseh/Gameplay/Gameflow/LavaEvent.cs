using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Utils;
using UnityEngine;
using nseh.Gameplay.Base.Interfaces;
using nseh.Managers.Main;

namespace nseh.Gameplay.Gameflow
{
    public class LavaEvent : MonoBehaviour, IEvent
    {

        #region Private Properties

        private float eventDuration;
        public  LavaGameComponent lava;
        private bool _lavaUp;
        private bool _isActivated;

        #endregion

        #region Public Properties

        public float elapsedTime;

        #endregion

        #region Public Methods

        public void Start()
        {
            _isActivated = false;
        }

        public void ActivateEvent()
        {
            _isActivated = true;
            _lavaUp = false;
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION;
            elapsedTime = 0;

        }

        public void Update()
        {
            if (_isActivated && !GameManager.Instance.isPaused)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= Constants.Events.Tar_Event.EVENT_START && !_lavaUp)
                {
                    _lavaUp = true;
                    lava.LavaMotion();

                }
                else if (elapsedTime >= Constants.Events.Tar_Event.EVENT_START + eventDuration)
                {
                    elapsedTime = 0;
                    _lavaUp = false;
                }
            }
        }

        public void EventRelease()
        {
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION;
            elapsedTime = 0;
            lava.ResetLava();
            _lavaUp = false;
            _isActivated = false;
        }

        #endregion

    } 
}
