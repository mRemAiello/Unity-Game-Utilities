using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class GameEventAsset<T> : GameEventAssetBase
    {
        [SerializeField, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] protected List<T> _callHistory;

        //
        protected T _currentValue;
        protected Action<T> _onInvoked;

        //
        public T CurrentValue => _currentValue;

        //
        public override void ResetData()
        {
            base.ResetData();

            //
            _callHistory = new List<T>();
        }

        public void AddListener(Action<T> action)
        {
            if (action == null)
                return;

            //
            _runtimeListeners.Add(new EventTuple
            {
                Caller = action.Target != null ? action.Target.ToString() : "Static",
                ClassName = action.Method.DeclaringType?.Name,
                MethodName = action.Method.Name
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
            _callHistory.Add(param);
            _currentValue = param;
            _onInvoked?.Invoke(param);
        }
    }
}