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

            // Registra il listener con riferimento al caller del target.
            MutableListeners.Add(BuildListenerTuple(call.Target));
            _onInvoked.AddListener(call);
        }

        public void RemoveListener(UnityAction call)
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

        [Button(ButtonSizes.Medium)]
        public void Invoke()
        {
            this.Log($"{name} event invoked", this);
            _onInvoked?.Invoke();
        }
    }
}
