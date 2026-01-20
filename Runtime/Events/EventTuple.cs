using System;
using Object = UnityEngine.Object;

namespace GameUtils
{
    [Serializable]
    public class EventTuple
    {
        public Object Caller;
        public string CallerDisplay;
        public string MethodName;

        //
        public EventTuple(Object caller, string callerDisplay, string methodName)
        {
            // Memorizza il riferimento serializzabile al caller con un fallback testuale e il nome metodo.
            Caller = caller;
            CallerDisplay = callerDisplay;
            MethodName = methodName;
        }
    }
}