using System;

namespace nseh.Utils.EditorCustomization
{
    public class HidePropertiesInInspector : Attribute
    {
        #region Private Properties

        private string[] _hiddenProperties;

        #endregion

        #region Public Properties

        public string[] HiddenProperties
        {
            get { return _hiddenProperties; }
        }

        #endregion

        public HidePropertiesInInspector(params string[] properties)
        {
            _hiddenProperties = properties;
        }
    }
}
