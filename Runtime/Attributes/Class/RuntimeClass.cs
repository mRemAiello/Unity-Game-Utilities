using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// MonoBehaviour that instantiates and manages runtime attributes for a class.
    /// </summary>
    [DeclareBoxGroup("class", Title = "Class")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class RuntimeClass : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("class")] protected bool _startWithClass = true;
        [SerializeField, Group("class"), ShowIf(nameof(_startWithClass), true), ShowProperties] protected ClassData _classData;
        [SerializeField, Group("class")] protected bool _refreshClassOnUpdate = false;

        // Debugging/diagnostic settings.
        [SerializeField, Group("debug")] private bool _logEnabled = true;
        [SerializeField, ReadOnly, HideInEditMode, TableList, Group("debug")] protected List<RuntimeAttribute> _attributes;

        // Public accessors.
        public ClassData ClassData => _classData;
        public bool LogEnabled => _logEnabled;

        // Initialize the class data on startup when configured.
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
                // Keep attribute timers and values fresh every frame if needed.
                RefreshAttributes();
            }
        }

        [Button(ButtonSizes.Medium)]
        public virtual void SetClass(ClassData classData)
        {
            _classData = classData;
            _attributes = new List<RuntimeAttribute>();
            foreach (var data in classData.Attributes)
            {
                // Create a runtime instance for each attribute definition.
                var runtimeAttribute = CreateRuntimeAttribute(data.Data, data.Value);
                if (runtimeAttribute != null)
                    _attributes.Add(runtimeAttribute);
            }
        }

        protected virtual RuntimeAttribute CreateRuntimeAttribute(AttributeData data, float value)
        {
            if (data.IsVital)
                return new RuntimeVital(data, value);

            // Default to a standard runtime attribute for non-vitals.
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

            // Helper methods to locate attributes in the runtime list.
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
