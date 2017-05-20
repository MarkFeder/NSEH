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

		/// <summary>
		/// Setup the sublevel manager providing the current levelManager instance. 
		/// The submanager is not active here yet.
		/// </summary>
		/// <param name="myGame">The GameManager this Service is attached to.</param>
		public virtual void Setup(GameManager myGame)
		{
			_myGame = myGame;
		}

		#endregion

		#region Abstract Methods

		/// <summary>
		/// Activate the service.
		/// </summary>
		public abstract void Activate();

		/// <summary>
		/// Service execution.
		/// </summary>
		public abstract void Tick();

		/// <summary>
		/// Deactivates the service.
		/// </summary>
		public abstract void Release();

		#endregion
	}
}
