using UnityEngine;

namespace GameUtils
{
    public abstract class NumericGameConstant : GameConstant
    {
        public abstract int IntValue { get; }
        public abstract float FloatValue { get; }
    }
}