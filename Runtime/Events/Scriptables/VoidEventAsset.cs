using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Void", order = 99)]
    public class VoidEventAsset : GameEventBaseAsset
    {
        // private
        private UnityEvent _onInvoked;

        public void AddListener(UnityAction call)
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
            this.Log($"{name} event invoked", this);
            _onInvoked?.Invoke();
        }
    }
}