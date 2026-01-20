using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    public abstract class GameEventAsset<T> : GameEventAssetBase
    {
        [SerializeField, Group("debug"), PropertyOrder(1), ReadOnly] private T _currentValue;

        // private readonly
        protected UnityEvent<T> _onInvoked;

        // public
        public T CurrentValue => _currentValue;

        //
        public void AddListener(UnityAction<T> call)
        {
            if (call == null)
            {
                return;
            }

            // 
            MutableListeners.Add(BuildListenerTuple(call.Target, call.Method.Name));
            _onInvoked.AddListener(call);
        }

        public void RemoveListener(UnityAction<T> call)
        {
            // Prepara il riferimento Unity per il confronto diretto del caller.
            var callerReference = call.Target as Object;

            foreach (var listener in Listeners)
            {
                if (listener.Caller == callerReference && listener.MethodName.Equals(call.Method.Name))
                {
                    MutableListeners.Remove(listener);
                    break;
                }
            }
            
            //
            _onInvoked.RemoveListener(call);
        }

        public void RemoveAllListeners()
        {
            MutableListeners.Clear();
            _onInvoked.RemoveAllListeners();
        }

        public virtual void Invoke(T param)
        {
            // Gestisce l'invocazione dell'evento con tracciamento opzionale.
            if (LogEnabled)
            {
                this.Log($"Event invoked: {param}");
            }              
            
            //
            _currentValue = param;
            _onInvoked?.Invoke(param);
        }
    }
}
