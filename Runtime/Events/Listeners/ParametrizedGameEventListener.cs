using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    public abstract class ParametrizedGameEventListener<T> : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private ParametrizedGameEvent<T> _event;
        [SerializeField] private UnityEvent<T> _response;

        private void OnEnable()
        {
            _event.RegisterListener(this);
        }

        private void OnDisable()
        {
            _event.UnregisterListener(this);
        }

        public void OnEventRaised(T value)
        {
            _response.Invoke(value);
        }

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}