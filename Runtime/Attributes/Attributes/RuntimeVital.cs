using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Runtime attribute with a separate current value and computed max value (vital-style stats).
    /// </summary>
    [Serializable]
    public class RuntimeVital : RuntimeAttribute
    {
        private float _currentMaxValue;

        // Expose vital-specific current values.
        public float CurrentMaxValue => _currentMaxValue;

        // Initialize max and current values based on the base attribute definition.
        public RuntimeVital(ClassData classData, AttributeData attributeData, float baseValue) : base(classData, attributeData, baseValue)
        {
            // Compute the initial current and max value with class context.
            _currentMaxValue = Data.ComputeCurrentMaxValue(ClassData, BaseValue, Modifiers);
            _currentValue = _data.ComputeCurrentValue(ClassData, BaseValue, Modifiers);
        }

        public void SetCurrentValue(float value)
        {
            // Clamp to min and computed max before notifying.
            float clamped = Mathf.Clamp(value, MinValue, _currentMaxValue);
            if (!Mathf.Approximately(_currentValue, clamped))
            {
                _currentValue = clamped;
                OnCurrentValueChange();
            }
        }

        /// <summary>
        /// Refreshes the derived max value via <see cref="AttributeData"/> and clamps the current vital value.
        /// </summary>
        protected override void RefreshCurrentValue()
        {
            // Percentage of the current value relative to the old max before refreshing.
            float percentage = _currentMaxValue > 0 ? _currentValue / _currentMaxValue : 0f;

            // Refresh the max value based on the current class data and modifiers.
            _currentMaxValue = Data.ComputeCurrentMaxValue(ClassData, BaseValue, Modifiers);

            // If configured to refresh the current value, clamp it to the new max while maintaining the same percentage.
            if (!Data.RefreshCurrentValueOnChange)
                return;

            _currentValue = Mathf.Clamp(_currentMaxValue * percentage, MinValue, _currentMaxValue);
        }

        // Override to react to changes in the current vital value.
        protected virtual void OnCurrentValueChange() { }
    }
}
