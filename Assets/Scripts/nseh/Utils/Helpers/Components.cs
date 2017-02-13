using System;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Utils.Helpers
{
    public static class Components
    {
        public static T GetComponentInChildWithTag<T>(this GameObject parent, string tag) 
            where T : Component
        {
            Transform t = parent.transform;

            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
            }

            return null;
        }

        public static T GetSafeComponent<T>(this GameObject obj) 
        {
            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError("Expected to find component of type " + typeof(T) + " but found none", obj);
            }

            return component;
        }

        public static T GetSafeComponentInChildren<T>(this GameObject obj)
        {
            T component = obj.GetComponentInChildren<T>();

            if (component == null)
            {
                Debug.LogError("Expected to find in children a component of type " + typeof(T) + " but found none", obj);
            }

            return component;
        }

        public static T[] GetSafeComponentsInChildren<T>(this GameObject obj)
        {
            T[] components = obj.GetComponentsInChildren<T>();

            if (components == null || components.Length == 0)
            {
                Debug.LogError("Expected to find in children some components of type " + typeof(T) + " but found none", obj);
            }

            return components;
        }
    }

    public static class Animators
    {
        public static bool AnimatorIsPlaying(this Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public static bool AnimatorIsPlaying(this Animator animator, string stateName)
        {
            return animator.AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }
    }

    public static class LINQExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(T item in enumeration)
            {
                action(item);
            }
        }
    }
}
