namespace nseh.Managers
{
    public abstract class Service
    {
        #region Public Properties
        public nseh.Managers.Main.GameManager MyGame;
        public bool IsActivated;
        #endregion

        #region Virtual Methods
        //Setup the Service providing the current game instance. The Service is not active here yet.
        virtual public void Setup(nseh.Managers.Main.GameManager myGame)
        {
            MyGame = myGame;
        }
        #endregion

        #region Abstract Methods
        //Activate the Service execution.
        abstract public void Activate();

        //Service execution.
        abstract public void Tick();

        //Deactivates the Service
        abstract public void Release();
        #endregion
    }
}
