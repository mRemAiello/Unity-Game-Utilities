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
        public virtual float CurrentValue => _currentValue;
        public float MinValue => _data.MinValue;
        public float MaxValue => _data.MaxValue;
        public IReadOnlyList<Modifier> Modifiers => _modifiers.AsReadOnly();

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
            HandleEvents();
        }

        public Modifier GetModifier(object source, float amount = 0, float duration = 0, ModifierType modifierType = ModifierType.Neutral)
        {
            return _modifiers.FirstOrDefault(m => m.Source == source && Math.Abs(m.Amount - amount) < Mathf.Epsilon && Math.Abs(m.Duration - duration) < Mathf.Epsilon && m.ModifierType == modifierType);
        }

        public List<Modifier> GetModifiersBySource(object source)
        {
            return _modifiers.Where(m => m.Source == source).ToList();
        }

        public bool HasModifier(object source, float amount = 0, float duration = 0, ModifierType modifierType = ModifierType.Neutral)
        {
            return GetModifier(source, amount, duration, modifierType) != null;
        }

        public void RemoveModifier(Modifier modifier)
        {
            if (!_modifiers.Contains(modifier))
            {
                Debug.LogWarning($"[RuntimeAttribute] Attempted to remove a modifier that does not exist on attribute {ID}.");
                return;
            }

            //
            _modifiers.Remove(modifier);
            RefreshCurrentValue();
            HandleEvents();
        }

        public void ClearModifiers()
        {
            _modifiers.Clear();
            RefreshCurrentValue();
            HandleEvents();
        }

        protected virtual void HandleEvents()
        {
            //
            OnChangeValue();
            if (CurrentValue == _data.MinValue)
                OnMinValue();
            else if (CurrentValue == _data.MaxValue)
                OnMaxValue();
        }

        public virtual void Refresh()
        {
            var mods = _modifiers.OrderBy(m => m.Order);
            List<Modifier> expiredModifiers = null;

            foreach (var modifier in mods)
            {
                if (modifier.Duration > 0.0f)
                {
                    modifier.Duration -= Time.deltaTime;
                    if (modifier.Duration < 0.0f)
                    {
                        expiredModifiers ??= new List<Modifier>();
                        expiredModifiers.Add(modifier);
                    }
                }
            }

            if (expiredModifiers != null)
            {
                foreach (var modifier in expiredModifiers)
                {
                    _modifiers.Remove(modifier);
                }

                RefreshCurrentValue();
                HandleEvents();
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

        protected float ClampAttributeValue(float value, AttributeClampType clampType)
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

        //
        protected virtual void OnChangeValue() { }
        protected virtual void OnMinValue() { }
        protected virtual void OnMaxValue() { }
    }
}