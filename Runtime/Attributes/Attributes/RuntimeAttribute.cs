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

        /// <summary>
        /// Gets the attribute identifier.
        /// </summary>
        public string ID => _data.ID;

        /// <summary>
        /// Gets the underlying attribute data.
        /// </summary>
        public AttributeData Data => _data;

        /// <summary>
        /// Gets the base (unmodified) value.
        /// </summary>
        public float BaseValue => _baseValue;

        /// <summary>
        /// Gets the current value computed by <see cref="AttributeData"/>.
        /// </summary>
        public virtual float CurrentValue => _currentValue;

        /// <summary>
        /// Gets the minimum allowed value.
        /// </summary>
        public float MinValue => _data.MinValue;

        /// <summary>
        /// Gets the maximum allowed value.
        /// </summary>
        public float MaxValue => _data.MaxValue;

        /// <summary>
        /// Gets the active modifiers.
        /// </summary>
        public IReadOnlyList<Modifier> Modifiers => _modifiers.AsReadOnly();

        /// <summary>
        /// Creates a runtime attribute with the given data and base value.
        /// </summary>
        public RuntimeAttribute(AttributeData attributeData, float baseValue)
        {
            _data = attributeData;
            _baseValue = baseValue;
            _modifiers = new List<Modifier>();

            //
            RefreshCurrentValue();
        }

        /// <summary>
        /// Adds a modifier and recalculates the current value.
        /// </summary>
        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            RefreshCurrentValue();
            HandleEvents();
        }

        /// <summary>
        /// Gets the first modifier matching the specified parameters.
        /// </summary>
        public Modifier GetModifier(object source, float amount = 0, float duration = 0, ModifierType modifierType = ModifierType.Neutral)
        {
            return _modifiers.FirstOrDefault(m => m.Source == source && Math.Abs(m.Amount - amount) < Mathf.Epsilon && Math.Abs(m.Duration - duration) < Mathf.Epsilon && m.ModifierType == modifierType);
        }

        /// <summary>
        /// Gets all modifiers created by the specified source.
        /// </summary>
        public List<Modifier> GetModifiersBySource(object source)
        {
            return _modifiers.Where(m => m.Source == source).ToList();
        }

        /// <summary>
        /// Returns true if a matching modifier exists.
        /// </summary>
        public bool HasModifier(object source, float amount = 0, float duration = 0, ModifierType modifierType = ModifierType.Neutral)
        {
            return GetModifier(source, amount, duration, modifierType) != null;
        }

        /// <summary>
        /// Removes a modifier and recalculates the current value.
        /// </summary>
        /// <param name="modifier">The modifier to remove.</param>
        /// <param name="includePermanent">When true, allows removal of permanent modifiers.</param>
        public void RemoveModifier(Modifier modifier, bool includePermanent = false)
        {
            if (!_modifiers.Contains(modifier))
            {
                Debug.LogWarning($"[RuntimeAttribute] Attempted to remove a modifier that does not exist on attribute {ID}.");
                return;
            }

            if (modifier.IsPermanent && !includePermanent)
            {
                return;
            }

            _modifiers.Remove(modifier);
            RefreshCurrentValue();
            HandleEvents();
        }

        /// <summary>
        /// Removes all modifiers and recalculates the current value.
        /// </summary>
        /// <param name="includePermanent">When true, also clears permanent modifiers.</param>
        public void ClearModifiers(bool includePermanent = false)
        {
            if (includePermanent)
            {
                _modifiers.Clear();
            }
            else
            {
                _modifiers.RemoveAll(modifier => !modifier.IsPermanent);
            }
            RefreshCurrentValue();
            HandleEvents();
        }

        /// <summary>
        /// Emits change, min, and max value events.
        /// </summary>
        protected virtual void HandleEvents()
        {
            //
            OnChangeValue();
            if (CurrentValue == _data.MinValue)
                OnMinValue();
            else if (CurrentValue == _data.MaxValue)
                OnMaxValue();
        }

        /// <summary>
        /// Updates modifier durations, removes expired ones, and recalculates the value.
        /// </summary>
        public virtual void Refresh()
        {
            var mods = _modifiers.OrderBy(m => m.Order);
            List<Modifier> expiredModifiers = null;

            foreach (var modifier in mods)
            {
                if (!modifier.IsPermanent && modifier.Duration > 0.0f)
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

        /// <summary>
        /// Recalculates the current value by delegating to <see cref="AttributeData"/>.
        /// </summary>
        protected virtual void RefreshCurrentValue()
        {
            _currentValue = _data.ComputeCurrentValue(_baseValue, _modifiers);
        }

        /// <summary>
        /// Called whenever the value changes.
        /// </summary>
        protected virtual void OnChangeValue() { }

        /// <summary>
        /// Called when the value hits the minimum.
        /// </summary>
        protected virtual void OnMinValue() { }

        /// <summary>
        /// Called when the value hits the maximum.
        /// </summary>
        protected virtual void OnMaxValue() { }
    }
}
