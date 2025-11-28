using System;
using UnityEngine;

namespace GameUtils
{
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

        //
        public AttributeData Data => _data;
        public float Value => _value;
    }
}