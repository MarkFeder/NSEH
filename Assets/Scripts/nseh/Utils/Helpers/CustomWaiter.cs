using System;
using UnityEngine;

namespace nseh.Utils.Helpers
{
    public delegate bool Condition(float elapsedSeconds);

    public class ConditionalBehaviour : MonoBehaviour
    {
        #region Private Properties

        private float _sinceAlive;

        private Action _action;
        private Condition _condition;

        #endregion

        #region Public C# Properties

        public Action Action { get { return _action; } set { _action = value; } }

        public Condition Condition { get { return _condition; } set { _condition = value; } }

        #endregion

        private void Update()
        {
            _sinceAlive += Time.deltaTime;
            if (_condition(_sinceAlive))
            {
                if (_action != null)
                {
                    _action();
                }

                Destroy(gameObject);
                _action = null;
                _condition = null;
            }
        }
    }

    /// <summary>
    /// A custom waiter to call from non-monobehaviour classes.
    /// </summary>
    public static class Wait
    {
        #region Private Properties

        private const string _goName = "Waiter";

        #endregion

        #region Public Methods

        /// <summary>
        /// Wait until meets condition.
        /// </summary>
        /// <param name="condition">The condition itself.</param>
        public static void Until(Condition condition)
        {
            GameObject go = new GameObject(_goName);

            ConditionalBehaviour c = go.AddComponent<ConditionalBehaviour>();
            c.Condition = condition;
        }

        /// <summary>
        /// Wait until meets condition. Then, execute action.
        /// </summary>
        /// <param name="condition">The condition itself.</param>
        /// <param name="action">The action to be executed.</param>
        public static void Until(Condition condition, Action action)
        {
            GameObject go = new GameObject(_goName);

            ConditionalBehaviour c = go.AddComponent<ConditionalBehaviour>();
            c.Condition = condition;
            c.Action = action;
        }

        #endregion
    }
}
