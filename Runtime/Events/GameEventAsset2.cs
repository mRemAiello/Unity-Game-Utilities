using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class GameEventAsset<T1, T2> : GameEventAssetBase
    {
        [NonSerialized] protected T1 _currentValue;
        [NonSerialized] protected T2 _currentValue2;
        [NonSerialized] protected List<(T1, T2)> _callHistory = new();
        protected Action<T1, T2> _onInvoked;

        //
        [ShowInInspector, Group("Debug"), ReadOnly] public T1 CurrentValue => _currentValue;
        [ShowInInspector, Group("Debug"), ReadOnly] public T2 CurrentValue2 => _currentValue2;
        [ShowInInspector, Group("Debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<(T1, T2)> CallHistory => _callHistory;

        //
        public void AddListener(MonoBehaviour caller, Action<T1, T2> action)
        {
            if (!TryRegisterListener(caller, action))
                return;

            _onInvoked += action;
        }

        public void RemoveListener(MonoBehaviour caller, Action<T1, T2> action)
        {
            if (!TryUnregisterListener(caller, action))
                return;

            _onInvoked -= action;
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveAllListeners()
        {
            ClearRuntimeListeners();

            _onInvoked = null;
        }

        [Button(ButtonSizes.Medium)]
        public override void ResetData()
        {
            ClearRuntimeListeners();

            _callHistory?.Clear();

            // 
            _currentValue = default;
            _currentValue2 = default;
            _onInvoked = null;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void Invoke(T1 param1, T2 param2)
        {
            this.Log($"{name} invoked with param: {param1}, {param2}", this);

            //
            _callHistory ??= new List<(T1, T2)>();
            _callHistory.Add((param1, param2));

            //
            _currentValue = param1;
            _currentValue2 = param2;
            _onInvoked?.Invoke(param1, param2);
        }
    }
}