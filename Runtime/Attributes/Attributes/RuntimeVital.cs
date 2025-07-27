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

        public event EventHandler OnCurrentValueChange;

        public new float CurrentValue
        {
            get => _currentVitalValue;
            set
            {
                float clamped = Mathf.Clamp(value, MinValue, _currentMaxValue);
                if (!Mathf.Approximately(_currentVitalValue, clamped))
                {
                    _currentVitalValue = clamped;
                    OnCurrentValueChange?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public float CurrentMaxValue => _currentMaxValue;

        public RuntimeVital(AttributeData attributeData, float baseValue) : base(attributeData, baseValue)
        {
            _currentMaxValue = base.CurrentValue;
            _currentVitalValue = base.CurrentValue;
        }

        protected override void RefreshCurrentValue()
        {
            base.RefreshCurrentValue();
            _currentMaxValue = base.CurrentValue;
            _currentVitalValue = Mathf.Clamp(_currentVitalValue, MinValue, _currentMaxValue);
        }
    }
}
