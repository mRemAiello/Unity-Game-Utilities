using System;

namespace GameUtils
{
    [Serializable]
    public class AttributeIntTuple : ElementTuple<AttributeData, float>
    {
        public AttributeIntTuple(AttributeData firstData, float secondData) : base(firstData, secondData)
        {
        }

        //
        public AttributeData Data => Item1;
        public float Value => Item2;
    }
}