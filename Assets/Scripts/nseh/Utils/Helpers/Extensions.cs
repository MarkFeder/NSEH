using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace nseh.Utils.Helpers
{
    public static class ComponentExtensions
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

    public static class AnimatorExtensions
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

    public static class StringBuilderExtensions
    {
        public static void Clear(this StringBuilder value)
        {
            value.Length = 0;
            value.Capacity = 0;
        }
    }

    public static class ListExtensions
    {
        public static void AddNotDuplicate<T>(this List<T> list, T element)
            where T : class
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }
            else
            {
                Debug.Log("The element " + element.ToString() + " is already in the list");
            }
        }

        public static void PrintOnDebug(this List<GameObject> list)
        {
            StringBuilder toPrint = new StringBuilder("Current list: [");

            foreach(GameObject obj in list)
            {
                toPrint.Append(", ");
                toPrint.Append(obj.name);
            }

            toPrint.Append("]");

            Debug.Log(toPrint);
        }
    }

    public static class CoroutinesExtensions
    {
        public static IEnumerator WaitForAll(params Coroutine[] coroutines)
        {
            for (int i = 0; i < coroutines.Length; i++)
            {
                yield return coroutines[i];
            }
        }
    }

    public static class ParallelCoroutineExtensions
    {
        public static RunInfo ParallelCoroutine(this IEnumerator coroutine, string group = "default")
        {
            if (!RunInfo.runners.ContainsKey(group))
            {
                RunInfo.runners[group] = new RunInfo();
            }
            var ri = RunInfo.runners[group];
            ri.count++;

            ParallelSupport.Instance.StartCoroutine(DoParallel(coroutine, ri));
            return ri;
        }

        public static IEnumerator DoParallel(IEnumerator coroutine, RunInfo ri)
        {
            yield return ParallelSupport.Instance.StartCoroutine(coroutine);
            ri.count--;
        }
    }

    public static class AnimationEventExtensions
    {
        public static AnimationEvent CreateAnimationEvent(string functionName, 
                                                          float time, 
                                                          SendMessageOptions messageOptions)
        {
            AnimationEvent animEvent = new AnimationEvent();
            animEvent.functionName = functionName;
            animEvent.time = time;
            animEvent.messageOptions = messageOptions;

            return animEvent;
        }

        public static void CreateAnimationEventForClip(ref AnimationClip clip, 
                                                       string functionName, 
                                                       float time, 
                                                       SendMessageOptions messageOptions)
        {
            AnimationEvent animEvent = new AnimationEvent();
            animEvent.functionName = functionName;
            animEvent.time = time;
            animEvent.messageOptions = messageOptions;

            clip.AddEvent(animEvent);
        }
    }
}
