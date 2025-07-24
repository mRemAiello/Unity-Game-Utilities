using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGSystem
{
    public class Attribute
    {
        private AttributeData m_Data;
        private readonly float m_BaseValue;

        public delegate float OnCalculateModValueAction();
        public delegate void OnMinValueAction();
        public delegate void OnMaxValueAction();
        public delegate void OnChangeValueAction();

        public event OnCalculateModValueAction OnCalculateModValue;
        public event OnMinValueAction OnMinValue;
        public event OnMaxValueAction OnMaxValue;
        public event OnChangeValueAction OnChangeValue;

        public Guid ID
        {
            get { return m_Data.ID; }
        }

        public string Name
        {
            get { return m_Data.Name; }
        }

        public string Description
        {
            get { return m_Data.description; }
        }

        public float MinValue
        {
            get { return m_Data.minValue; }
        }

        public float MaxValue
        {
            get { return m_Data.maxValue; }
        }

        public List<Modifier> Modifiers
        {
            get;
            private set;
        }

        public float CurrentValue
        {
            get
            {
                float returnValue = 0;
                if (OnCalculateModValue != null)
                {
                    returnValue = OnCalculateModValue();
                }
                else
                {
                    returnValue = m_BaseValue;
                    returnValue = ApplyModifiers(returnValue);
                    returnValue = Mathf.Clamp(returnValue, MinValue, MaxValue);
                    returnValue = ClampAttributeValue(returnValue, m_Data.clampType);
                }                    

                return returnValue;
            }
        }

        public Attribute(AttributeData attributeData, float baseValue)
        {
            m_Data = attributeData;
            m_BaseValue = baseValue;
            Modifiers = new List<Modifier>();
        }

        public void Update()
        {
            var mods = Modifiers.OrderBy(m => m.Order);
            foreach (var modifier in mods)
            {
                if (modifier.Duration > 0.0f)
                {
                    modifier.Duration -= Time.deltaTime;
                    if (modifier.Duration < 0.0f)
                        RemoveModifier(modifier);
                }
            }
        }

        private float ApplyModifiers(float modValue)
        {
            var mods = Modifiers.OrderBy(m => m.Order);
            foreach (var modifier in mods)
                modValue = modifier.ApplyModifier(modValue);
            return modValue;
        }

        private float ClampAttributeValue(float baseValue, AttributeClampType clampType)
        {
            switch (clampType)
            {
                case AttributeClampType.RawFloat:
                    return baseValue;
                case AttributeClampType.Floor:
                    return Mathf.Floor(baseValue);
                case AttributeClampType.Round:
                    return Mathf.Round(baseValue);
            }
            return baseValue;
        }

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);

            OnChangeValue?.Invoke();
            if (CurrentValue == m_Data.minValue && OnMinValue != null)
                OnMinValue();
            if (CurrentValue == m_Data.maxValue && OnMaxValue != null)
                OnMaxValue();
        }

        public void RemoveModifier(Modifier modifier)
        {
            Modifiers.Remove(modifier);

            OnChangeValue?.Invoke();
            if (CurrentValue == m_Data.minValue && OnMinValue != null)
                OnMinValue();
            if (CurrentValue == m_Data.maxValue && OnMaxValue != null)
                OnMaxValue();
        }

        public void ClearModifiers()
        {
            Modifiers.Clear();

            OnChangeValue?.Invoke();
            if (CurrentValue == m_Data.minValue && OnMinValue != null)
                OnMinValue();
            if (CurrentValue == m_Data.maxValue && OnMaxValue != null)
                OnMaxValue();
        }

        public dynamic GetCustomValue<T>(string name)
        {
            return CustomValueManager.GetCustomValue<T>(m_Data.customValues, name);
        }

        public dynamic GetCustomValue<T>(Guid id)
        {
            return CustomValueManager.GetCustomValue<T>(m_Data.customValues, id);
        }
    }
}