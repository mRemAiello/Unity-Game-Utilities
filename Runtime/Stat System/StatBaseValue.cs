using System;

namespace GameUtils
{
    [Serializable]
    public class StatBaseValue : ElementTuple<StatData, int>
    {
        public StatBaseValue(StatData firstData, int secondData) : base(firstData, secondData)
        {
        }
    }
}