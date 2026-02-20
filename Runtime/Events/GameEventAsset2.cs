using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class GameEventAsset<T1, T2> : GameEventAssetBase
    {
        [NonSerialized] protected List<EventTuple> _runtimeListeners = new();
        [NonSerialized] protected List<(T1, T2)> _callHistory;
        [NonSerialized] protected T1 _currentValue;
        [NonSerialized] protected T2 _currentValue2;
        protected Action<T1, T2> _onInvoked;

        //
        [ShowInInspector, Group("debug"), ReadOnly] public T1 CurrentValue => _currentValue;
        [ShowInInspector, Group("debug"), ReadOnly] public T2 CurrentValue2 => _currentValue2;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<EventTuple> RuntimeListeners => _runtimeListeners;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<(T1, T2)> CallHistory => _callHistory;

        //
        [Button(ButtonSizes.Medium)]
        public override void ResetData()
        {
            _runtimeListeners = new List<EventTuple>();
            _callHistory = new List<(T1, T2)>();
        }

        public void AddListener(MonoBehaviour caller, Action<T1, T2> action)
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

        public void RemoveListener(MonoBehaviour caller, Action<T1, T2> action)
        {
            // Deletes the listener and its reference from the runtime listeners list.
            _runtimeListeners.RemoveAll(tuple => tuple.CallerScript == caller && tuple.MethodName == action.Method.Name);
            _onInvoked -= action;
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveAllListeners()
        {
            _runtimeListeners.Clear();
            _onInvoked = null;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void Invoke(T1 param1, T2 param2)
        {
            // Gestisce l'invocazione dell'evento con tracciamento opzionale.
            this.Log($"Invoked with params: {param1}, {param2}", this);

            //
            _callHistory ??= new List<(T1, T2)>();
            _callHistory.Add((param1, param2));
            _currentValue = param1;
            _currentValue2 = param2;
            _onInvoked?.Invoke(param1, param2);
        }
    }
}