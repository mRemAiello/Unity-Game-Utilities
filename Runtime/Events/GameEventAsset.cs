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
            MutableListeners.Add(new EventTuple(call.Target.ToString(), call.Method.Name));
            _onInvoked.AddListener(call);
        }

        public void RemoveListener(UnityAction<T> call)
        {
            foreach (var listener in Listeners)
            {
                if (listener.EventData.Equals(call.Target.ToString()) && listener.EventName.Equals(call.Method.Name))
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

        public void Invoke(T param)
        {
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