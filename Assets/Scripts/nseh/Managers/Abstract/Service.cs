using nseh.Managers.Main;

namespace nseh.Managers
{
    public abstract class Service
    {

        #region Protected Properties

        protected GameManager _myGame;
        protected bool _isActivated;

        #endregion

        #region Public Properties

        public bool IsActivated
        {
            get { return _isActivated; }
        }

        public GameManager MyGame
        {
            get { return _myGame; }
        }

		#endregion

		#region Public Methods

		public virtual void Setup(GameManager myGame)
		{
			_myGame = myGame;
		}

		#endregion

		#region Abstract Methods

		public abstract void Activate();

		public abstract void Tick();

		public abstract void Release();

		#endregion

	}
}
