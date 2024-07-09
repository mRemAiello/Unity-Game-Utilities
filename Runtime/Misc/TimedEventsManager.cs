using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class TimedEventsManager : Singleton<TimedEventsManager>
    {
        [SerializeField] private float _intervalInSeconds = 5f;
        [SerializeField] private VoidGameEventAsset _onPeriodicTask;

        //
        [Tab("Debug")]
        [SerializeField, ReadOnly] private float _timeElapsed;

        void Update()
        {
            _timeElapsed += Time.unscaledDeltaTime;
            if (_timeElapsed >= _intervalInSeconds)
            {
                _timeElapsed = 0f;
                _onPeriodicTask?.Invoke();
            }
        }
    }
}