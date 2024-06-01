using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arbinty.Utils
{
    [Serializable]
    public class FloatMinMax
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        // public fields
        public float Min 
        { 
            get => _min;
            set => _min = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public float GetValue()
        {
            return Random.Range(_min, _max);
        }
    }
}