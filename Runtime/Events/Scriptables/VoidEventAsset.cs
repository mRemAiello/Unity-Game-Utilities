using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Void", order = 99)]
    public class VoidEventAsset : GameEventAssetBase
    {
        // private
        protected UnityEvent _onInvoked;

        public void AddListener(UnityAction call)
        {
            if (call == null)
                return;

            // 
            MutableListeners.Add(new EventTuple(call.Target.ToString(), call.Method.Name));
            _onInvoked.AddListener(call);
        }

        public void RemoveListener(UnityAction call)
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

        [Button(ButtonSizes.Medium)]
        public void Invoke()
        {
            this.Log($"{name} event invoked", this);
            _onInvoked?.Invoke();
        }
    }
}