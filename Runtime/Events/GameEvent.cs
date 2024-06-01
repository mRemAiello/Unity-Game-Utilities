using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "Game Utils/Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> _eventListeners = new();

        // Getters
        public List<GameEventListener> Events => _eventListeners;

        public void Raise()
        {
            Debug.Log($"Event {name} raised");

            //
            foreach (GameEventListener listener in _eventListeners)
            {
                listener.OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!_eventListeners.Contains(listener))
            {
                _eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (_eventListeners.Contains(listener))
            {
                _eventListeners.Remove(listener);
            }
        }
    }
}