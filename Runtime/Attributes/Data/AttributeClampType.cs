using System;

namespace GameUtils
{
    [Serializable]
    public enum AttributeClampType
    {
        RawFloat = 0,
        Round = 1,
        Floor = 2,
        Ceiling = 3,
        Integer = 4
    }
}