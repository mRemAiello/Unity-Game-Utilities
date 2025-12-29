using System;
using UnityEngine.Events;

namespace GameUtils
{
    internal class ScheduledEvent
    {
        public int ID;
        public float RemainingTime;
        public bool UseUnscaledTime;
        public Action Callback;
        public UnityEvent UnityEvent;
    }
}
