using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class GameEventAsset<T> : GameEventAssetBase
    {
        [NonSerialized] protected List<T> _callHistory = new();
        [NonSerialized] protected T _currentValue;
        protected Action<T> _onInvoked;

        //
        [ShowInInspector, Group("Debug"), ReadOnly] public T CurrentValue => _currentValue;
        [ShowInInspector, Group("Debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<T> CallHistory => _callHistory;

        //
        public void AddListener(MonoBehaviour caller, Action<T> action)
        {
            if (!TryRegisterListener(caller, action))
                return;

            _onInvoked += action;
        }

        public void RemoveListener(MonoBehaviour caller, Action<T> action)
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

            _currentValue = default;
            _onInvoked = null;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void Invoke(T param)
        {
            this.Log($"{name} invoked with param: {param}", this);

            // 
            _callHistory ??= new List<T>();

            // 
            _currentValue = param;
            _callHistory.Add(param);
            _onInvoked?.Invoke(param);
        }
    }
}