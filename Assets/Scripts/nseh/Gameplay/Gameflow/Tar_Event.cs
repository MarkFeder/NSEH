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

        private float eventDuration = Constants.Events.Tar_Event.EVENT_DURATION;
        public TarComponent lava;
        private bool _lavaUp;

        //List<EventComponent> _tarComponents;
        //bool eventFinished = false;

        #endregion

        #region Public Properties

        public float elapsedTime;
        //Event components should suscribe their movement functions here to be handled by event
        //Resets all event component positions

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
            _lavaUp = false;
            //ResetTarComponents();
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION;
            elapsedTime = 0;
            
            //_alarmSound = GameManager.Instance.GameSounds.GetVolcanoLavaSound();
            //_bubbleSound = GameManager.Instance.GameSounds.GetRandomBubbleSound();

            //GameManager.Instance.SoundManager.PlayAudio(_bubbleSound, true);
        }

        //Event execution.
        override public void EventTick()
        {
            elapsedTime += Time.deltaTime;
            //Controls when the tar should go up
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

        //Deactivates the event
        override public void EventRelease()
        {
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION;
            elapsedTime = 0;
            lava.ResetLava();
            _isActivated = false;
            _lavaUp = false;
        }
       

        #endregion
    } 
}
