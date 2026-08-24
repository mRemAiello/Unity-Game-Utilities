using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Debug")]
    public abstract class GameEventAssetBase : ScriptableObject, ILoggable
    {
        [SerializeField, Group("Debug")] private bool _logEnabled;
        [NonSerialized] private readonly List<EventListenerInfo> _runtimeListeners = new();

        //
        public bool LogEnabled => _logEnabled;
        public IReadOnlyList<EventListenerInfo> RuntimeListeners => _runtimeListeners;
        public int ListenerCount => _runtimeListeners.Count;
        public bool HasListeners => _runtimeListeners.Count > 0;

        //
        protected bool TryRegisterListener(MonoBehaviour caller, Delegate action)
        {
            if (caller == null)
            {
                this.LogWarning("Attempted to register a listener with a null caller.", this);
                return false;
            }

            if (action == null)
            {
                this.LogWarning("Attempted to register a listener with a null action.", this);
                return false;
            }

            // Check if the listener is already registered
            _runtimeListeners.Add(new EventListenerInfo
            {
                CallerGameObject = caller.gameObject,
                CallerScript = caller,
                MethodName = action.Method.Name
            });
            return true;
        }

        protected bool TryUnregisterListener(MonoBehaviour caller, Delegate action)
        {
            if (caller == null)
            {
                this.LogWarning("Attempted to unregister a listener with a null caller.", this);
                return false;
            }

            if (action == null)
            {
                this.LogWarning("Attempted to unregister a listener with a null action.", this);
                return false;
            }

            // Remove all listeners that match the caller and method name
            _runtimeListeners.RemoveAll(x => x.CallerScript == caller && x.MethodName == action.Method.Name);
            return true;
        }

        protected void ClearRuntimeListeners() => _runtimeListeners.Clear();
        public abstract void ResetData();
    }
}