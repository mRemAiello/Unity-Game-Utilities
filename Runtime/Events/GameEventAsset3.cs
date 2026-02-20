using System;
using System.Collections.Generic;
using TriInspector;

namespace GameUtils
{
    public abstract class GameEventAsset<T1, T2, T3> : GameEventAssetBase
    {
        [NonSerialized] protected List<EventTuple> _runtimeListeners = new();
        [NonSerialized] protected List<(T1, T2, T3)> _callHistory;
        [NonSerialized] protected T1 _currentValue;
        [NonSerialized] protected T2 _currentValue2;
        [NonSerialized] protected T3 _currentValue3;
        protected Action<T1, T2, T3> _onInvoked;

        [ShowInInspector, Group("debug"), ReadOnly] public T1 CurrentValue => _currentValue;
        [ShowInInspector, Group("debug"), ReadOnly] public T2 CurrentValue2 => _currentValue2;
        [ShowInInspector, Group("debug"), ReadOnly] public T3 CurrentValue3 => _currentValue3;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<EventTuple> RuntimeListeners => _runtimeListeners;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<(T1, T2, T3)> CallHistory => _callHistory;

        [Button(ButtonSizes.Medium)]
        public override void ResetData()
        {
            // Resets runtime-only debug data.
            _runtimeListeners = new List<EventTuple>();
            _callHistory = new List<(T1, T2, T3)>();
        }

        public void AddListener(Action<T1, T2, T3> action)
        {
            // Guards against invalid listener registration.
            if (action == null)
            {
                this.LogWarning("Attempted to add a null listener.", this);
                return;
            }

            _onInvoked += action;
        }

        public void RemoveListener(Action<T1, T2, T3> action)
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

        public virtual void Invoke(T1 param1, T2 param2, T3 param3)
        {
            // Tracks invocation details for runtime diagnostics.
            this.Log($"Invoked with params: {param1}, {param2}, {param3}", this);

            _callHistory ??= new List<(T1, T2, T3)>();
            _callHistory.Add((param1, param2, param3));
            _currentValue = param1;
            _currentValue2 = param2;
            _currentValue3 = param3;
            _onInvoked?.Invoke(param1, param2, param3);
        }
    }
}
