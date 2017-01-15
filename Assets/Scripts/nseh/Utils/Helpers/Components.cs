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
            where T : MonoBehaviour
        {
            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError("Expected to find component of type " + typeof(T) + " but found none", obj);
            }

            return component;
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
}
