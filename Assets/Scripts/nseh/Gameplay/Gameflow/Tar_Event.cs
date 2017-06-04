using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Audio;
using nseh.Managers.Level;
using nseh.Managers.Main;
using nseh.Utils;
using UnityEngine;

namespace nseh.Gameplay.Gameflow
{
    public class Tar_Event : LevelEvent
    {
        #region Private Properties

        private bool isUp = false;
        private float eventDuration = Constants.Events.Tar_Event.EVENT_DURATION_MIN;

        private bool _playedAlarmSound;
        private AudioController _alarmSound;
        private AudioController _bubbleSound;

        //List<EventComponent> _tarComponents;
        //bool eventFinished = false;

        #endregion

        #region Public Properties

        public float elapsedTime;
        //Event components should suscribe their movement functions here to be handled by event
        public delegate bool TarHandler(float gameTime);
        public static event TarHandler TarUp;
        public static event TarHandler TarDown;

        //Resets all event component positions
        public delegate void TarReset();
        public static event TarReset ResetTarComponents;

        #endregion

        #region Public Methods

        //Setup the event providing the current game instance. The event is not active here yet.
        override public void Setup(LevelManager lvlManager)
		{
			base.Setup(lvlManager);
			//_tarComponents = new List<EventComponent>();
		}

        //Activate the event execution.
        override public void ActivateEvent()
        {
            _isActivated = true;

            ResetTarComponents();
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION_MIN;
            elapsedTime = 0;
            isUp = false;

            // Handle sounds
            _playedAlarmSound = false;
            _alarmSound = GameManager.Instance.GameSounds.GetVolcanoLavaSound();
            _bubbleSound = GameManager.Instance.GameSounds.GetRandomBubbleSound();

            GameManager.Instance.SoundManager.PlayAudio(_bubbleSound, true);
        }

        //Event execution.
        override public void EventTick()
        {
            elapsedTime += Time.deltaTime;
            //Controls when the tar should go up
            if (elapsedTime >= Constants.Events.Tar_Event.EVENT_START && elapsedTime < (Constants.Events.Tar_Event.EVENT_START + eventDuration) && !isUp)
            {
                //foreach(EventComponent tarComponent in _tarComponents)
                //{
                isUp = TarUp(elapsedTime);

                if (!_playedAlarmSound)
                {
                    GameManager.Instance.SoundManager.PlayAudio(_alarmSound, false);
                    _playedAlarmSound = true;
                }

                //}
            }
            //Controls when the tar should go down
            else if (elapsedTime >= (Constants.Events.Tar_Event.EVENT_START + eventDuration) && isUp)
            {
                isUp = TarDown(elapsedTime);
            }
            //Controls when the event cycle is completed and resets the involved variables
            else if (elapsedTime >= (Constants.Events.Tar_Event.EVENT_START + eventDuration) && !isUp)
            {
                if (eventDuration != Constants.Events.Tar_Event.EVENT_DURATION_MAX)
                {
                    eventDuration += Constants.Events.Tar_Event.EVENT_DURATION_INCREASE;
                }

                elapsedTime = 0;
                _playedAlarmSound = false;

                //Debug.Log("Variables are reset and tar will remain up next time: " + eventDuration + " seconds.");
            }
            //LvlManager.ChangeState(LevelManager.States.LevelEvent);
        }

        //Deactivates the event
        override public void EventRelease()
        {
            ResetTarComponents();
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION_MIN;
            elapsedTime = 0;
            isUp = false;

            _playedAlarmSound = false;
            GameManager.Instance.SoundManager.StopAudio(_bubbleSound);
            GameManager.Instance.SoundManager.StopAudio(_alarmSound);

            _isActivated = false;
        }
        /*
            public void RegisterLight(MonoBehaviour componentToRegister)
            {
                _tarComponents.Add(componentToRegister);
            }
        */

        #endregion
    } 
}
