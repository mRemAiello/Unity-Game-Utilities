using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public abstract class Command : ScriptableObject
    {
        public abstract void Execute();
    }
}