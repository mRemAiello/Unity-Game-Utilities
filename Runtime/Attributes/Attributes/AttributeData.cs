using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Scriptable definition for an attribute, including value limits and clamping rules.
    /// </summary>
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ATTRIBUTES_NAME + "Attribute")]
    [DeclareBoxGroup("Attribute")]
    public class AttributeData : ItemVisualData
    {
        [SerializeField, Group("Attribute")] private float _minValue = 0;
        [SerializeField, Group("Attribute")] private float _maxValue = 99;
        [SerializeField, Group("Attribute")] private bool _isVital;
        [SerializeField, Group("Attribute"), ShowIf("_isVital")] private bool _refreshCurrentValueOnChange = false;
        [SerializeField, Group("Attribute")] private AttributeClampType _clampType;

        // Inspector-facing configuration values.
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public bool IsVital => _isVital;
        public bool RefreshCurrentValueOnChange => _refreshCurrentValueOnChange;
        public AttributeClampType ClampType => _clampType;

        //
        protected virtual float ApplyModifiers(float value, IReadOnlyList<Modifier> modifiers)
        {
            if (modifiers == null || modifiers.Count == 0)
            {
                return value;
            }

            // Apply modifiers in deterministic order.
            var orderedModifiers = modifiers.OrderBy(modifier => modifier.Order);
            foreach (var modifier in orderedModifiers)
            {
                value = modifier.ApplyModifier(value);
            }

            return value;
        }

        public virtual float ClampAttributeValue(float value, AttributeClampType clampType)
        {
            // Convert to the desired numeric representation.
            return clampType switch
            {
                AttributeClampType.RawFloat => value,
                AttributeClampType.Floor => Mathf.Floor(value),
                AttributeClampType.Round => Mathf.Round(value),
                AttributeClampType.Ceiling => Mathf.Ceil(value),
                AttributeClampType.Integer => Mathf.RoundToInt(value),
                _ => value,
            };
        }

        /// <summary>
        /// Computes the current value for a runtime attribute, applying modifiers and clamping.
        /// </summary>
        public virtual float ComputeCurrentValue(ClassData classData, float baseValue, IReadOnlyList<Modifier> modifiers)
        {
            // Allow derived classes to react to class data while keeping base behavior unchanged.
            var value = ApplyModifiers(baseValue, modifiers);
            value = Mathf.Clamp(value, _minValue, _maxValue);
            return ClampAttributeValue(value, _clampType);
        }

        /// <summary>
        /// Computes the current maximum value for a runtime vital attribute.
        /// </summary>
        public virtual float ComputeCurrentMaxValue(ClassData classData, float baseValue, IReadOnlyList<Modifier> modifiers)
        {
            // Allow derived classes to react to class data while keeping base behavior unchanged.
            var value = ApplyModifiers(baseValue, modifiers);
            value = Mathf.Clamp(value, _minValue, _maxValue);
            return ClampAttributeValue(value, _clampType);
        }
    }
}
