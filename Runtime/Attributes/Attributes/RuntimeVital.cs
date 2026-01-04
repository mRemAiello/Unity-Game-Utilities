using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class RuntimeVital : RuntimeAttribute
    {
        [SerializeField, ReadOnly] private float _currentVitalValue;
        [SerializeField, ReadOnly] private float _currentMaxValue;

        //
        public override float CurrentValue => _currentVitalValue;
        public float CurrentMaxValue => _currentMaxValue;

        //
        public RuntimeVital(AttributeData attributeData, float baseValue) : base(attributeData, baseValue)
        {
            _currentMaxValue = Data.ComputeCurrentMaxValue(BaseValue, Modifiers);
            _currentVitalValue = Mathf.Clamp(base.CurrentValue, MinValue, _currentMaxValue);
        }

        public void SetCurrentValue(float value)
        {
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

        //
        protected virtual void OnCurrentValueChange() { }
    }
}
