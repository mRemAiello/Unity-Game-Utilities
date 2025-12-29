using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(0)]
    public class TimedEventsManager : Singleton<TimedEventsManager>
    {
        [SerializeField] private List<TimedEventData> _initialEvents = new();

        [Tab("Debug")]
        [SerializeField, ReadOnly] private int _scheduledEventsCount;

        private readonly List<ScheduledEvent> _scheduledEvents = new();
        private int _nextEventId = 1;

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            _scheduledEvents.Clear();
            _scheduledEventsCount = 0;

            ScheduleInitialEvents();
        }

        public TimedEventHandle AddEvent(float delayInSeconds, Action callback, bool useUnscaledTime = true)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return AddScheduledEvent(delayInSeconds, useUnscaledTime, callback, null);
        }

        public bool RemoveEvent(TimedEventHandle handle)
        {
            for (int i = _scheduledEvents.Count - 1; i >= 0; i--)
            {
                if (_scheduledEvents[i].ID == handle.ID)
                {
                    _scheduledEvents.RemoveAt(i);
                    _scheduledEventsCount = _scheduledEvents.Count;
                    return true;
                }
            }

            return false;
        }

        private void ScheduleInitialEvents()
        {
            if (_initialEvents == null)
            {
                return;
            }

            foreach (var timedEvent in _initialEvents)
            {
                if (timedEvent == null)
                {
                    continue;
                }

                AddScheduledEvent(timedEvent.DelayInSeconds, timedEvent.UseUnscaledTime, null, timedEvent.OnElapsed);
            }
        }

        private TimedEventHandle AddScheduledEvent(float delayInSeconds, bool useUnscaledTime, Action callback, UnityEvent unityEvent)
        {
            var scheduledEvent = new ScheduledEvent
            {
                ID = _nextEventId++,
                RemainingTime = Mathf.Max(0f, delayInSeconds),
                UseUnscaledTime = useUnscaledTime,
                Callback = callback,
                UnityEvent = unityEvent
            };

            _scheduledEvents.Add(scheduledEvent);
            _scheduledEventsCount = _scheduledEvents.Count;

            return new TimedEventHandle(scheduledEvent.ID);
        }

        private void Update()
        {
            if (_scheduledEvents.Count == 0)
            {
                return;
            }

            for (int i = _scheduledEvents.Count - 1; i >= 0; i--)
            {
                var scheduledEvent = _scheduledEvents[i];
                float deltaTime = scheduledEvent.UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                scheduledEvent.RemainingTime -= deltaTime;

                if (scheduledEvent.RemainingTime > 0f)
                {
                    continue;
                }

                _scheduledEvents.RemoveAt(i);

                scheduledEvent.Callback?.Invoke();
                scheduledEvent.UnityEvent?.Invoke();
            }

            _scheduledEventsCount = _scheduledEvents.Count;
        }
    }
}
