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
        [SerializeField, ReadOnly] private float _currentVitalValue;
        [SerializeField, ReadOnly] private float _currentMaxValue;

        // Expose vital-specific current values.
        public override float CurrentValue => _currentVitalValue;
        public float CurrentMaxValue => _currentMaxValue;

        // Initialize max and current values based on the base attribute definition.
        public RuntimeVital(AttributeData attributeData, float baseValue) : base(attributeData, baseValue)
        {
            _currentMaxValue = Data.ComputeCurrentMaxValue(BaseValue, Modifiers);
            _currentVitalValue = Mathf.Clamp(base.CurrentValue, MinValue, _currentMaxValue);
        }

        public void SetCurrentValue(float value)
        {
            // Clamp to min and computed max before notifying.
            float clamped = Mathf.Clamp(value, MinValue, _currentMaxValue);
            if (!Mathf.Approximately(_currentVitalValue, clamped))
            {
                _currentVitalValue = clamped;
                OnCurrentValueChange();
            }
        }

        /// <summary>
        /// Refreshes the derived max value via <see cref="AttributeData"/> and clamps the current vital value.
        /// </summary>
        protected override void RefreshCurrentValue()
        {
            base.RefreshCurrentValue();
            _currentMaxValue = Data.ComputeCurrentMaxValue(BaseValue, Modifiers);
            _currentVitalValue = Mathf.Clamp(_currentVitalValue, MinValue, _currentMaxValue);
        }

        // Override to react to changes in the current vital value.
        protected virtual void OnCurrentValueChange() { }
    }
}
