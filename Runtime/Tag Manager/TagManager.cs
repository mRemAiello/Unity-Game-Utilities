using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class TagManager
    {
        [SerializeField, ReadOnly] private SerializedDictionary<string, int> _values;

        public TagManager()
        {
            _values = new SerializedDictionary<string, int>();
        }

        public void SetTagValue(GameTag tag, bool value)
        {
            int intValue = Convert.ToInt32(value);
            SetTagValue(tag, intValue);
        }

        private int GetTagValue(string tag)
        {
            _values.TryGetValue(tag, out int value);
            return value;
        }

        private bool HasTag(string tag)
        {
            if (!_values.TryGetValue(tag, out int value))
            {
                return false;
            }
            return value > 0;
        }

        public Dictionary<string, int> GetMap() => _values;
        public bool TryGetValue(string tag, out int value) => _values.TryGetValue(tag, out value);
        public int GetTagValue(GameTag tag) => GetTagValue(tag.ID);
        private void SetTagValue(string tag, int value) => _values[tag] = value;
        public void SetTagValue(GameTag tag, int value) => SetTagValue(tag.ID, value);
        public bool HasTag(GameTag tag) => HasTag(tag.ID);
        public void Clear() => _values.Clear();
    }
}