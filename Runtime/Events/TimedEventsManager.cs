using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class TimedEventsManager : Singleton<TimedEventsManager>
    {
        [SerializeField] private float _intervalInSeconds = 5f;
        [SerializeField] private VoidEventAsset _onPeriodicTask;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private float _timeElapsed = 0;

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            _timeElapsed = 0;
        }

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