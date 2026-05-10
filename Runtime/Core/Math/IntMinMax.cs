using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameUtils
{
    [Serializable]
    public class IntMinMax
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        // public fields
        public int Min
        {
            get => _min;
            set => _min = value;
        }

        public int Max
        {
            get => _max;
            set => _max = value;
        }

        public int GetValue()
        {
            return Random.Range(_min, _max + 1);
        }
    }
}