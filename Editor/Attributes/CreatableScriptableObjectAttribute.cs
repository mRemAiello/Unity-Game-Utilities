using System;

namespace UnityEditor.GameUtils
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CreatableScriptableObjectAttribute : Attribute
    {
        public string MenuName { get; }

        public CreatableScriptableObjectAttribute(string menuName)
        {
            MenuName = menuName;
        }
    }
}