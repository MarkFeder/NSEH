using System;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;
using UnityEngine;

namespace nseh.Utils.EditorCustomization
{
    //[UnityEditor.CustomEditor(typeof(MonoBehaviour), true)]
    public class CustomEditor : MonoBehaviour
    {
        #region Private Properties
/*
        private string[] _hiddenProperties;

        #endregion

        #region Protected Methods

        protected virtual void OnEnable()
        {
            Type tp = target.GetType();
            HidePropertiesInInspector[] arr = tp.GetCustomAttributes(typeof(HidePropertiesInInspector), true) as HidePropertiesInInspector[];

            if (arr != null && arr.Length > 0)
            {
                var set = new HashSet<string>();
                foreach (var attribute in arr)
                {
                    foreach (var property in attribute.HiddenProperties)
                    {
                        set.Add(property);
                    }
                }
                _hiddenProperties = new string[set.Count];
                set.CopyTo(_hiddenProperties);
            }
        }

        #endregion

        #region Public Methods

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        public new bool DrawDefaultInspector()
        {
            //draw properties
            serializedObject.Update();
            var result = DrawDefaultInspectorExcept(serializedObject, _hiddenProperties);
            serializedObject.ApplyModifiedProperties();

            return result;
        }

        public static bool DrawDefaultInspectorExcept(SerializedObject serializedObject, params string[] propsNotToDraw)
        {
            if (serializedObject == null)
            {
                throw new System.ArgumentNullException("serializedObject");
            }

            EditorGUI.BeginChangeCheck();

            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                if (propsNotToDraw == null || !propsNotToDraw.Contains(iterator.name))
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }

            return EditorGUI.EndChangeCheck();
        }
*/
        #endregion
    }
}
