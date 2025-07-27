using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class RuntimeAttribute
    {
        [SerializeField, ReadOnly] private AttributeData _data;
        [SerializeField, ReadOnly] private float _baseValue;
        [SerializeField, ReadOnly] private float _currentValue;
        [SerializeField, ReadOnly] private List<Modifier> _modifiers;

        //
        public string ID => _data.ID;
        public AttributeData Data => _data;
        public float BaseValue => _baseValue;
        public float CurrentValue => _currentValue;
        public float MinValue => _data.MinValue;
        public float MaxValue => _data.MaxValue;

        //
        public RuntimeAttribute(AttributeData attributeData, float baseValue)
        {
            _data = attributeData;
            _baseValue = baseValue;
            _modifiers = new List<Modifier>();

            //
            RefreshCurrentValue();
        }

        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            RefreshCurrentValue();

            /*OnChangeValue?.Invoke();
            if (CurrentValue == m_Data.minValue && OnMinValue != null)
                OnMinValue();
            if (CurrentValue == m_Data.maxValue && OnMaxValue != null)
                OnMaxValue();*/
        }

        public void RemoveModifier(Modifier modifier)
        {
            _modifiers.Remove(modifier);
            RefreshCurrentValue();

            /*OnChangeValue?.Invoke();
            if (CurrentValue == m_Data.minValue && OnMinValue != null)
                OnMinValue();
            if (CurrentValue == m_Data.maxValue && OnMaxValue != null)
                OnMaxValue();*/
        }

        public void ClearModifiers()
        {
            _modifiers.Clear();
            RefreshCurrentValue();

            /*OnChangeValue?.Invoke();
            if (CurrentValue == m_Data.minValue && OnMinValue != null)
                OnMinValue();
            if (CurrentValue == m_Data.maxValue && OnMaxValue != null)
                OnMaxValue();*/
        }

        public void Refresh()
        {
            var mods = _modifiers.OrderBy(m => m.Order);
            foreach (var modifier in mods)
            {
                if (modifier.Duration > 0.0f)
                {
                    modifier.Duration -= Time.deltaTime;
                    if (modifier.Duration < 0.0f)
                    {
                        RemoveModifier(modifier);
                    }
                }
            }
        }

        protected virtual void RefreshCurrentValue()
        {
            _currentValue = _baseValue;
            _currentValue = ApplyModifiers(_currentValue);
            _currentValue = Mathf.Clamp(_currentValue, MinValue, MaxValue);
            _currentValue = ClampAttributeValue(_currentValue, _data.ClampType);
        }

        protected virtual float ApplyModifiers(float modValue)
        {
            var mods = _modifiers.OrderBy(m => m.Order);
            foreach (var modifier in mods)
            {
                modValue = modifier.ApplyModifier(modValue);
            }
            return modValue;
        }

        protected float ClampAttributeValue(float baseValue, AttributeClampType clampType)
        {
            return clampType switch
            {
                AttributeClampType.RawFloat => baseValue,
                AttributeClampType.Floor => Mathf.Floor(baseValue),
                AttributeClampType.Round => Mathf.Round(baseValue),
                AttributeClampType.Ceiling => Mathf.Ceil(baseValue),
                AttributeClampType.Integer => Mathf.RoundToInt(baseValue),
                _ => baseValue,
            };
        }
    }
}