using nseh.Managers.Level;
using UnityEngine;

namespace nseh.Gameplay.Base.Abstract.Gameflow
{
    public abstract class SubLevelManager
    {
        #region Protected Properties

        protected LevelManager _levelManager;
        protected bool _isActivated;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup the sublevel manager providing the current levelManager instance. 
        /// The submanager is not active here yet.
        /// </summary>
        /// <param name="levelManager">The levelManager this SubLevelManager is attached to</param>
        public virtual void Setup(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Activate the submanager execution.
        /// </summary>
        public abstract void ActivateSubManager();

        /// <summary>
        /// Submanager execution.
        /// </summary>
        public abstract void SubManagerTick();

        /// <summary>
        /// Deactivates the submanager.
        /// </summary>
        public abstract void ReleaseSubManager(); 

        #endregion
    }
}
