using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(-10000)]
    [DeclareBoxGroup("Debug")]
    public abstract class Singleton<T> : MonoBehaviour, ILoggable where T : Singleton<T>
    {
        private static T _instance;
        [SerializeField, Group("Debug"), PropertyOrder(99)] private bool _logEnabled = false;

        //
        public bool LogEnabled => _logEnabled;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance;
            }

            protected set => _instance = value;
        }
        public static bool InstanceExists => Instance != null;

        //
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;
            OnPostAwake();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
                OnPostDestroy();
            }
        }

        protected virtual void OnPostAwake() { }
        protected virtual void OnPostDestroy() { }
    }
}
