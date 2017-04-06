namespace nseh.Managers
{
    public abstract class Service
    {
        public nseh.Managers.Main.GameManager MyGame;
        public bool IsActivated;

        //Setup the Service providing the current game instance. The Service is not active here yet.
        virtual public void Setup(nseh.Managers.Main.GameManager myGame)
        {
            MyGame = myGame;
        }

        //Activate the Service execution.
        abstract public void Activate();

        //Service execution.
        abstract public void Tick();

        //Deactivates the Service
        abstract public void Release();
    } 
}
