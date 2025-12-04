using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("class", Title = "Class")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class RuntimeClass : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("class")] protected bool _startWithClass = true;
        [SerializeField, Group("class"), ShowIf(nameof(_startWithClass), true), ShowProperties] protected ClassData _classData;
        [SerializeField, Group("class")] protected bool _refreshClassOnUpdate = false;

        //
        [SerializeField, Group("debug")] private bool _logEnabled = true;
        [SerializeField, ReadOnly, HideInEditMode, TableList, Group("debug")] protected List<RuntimeAttribute> _attributes;

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
                RefreshAttributes();
            }
        }

        [Button]
        public virtual void SetClass(ClassData classData)
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

        protected virtual RuntimeAttribute CreateRuntimeAttribute(AttributeData data, float value)
        {
            if (data.IsVital)
                return new RuntimeVital(data, value);

            //
            return new RuntimeAttribute(data, value);
        }

        public void RefreshAttributes()
        {
            if (_attributes == null || _attributes.Count == 0)
            {
                this.LogWarning("No attributes to refresh on this class instance.");
                return;
            }

            foreach (var attribute in _attributes)
            {
                attribute.Refresh();
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
                if (attribute.Data is T)
                {
                    return attribute;
                }
            }
            return null;
        }

        public bool TryGetAttribute(string attributeId, out RuntimeAttribute attribute)
        {
            attribute = GetAttribute(attributeId);
            if (attribute == null)
            {
                this.LogError($"Attribute with id {attributeId} not found in class data.");
                return false;
            }

            return true;
        }

        public bool TryGetAttribute(AttributeData attributeData, out RuntimeAttribute attribute)
        {
            attribute = GetAttribute(attributeData);
            if (attribute == null)
            {
                this.LogError($"Attribute {attributeData?.name ?? "<null>"} not found in class data.");
                return false;
            }

            return true;
        }

        public RuntimeAttribute GetAttribute(string attributeId)
        {
            if (string.IsNullOrEmpty(attributeId))
                return null;

            foreach (var attribute in _attributes)
            {
                if (attribute.Data.ID == attributeId)
                {
                    return attribute;
                }
            }

            return null;
        }

        public RuntimeAttribute GetAttribute(AttributeData attributeData)
        {
            if (attributeData == null)
                return null;

            foreach (var attribute in _attributes)
            {
                if (attribute.Data == attributeData || attribute.Data.ID == attributeData.ID)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}