using UnityEngine;

namespace GameUtils
{
    // Ensures singleton-based MonoBehaviours initialize much earlier than standard scripts.
    [DefaultExecutionOrder(-10000)]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }
        public static bool InstanceExists => Instance != null;

        protected void Awake()
        {
            if (InstanceExists)
            {
                Destroy(gameObject);
            }
            else
            {
                // Registers the first valid instance before the rest of the scene startup flow.
                Instance = (T)this;
                OnPostAwake();
            }
        }

        protected void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                OnPostDestroy();
            }
        }

        protected virtual void OnPostAwake() { }
        protected virtual void OnPostDestroy() { }
    }
}
