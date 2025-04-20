using System;

namespace GameUtils
{
    [Serializable]
    public class EventTuple : Tuple<string, string>
    {
        public EventTuple(string firstData, string secondData) : base(firstData, secondData)
        {
        }

        //
        public string EventName => Item1;
        public string EventData => Item2;
    }
}