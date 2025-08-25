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
        [SerializeField, Group("class"), ShowIf(nameof(_startWithClass), true)] private ClassData _classData;
        [SerializeField, Group("class")] private bool _refreshClassOnUpdate = false;

        //
        [SerializeField, Group("debug")] private bool _logEnabled = true;
        [SerializeField, ReadOnly, HideInEditMode, TableList, Group("debug")] private List<RuntimeAttribute> _attributes;

        //
        public ClassData ClassData => _classData;
        public bool LogEnabled => _logEnabled;

        //
        void Start()
        {
            if (_startWithClass && _classData != null)
            {
                SetClass(_classData);
            }
        }

        void Update()
        {
            if (_refreshClassOnUpdate)
            {
                //RefreshAttributes();
            }
        }

        [Button]
        public void SetClass(ClassData classData)
        {
            _classData = classData;
            _attributes = new List<RuntimeAttribute>();
            foreach (var data in classData.Attributes)
            {
                var runtimeAttribute = CreateRuntimeAttribute(data.Data, data.Value);
                if (runtimeAttribute != null)
                    _attributes.Add(runtimeAttribute);
            }
        }

        private RuntimeAttribute CreateRuntimeAttribute(AttributeData data, float value)
        {
            if (data.IsVital)
                return new RuntimeVital(data, value);

            //
            return new RuntimeAttribute(data, value);
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
                if (attribute.Data is T)
                {
                    return attribute;
                }
            }
            return null;
        }
    }
}