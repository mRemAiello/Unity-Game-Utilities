using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class EventTuple
    {
        [SerializeField] private UnityEngine.Object _caller;
        public string CallerDisplay;
        public string MethodName;

        // Espone il riferimento Unity del caller memorizzato nella tupla.
        public UnityEngine.Object Caller => _caller;

        //
        public EventTuple(UnityEngine.Object caller, string callerDisplay, string methodName)
        {
            // Memorizza il riferimento serializzabile al caller con un fallback testuale e il nome metodo.
            _caller = caller;
            CallerDisplay = callerDisplay;
            MethodName = methodName;
        }
    }
}
