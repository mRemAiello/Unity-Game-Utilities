using UnityEngine;
using UnityEngine.Events;
using VInspector;
using ReadOnlyAttribute = VInspector.ReadOnlyAttribute;

namespace GameUtils
{
    public abstract class GameEventAsset<T> : GameEventBaseAsset
    {
        [SerializeField] private bool _log = false;
        [SerializeField, ReadOnly] private T _currentValue;

        // private readonly
        private UnityEvent<T> _onInvoked;

        // public
        public T CurrentValue => _currentValue;

        public void AddListener(UnityAction<T> call)
        {
            if (call == null)
            {
                return;
            }

            // 
            Listeners.Add(new EventTuple(call.Target.ToString(), call.Method.Name));

            //
            _onInvoked.AddListener(call);
        }

        public void RemoveListener(UnityAction<T> call)
        {
            foreach (var listener in Listeners)
            {
                if (listener.Item1.Equals(call.Target.ToString()) && listener.Item2.Equals(call.Method.Name))
                {
                    Listeners.Remove(listener);
                    break;
                }
            }
            
            //
            _onInvoked.RemoveListener(call);
        }

        public void RemoveAllListeners()
        {
            Listeners.Clear();
            _onInvoked.RemoveAllListeners();
        }

        [Button]
        public void Invoke(T param)
        {
            if (_log)
            {
                Debug.Log($"{name} event invoked: {param}", this);
            }              
            
            //
            _currentValue = param;
            _onInvoked?.Invoke(param);
        }
    }
}