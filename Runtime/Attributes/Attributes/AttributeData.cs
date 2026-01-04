using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ATTRIBUTES_NAME + "Attribute")]
    [DeclareBoxGroup("attribute", Title = "Attribute")]
    public class AttributeData : ItemAssetBase
    {
        [SerializeField, Group("attribute")] private float _minValue = 0;
        [SerializeField, Group("attribute")] private float _maxValue = 99;
        [SerializeField, Group("attribute")] private bool _isVital;
        [SerializeField, Group("attribute")] private AttributeClampType _clampType;

        //
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public bool IsVital => _isVital;
        public AttributeClampType ClampType => _clampType;

        /// <summary>
        /// Computes the current value for a runtime attribute, applying modifiers and clamping.
        /// </summary>
        public float ComputeCurrentValue(float baseValue, IReadOnlyList<Modifier> modifiers)
        {
            var value = ApplyModifiers(baseValue, modifiers);
            value = Mathf.Clamp(value, _minValue, _maxValue);
            return ClampAttributeValue(value, _clampType);
        }

        /// <summary>
        /// Computes the current maximum value for a runtime vital attribute.
        /// </summary>
        public float ComputeCurrentMaxValue(float baseValue, IReadOnlyList<Modifier> modifiers)
        {
            var value = ApplyModifiers(baseValue, modifiers);
            value = Mathf.Clamp(value, _minValue, _maxValue);
            return ClampAttributeValue(value, _clampType);
        }

        private static float ApplyModifiers(float value, IReadOnlyList<Modifier> modifiers)
        {
            if (modifiers == null || modifiers.Count == 0)
            {
                return value;
            }

            var orderedModifiers = modifiers.OrderBy(modifier => modifier.Order);
            foreach (var modifier in orderedModifiers)
            {
                value = modifier.ApplyModifier(value);
            }

            return value;
        }

        private static float ClampAttributeValue(float value, AttributeClampType clampType)
        {
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
    }
}
