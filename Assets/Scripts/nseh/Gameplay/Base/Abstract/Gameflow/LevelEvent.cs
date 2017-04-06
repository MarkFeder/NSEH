using nseh.Managers.Level;

namespace nseh.Gameplay.Base.Abstract.Gameflow
{
    public abstract class LevelEvent
    {
        //public GameManager MyGame;
        public LevelManager LvlManager;
        public bool IsActivated;

        //Setup the event providing the current game instance. The event is not active here yet.
        public virtual void Setup(/*GameManager myGame, */LevelManager lvlManager)
        {
            //MyGame = myGame;
            LvlManager = lvlManager;
        }

        //Activate the event execution.
        public abstract void ActivateEvent();

        //Event execution.
        public abstract void EventTick();

        //Deactivates the event
        public abstract void EventRelease();
    }

}