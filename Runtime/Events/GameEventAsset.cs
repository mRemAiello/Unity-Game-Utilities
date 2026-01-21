using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("log", Title = "Log")]
    public abstract class GameEventAsset<T> : ScriptableObject, ILoggable
    {
        [SerializeField, Group("log")] private bool _logEnabled = false;
        [SerializeField, Group("debug"), ReadOnly] private T _currentValue;
        [SerializeField, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] protected List<EventTuple> _runtimeListeners = new();

        //
        protected Action<T> _onInvoked;

        //
        public bool LogEnabled => _logEnabled;
        public T CurrentValue => _currentValue;

        //
        public void AddListener(Action<T> action)
        {
            if (action == null)
                return;

            //
            _runtimeListeners.Add(new EventTuple
            {
                Caller = action.Target != null ? action.Target.ToString() : "Static",
                MethodName = action.Method.Name,
                ClassName = action.Method.DeclaringType?.Name
            });

            //
            _onInvoked += action;
        }

        public void RemoveListener(Action<T> action)
        {
            // Deletes the listener and its reference from the runtime listeners list.
            _runtimeListeners.RemoveAll(tuple => tuple.Caller == action.Target?.ToString() && tuple.MethodName == action.Method.Name);

            //
            _onInvoked -= action;
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveAllListeners()
        {
            _runtimeListeners.Clear();
            _onInvoked = null;
        }

        public virtual void Invoke(T param)
        {
            // Gestisce l'invocazione dell'evento con tracciamento opzionale.
            this.Log($"[GameEventAsset<{typeof(T).Name}>] Invoked with param: {param}", this);

            //
            _currentValue = param;
            _onInvoked?.Invoke(param);
        }
    }
}
