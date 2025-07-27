using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("class", Title = "Class")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class RuntimeClass : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("class")] private bool _startWithClass = true;
        [SerializeField, Group("class")] private ClassData _classData;
        [SerializeField, Group("class")] private bool _refreshClassOnUpdate = false;

        //
        [SerializeField, Group("debug")] private bool _logEnabled = true;
        [SerializeField, ReadOnly, TableList, Group("Debug")] private List<RuntimeAttribute> _attributes;

        //
        public ClassData ClassData => _classData;
        public bool LogEnabled => _logEnabled;

        //
        void Start()
        {
            if (_startWithClass && _classData != null)
            {
                //RefreshAttributes();
            }
        }

        void Update()
        {
            if (_refreshClassOnUpdate)
            {
                //RefreshAttributes();
            }
        }

        public void SetClass(ClassData classData)
        {
            _classData = classData;
            if (_startWithClass)
            {
                //RefreshAttributes();
            }
        }

        //
        public bool TryGetAttribute<T>(out RuntimeAttribute attribute) where T : AttributeData
        {
            attribute = GetAttribute<T>();
            if (attribute == null)
            {
                this.LogError($"Attribute of type {typeof(T).Name} not found in class data.");
                return false;
            }
            return true;
        }

        public RuntimeAttribute GetAttribute<T>() where T : AttributeData
        {
            foreach (var attribute in _attributes)
            {
                if (attribute.Data is T data)
                {
                    return attribute;
                }
            }
            return null;
        }
    }
}