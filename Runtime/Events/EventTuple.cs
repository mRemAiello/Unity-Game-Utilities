using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class EventTuple
    {
        public GameObject Caller;
        public string ClassName;
        public string MethodName;
    }
}