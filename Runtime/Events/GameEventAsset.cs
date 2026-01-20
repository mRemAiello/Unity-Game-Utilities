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

            // Registra il listener con riferimento al caller del target.
            MutableListeners.Add(BuildListenerTuple(call.Target));
            _onInvoked.AddListener(call);
        }

        public void RemoveListener(UnityAction<T> call)
        {
            // Recupera i riferimenti del caller per individuare la tupla corretta.
            var callerGameObject = GetListenerGameObject(call.Target);
            var callerScriptable = GetListenerScriptableObject(call.Target);

            foreach (var listener in Listeners)
            {
                if (listener.CallerGameObject == callerGameObject && listener.CallerScriptable == callerScriptable)
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
