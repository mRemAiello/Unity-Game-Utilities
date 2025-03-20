using System;
using UnityEngine;

namespace GameUtils
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class Required : PropertyAttribute
    {
        public WarningType WarningType { get; private set; }

        public Required(WarningType warningType = WarningType.Error)
        {
            WarningType = warningType;
        }
    }
}