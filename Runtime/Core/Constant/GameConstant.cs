using UnityEngine;

namespace GameUtils
{
    public abstract class GameConstant : ScriptableObject
    {
        public abstract string StringValue { get; }
    }
}