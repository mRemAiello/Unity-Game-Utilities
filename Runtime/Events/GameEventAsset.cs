using System;
using System.Collections.Generic;
using TriInspector;

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

        public void AddListener(Action<T> action)
        {
            if (action == null)
                return;

            //
            /*_runtimeListeners.Add(new EventTuple
            {
                Caller = ,
                ClassName = action.Method.DeclaringType?.Name,
                MethodName = action.Method.Name
            });*/

            //
            _onInvoked += action;
        }

        public void RemoveListener(Action<T> action)
        {
            // Deletes the listener and its reference from the runtime listeners list.
            //_runtimeListeners.RemoveAll(tuple => tuple.Caller == action.Target?.ToString() && tuple.MethodName == action.Method.Name);

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