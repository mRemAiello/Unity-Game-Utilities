using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public abstract class ParametrizedGameEvent<T> : ScriptableObject
    {
        private readonly List<ParametrizedGameEventListener<T>> _eventListeners = new();

        public void Raise(T value)
        {
            foreach (ParametrizedGameEventListener<T> listener in _eventListeners)
            {
                listener.OnEventRaised(value);
            }
        }

        public void RegisterListener(ParametrizedGameEventListener<T> listener)
        {
            if (!_eventListeners.Contains(listener))
            {
                _eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(ParametrizedGameEventListener<T> listener)
        {
            if (_eventListeners.Contains(listener))
            {
                _eventListeners.Remove(listener);
            }
        }
    }
}