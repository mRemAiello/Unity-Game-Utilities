using UnityEngine;

namespace GameUtils
{
    public static class GameObjectExtension
    {
        public static T GetComponentForced<T>(this GameObject gameObject) where T : Component
        {
            T obj = gameObject.GetComponent<T>() ?? gameObject.GetComponentInChildren<T>();
            return obj;
        }
    }
}