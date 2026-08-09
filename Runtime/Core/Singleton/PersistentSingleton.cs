using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(-10000)]
    public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            // 
            DontDestroyOnLoad(gameObject);
        }
    }
}