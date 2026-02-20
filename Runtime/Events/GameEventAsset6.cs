using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class GameEventAsset<T1, T2, T3, T4, T5, T6> : GameEventAssetBase
    {
        [NonSerialized] protected List<EventTuple> _runtimeListeners = new();
        [NonSerialized] protected List<(T1, T2, T3, T4, T5, T6)> _callHistory;
        [NonSerialized] protected T1 _currentValue;
        [NonSerialized] protected T2 _currentValue2;
        [NonSerialized] protected T3 _currentValue3;
        [NonSerialized] protected T4 _currentValue4;
        [NonSerialized] protected T5 _currentValue5;
        [NonSerialized] protected T6 _currentValue6;
        protected Action<T1, T2, T3, T4, T5, T6> _onInvoked;

        [ShowInInspector, Group("debug"), ReadOnly] public T1 CurrentValue => _currentValue;
        [ShowInInspector, Group("debug"), ReadOnly] public T2 CurrentValue2 => _currentValue2;
        [ShowInInspector, Group("debug"), ReadOnly] public T3 CurrentValue3 => _currentValue3;
        [ShowInInspector, Group("debug"), ReadOnly] public T4 CurrentValue4 => _currentValue4;
        [ShowInInspector, Group("debug"), ReadOnly] public T5 CurrentValue5 => _currentValue5;
        [ShowInInspector, Group("debug"), ReadOnly] public T6 CurrentValue6 => _currentValue6;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<EventTuple> RuntimeListeners => _runtimeListeners;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<(T1, T2, T3, T4, T5, T6)> CallHistory => _callHistory;

        [Button(ButtonSizes.Medium)]
        public override void ResetData()
        {
            // Resets runtime-only debug data.
            _runtimeListeners = new List<EventTuple>();
            _callHistory = new List<(T1, T2, T3, T4, T5, T6)>();
        }

        public void AddListener(MonoBehaviour caller, Action<T1, T2, T3, T4, T5, T6> action)
        {
            if (action == null)
            {
                this.LogWarning($"Attempted to add a null listener.", this);
                return;
            }

            //
            _runtimeListeners.Add(new EventTuple
            {
                CallerGameObject = caller.gameObject,
                CallerScript = caller,
                MethodName = action.Method.Name
            });

            //
            _onInvoked += action;
        }

        public void RemoveListener(Action<T1, T2, T3, T4, T5, T6> action)
        {
            // Removes the callback from the invocation chain.
            _onInvoked -= action;
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveAllListeners()
        {
            // Clears debug state and runtime callback references.
            _runtimeListeners.Clear();
            _onInvoked = null;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void Invoke(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
        {
            // Tracks invocation details for runtime diagnostics.
            this.Log($"Invoked with params: {param1}, {param2}, {param3}, {param4}, {param5}, {param6}", this);

            //
            _callHistory ??= new List<(T1, T2, T3, T4, T5, T6)>();
            _callHistory.Add((param1, param2, param3, param4, param5, param6));
            _currentValue = param1;
            _currentValue2 = param2;
            _currentValue3 = param3;
            _currentValue4 = param4;
            _currentValue5 = param5;
            _currentValue6 = param6;
            _onInvoked?.Invoke(param1, param2, param3, param4, param5, param6);
        }
    }
}
