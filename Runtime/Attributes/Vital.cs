using System;

namespace RPGSystem
{
    public class Vital : Attribute
    {
        private float m_CurrentValue;

        public event EventHandler OnCurrentValueChange;

        public new float CurrentValue
        {
            get { return m_CurrentValue; }
            set
            {
                if (m_CurrentValue != value)
                {
                    m_CurrentValue = value;
                    OnCurrentValueChange?.Invoke(this, null);
                }
            }
        }

        public float CurrentMaxValue { get; set; }

        public Vital(AttributeData attributeData, float baseValue) : base(attributeData, baseValue)
        {
            m_CurrentValue = base.CurrentValue;
            CurrentMaxValue = base.CurrentValue;
        }
    }
}