using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    [Serializable]
    public class TimedEventData
    {
        [Min(0f)]
        public float DelayInSeconds = 1f;
        public bool UseUnscaledTime = true;
        public UnityEvent OnElapsed;
    }
}
