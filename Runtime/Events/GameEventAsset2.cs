using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class GameEventAsset2<T1, T2> : GameEventAssetBase
    {
        [SerializeField, Group("internal"), ReadOnly] protected T1 _currentValue;
        [SerializeField, Group("internal"), ReadOnly] protected T2 _currentValue2;
        [SerializeField, Group("internal"), TableList(AlwaysExpanded = true), ReadOnly] protected List<(T1, T2)> _callHistory;

        //
        protected Action<T1, T2> _onInvoked;

        //
        public T1 CurrentValue => _currentValue;

        //
        public override void ResetData()
        {
            base.ResetData();

            //
            _callHistory = new List<(T1, T2)>();
        }

        public void AddListener(Action<T1, T2> action)
        {
            if (action == null)
            {
                this.LogWarning($"Attempted to add a null listener.", this);
                return;
            }

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

        public void RemoveListener(Action<T1, T2> action)
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

        public virtual void Invoke(T1 param1, T2 param2)
        {
            // Gestisce l'invocazione dell'evento con tracciamento opzionale.
            this.Log($"Invoked with params: {param1}, {param2}", this);

            //
            _callHistory ??= new List<(T1, T2)>();

            //
            _callHistory.Add((param1, param2));
            _currentValue = param1;
            _currentValue2 = param2;
            _onInvoked?.Invoke(param1, param2);
        }
    }
}