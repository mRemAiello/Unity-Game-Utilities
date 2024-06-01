using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent _event;
        [SerializeField] private UnityEvent _response;

        // Getters
        public GameEvent Event => _event;
        public UnityEvent Response => _response;

        private void OnEnable()
        {
            _event.RegisterListener(this);
        }

        private void OnDisable()
        {
            _event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Debug.Log($"{name} raise {_event.name}");

            //
            _response.Invoke();
        }
    }
}