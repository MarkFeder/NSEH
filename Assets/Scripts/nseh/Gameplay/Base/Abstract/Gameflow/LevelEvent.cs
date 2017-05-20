using nseh.Managers.Level;

namespace nseh.Gameplay.Base.Abstract.Gameflow
{
    public abstract class LevelEvent
    {
		#region Protected Properties

		protected LevelManager _levelManager;
		protected bool _isActivated;

        #endregion

        #region Public Properties

        public bool IsActivated
        {
            get { return _isActivated; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup the sublevel manager providing the current levelManager instance. 
        /// The submanager is not active here yet.
        /// </summary>
        /// <param name="levelManager">The levelManager this LevelEvent is attached to.</param>
        public virtual void Setup(LevelManager levelManager)
		{
			_levelManager = levelManager;
		}

        #endregion

		#region Abstract Methods

		/// <summary>
		/// Activate the submanager execution.
		/// </summary>
		public abstract void ActivateEvent();

		/// <summary>
		/// Submanager execution.
		/// </summary>
		public abstract void EventTick();

		/// <summary>
		/// Deactivates the submanager.
		/// </summary>
		public abstract void EventRelease();

		#endregion
    }
}