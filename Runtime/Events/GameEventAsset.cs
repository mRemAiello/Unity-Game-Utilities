using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class GameEventAsset<T> : GameEventAssetBase
    {
        [NonSerialized] protected List<EventTuple> _runtimeListeners = new();
        [NonSerialized] protected List<T> _callHistory;
        [NonSerialized] protected T _currentValue;
        protected Action<T> _onInvoked;

        //
        [ShowInInspector, Group("debug"), ReadOnly] public T CurrentValue => _currentValue;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<EventTuple> RuntimeListeners => _runtimeListeners;
        [ShowInInspector, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<T> CallHistory => _callHistory;

        //
        [Button(ButtonSizes.Medium)]
        public override void ResetData()
        {
            _runtimeListeners = new List<EventTuple>();
            _callHistory = new List<T>();
        }

        public void AddListener(MonoBehaviour caller, Action<T> action)
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

        public void RemoveListener(MonoBehaviour caller, Action<T> action)
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
        public virtual void Invoke(T param)
        {
            // Gestisce l'invocazione dell'evento con tracciamento opzionale.
            this.Log($"Invoked with param: {param}", this);

            // Inizializza la cronologia solo in runtime per evitare persistenza sull'asset.
            _callHistory ??= new List<T>();

            // Aggiunge il parametro alla cronologia di debug runtime.
            _callHistory.Add(param);
            _currentValue = param;
            _onInvoked?.Invoke(param);
        }
    }
}