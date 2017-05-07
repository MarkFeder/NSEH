// Put usings here
using UnityEngine;

// Put namespace here
namespace nseh.Utils.Helpers
{
    // Put class/es here
    public class TemplateClass : MonoBehaviour
    {
        // Always put regions to divide class code (PRIVATE, PROTECTED, PUBLIC METHODS/PROPERTIES/BLABLA...)
        #region Private Properties

        // Put private properties following this style
        // SerializeField should be put if you want to visualize it on Unity's inspector
        [SerializeField]
        private int _privateVar;

        #endregion

        #region Public Properties

        // If you need more space, it's ok.
        public int PrivateVar
        {
            get { return _privateVar; }
            set { _privateVar = value; }
        }

        #endregion

        // Comments should be done this way
        /* Or this way ... */

        #region Public Methods
        
        // Put always method specifier 
        // (public, public virtual ..., protected, private, etc.)
        public void Start()
        {
        }

        public void Update()
        {
        }

        // Always put the methods with the first uppercase letter
        public int GetPrivateVar()
        {
            return _privateVar;
        }

        /// <summary>
        /// This is a special comment for the method that you can see
        /// when you hover over it with your mouse
        /// </summary>
        /// <returns></returns>
        public int GetPrivVar()
        {
            return _privateVar;
        }

        #endregion

        #region Private Methods

        private int GetPrivvVar()
        {
            return _privateVar;
        }

        #endregion

        #region Protected Methods

        #endregion
    }
}
