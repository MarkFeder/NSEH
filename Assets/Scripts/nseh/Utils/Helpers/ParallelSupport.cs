using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Utils.Helpers
{
    public class RunInfo
    {
        #region Public Properties

        public int count;
        public static Dictionary<string, RunInfo> runners = new Dictionary<string, RunInfo>();

        #endregion
    }

    public class ParallelSupport : MonoBehaviour
    {
        #region Private Properties

        private const string _goName = "CoroutinesManager";
        private static ParallelSupport _instance;

        #endregion

        #region Public Properties

        public static ParallelSupport Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject(_goName);
                    go.hideFlags = go.hideFlags | HideFlags.HideAndDontSave;
                    _instance = go.AddComponent<ParallelSupport>();
                }

                return _instance;
            }
        }

        #endregion
    }
}
