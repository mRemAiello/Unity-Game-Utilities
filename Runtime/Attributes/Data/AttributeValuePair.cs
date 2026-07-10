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

        public AttributeValuePair(AttributeData attribute, float value)
        {
            _data = attribute;
            _value = value;
        }

        //
        public AttributeData Attribute => _data;
        public float Value => _value;
    }
}
