using System;

namespace GameUtils
{
    [Serializable]
    public class EventTuple
    {
        public string EventName;
        public string EventData;

        //
        public EventTuple(string firstData, string secondData)
        {
            EventName = firstData;
            EventData = secondData;
        }
    }
}