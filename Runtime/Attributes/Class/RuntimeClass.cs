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

        /// <summary>
        /// Try to get an attribute by data type and runtime instance type.
        /// </summary>
        /// <typeparam name="TData">The attribute data type to match.</typeparam>
        /// <typeparam name="TRuntime">The runtime attribute type to return.</typeparam>
        /// <param name="attribute">The matching runtime attribute, if found.</param>
        /// <returns>True when a matching runtime attribute is found.</returns>
        public bool TryGetAttribute<TData, TRuntime>(out TRuntime attribute)
            where TData : AttributeData
            where TRuntime : RuntimeAttribute
        {
            attribute = GetAttribute<TData, TRuntime>();
            if (attribute == null)
            {
                this.LogError($"Attribute of type {typeof(TData).Name} with runtime {typeof(TRuntime).Name} not found in class data.");
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

        /// <summary>
        /// Get an attribute by data type and runtime instance type.
        /// </summary>
        /// <typeparam name="TData">The attribute data type to match.</typeparam>
        /// <typeparam name="TRuntime">The runtime attribute type to return.</typeparam>
        /// <returns>The matching runtime attribute or null if not found or mismatched.</returns>
        public TRuntime GetAttribute<TData, TRuntime>()
            where TData : AttributeData
            where TRuntime : RuntimeAttribute
        {
            foreach (var attribute in _attributes)
            {
                if (attribute.Data is TData)
                {
                    if (attribute is TRuntime runtimeAttribute)
                        return runtimeAttribute;

                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Get a vital runtime attribute for the requested data type.
        /// </summary>
        /// <typeparam name="TData">The attribute data type to match.</typeparam>
        /// <returns>The matching RuntimeVital or null if not found.</returns>
        public RuntimeVital GetVital<TData>() where TData : AttributeData
        {
            return GetAttribute<TData, RuntimeVital>();
        }

        /// <summary>
        /// Try to get a vital runtime attribute for the requested data type.
        /// </summary>
        /// <typeparam name="TData">The attribute data type to match.</typeparam>
        /// <param name="attribute">The matching RuntimeVital, if found.</param>
        /// <returns>True when a matching RuntimeVital is found.</returns>
        public bool TryGetVital<TData>(out RuntimeVital attribute) where TData : AttributeData
        {
            return TryGetAttribute<TData, RuntimeVital>(out attribute);
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
