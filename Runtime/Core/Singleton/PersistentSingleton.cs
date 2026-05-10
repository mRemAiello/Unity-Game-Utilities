using UnityEngine;

namespace GameUtils
{
    // Ensures persistent singleton-based MonoBehaviours are prioritized at startup.
    [DefaultExecutionOrder(-10000)]
    public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected new void Awake()
        {
            base.Awake();

            // 
            DontDestroyOnLoad(gameObject);
        }
    }
}