using UnityEngine;
using UnityEngine.Events;
using VInspector;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "Events/Void")]
    public class VoidEventAsset : GameEventBaseAsset
    {
        [SerializeField] private bool _log = false;

        // private readonly
        private readonly UnityEvent _onInvoked;

        public void AddListener(UnityAction call)
        {
            if (call == null)
            {
                return;
            }

            // 
            var tuple = new EventTuple(call.Target.ToString(), call.Method.Name);
            Listeners.Add(tuple);

            //
            _onInvoked.AddListener(call);
        }

        public void RemoveListener(UnityAction call)
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
        public void Invoke()
        {
            if (_log)
            {
                Debug.Log($"{name} event invoked", this);
            }

            //
            _onInvoked?.Invoke();
        }
    }
}