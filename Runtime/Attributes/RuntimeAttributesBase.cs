using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Attributes")]
    [DeclareBoxGroup("Debug")]
    public abstract class RuntimeAttributesBase : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("Attributes")] protected bool _refreshAttributesOnUpdate = false;

        //
        [SerializeField, Group("Debug")] private bool _logEnabled = true;
        [SerializeField, ReadOnly, HideInEditMode, TableList, Group("Debug")] protected List<RuntimeAttribute> _attributes;
        [SerializeField, Group("Debug")] private DebugInfo _debugInfo;

        //
        public bool LogEnabled => _logEnabled;

        //
        void Start()
        {
            _debugInfo = GetComponent<DebugInfo>();

            //
            Init();
        }

        protected virtual void Init()
        {
        }

        void Update()
        {
            if (_refreshAttributesOnUpdate)
            {
                // Keep attribute timers and values fresh every frame if needed.
                RefreshAttributes();
            }
        }

        protected virtual RuntimeAttribute CreateRuntimeAttribute(ClassData classData, AttributeData data, float value)
        {
            // Pass class data so attribute calculations can consider class context.
            if (data.IsVital)
                return new RuntimeVital(classData, data, value);

            // Default to a standard runtime attribute for non-vitals.
            return new RuntimeAttribute(classData, data, value);
        }

        public void RefreshAttributes()
        {
            if (_attributes == null || _attributes.Count == 0)
            {
                this.LogWarning("No attributes to refresh on this class instance.");
                return;
            }

            if (_debugInfo != null)
            {
                _debugInfo.Info = "";
            }

            // Refresh each attribute and update debug info if available.
            foreach (var attribute in _attributes)
            {
                attribute.Refresh();
                if (_debugInfo != null)
                {
                    _debugInfo.Info += $"{attribute}\n";
                }
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

        public RuntimeVital GetVital(string attributeId)
        {
            var attribute = GetAttribute(attributeId);
            if (attribute is RuntimeVital vital)
                return vital;

            return null;
        }

        public RuntimeVital GetVital(AttributeData attributeData)
        {
            var attribute = GetAttribute(attributeData);
            if (attribute is RuntimeVital vital)
                return vital;

            return null;
        }

        public void IncreaseValue(AttributeData attributeData, float amount)
        {
            RuntimeVital vital = GetVital(attributeData);
            if (vital != null && amount >= 0)
            {
                vital.SetCurrentValue(vital.CurrentValue + amount);
            }
        }

        public void DecreaseValue(AttributeData attributeData, float amount)
        {
            RuntimeVital vital = GetVital(attributeData);
            if (vital != null && amount >= 0)
            {
                vital.SetCurrentValue(vital.CurrentValue - amount);
            }
        }

        [Button(ButtonSizes.Medium)]
        public void AddModifier(AttributeData attributeData, Modifier modifier)
        {
            if (TryGetAttribute(attributeData, out var attribute))
            {
                attribute.AddModifier(modifier);
            }
        }

        [Button(ButtonSizes.Medium)]
        public void AddModifier(string attributeID, Modifier modifier)
        {
            if (TryGetAttribute(attributeID, out var attribute))
            {
                attribute.AddModifier(modifier);
            }
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveModifier(AttributeData attributeData, Modifier modifier)
        {
            if (TryGetAttribute(attributeData, out var attribute))
            {
                attribute.RemoveModifier(modifier);
            }
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveModifier(string attributeID, Modifier modifier)
        {
            if (TryGetAttribute(attributeID, out var attribute))
            {
                attribute.RemoveModifier(modifier);
            }
        }

        [Button(ButtonSizes.Medium)]
        public void ClearModifiers(AttributeData attributeData)
        {
            if (TryGetAttribute(attributeData, out var attribute))
            {
                attribute.ClearModifiers();
            }
        }

        [Button(ButtonSizes.Medium)]
        public void ClearModifiers(string attributeID)
        {
            if (TryGetAttribute(attributeID, out var attribute))
            {
                attribute.ClearModifiers();
            }
        }
    }
}