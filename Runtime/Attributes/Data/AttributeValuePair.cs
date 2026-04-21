using System;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Couples an attribute definition with its initial value.
    /// </summary>
    [Serializable]
    public class AttributeValuePair
    {
        [SerializeField] private AttributeData _data;
        [SerializeField] private float _value;

        public AttributeValuePair(AttributeData data, float value)
        {
            _data = data;
            _value = value;
        }

        // Expose serialized data for runtime consumption.
        public AttributeData Data => _data;
        public float Value => _value;
    }
}
