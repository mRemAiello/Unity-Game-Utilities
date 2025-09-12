using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class TagManager
    {
        [SerializeField, ReadOnly] private SerializedDictionary<string, RuntimeTag> _values;

        public TagManager()
        {
            _values = new SerializedDictionary<string, RuntimeTag>();
        }

        public void SetTagValue(GameTag tag, bool value)
        {
            int intValue = Convert.ToInt32(value);
            SetTagValue(tag, intValue);
        }

        public bool HasAny<T>() where T : GameTag
        {
            foreach (var kvp in _values)
            {
                if (kvp.Value.Tag.GetType() == typeof(T) && kvp.Value.Value > 0)
                {
                    return true;
                }
            }
            return false;
        }

        //
        public Dictionary<string, RuntimeTag> GetMap() => _values;
        public bool TryGetValue(string tag, out RuntimeTag runtimeTag) => _values.TryGetValue(tag, out runtimeTag);
        public void SetTagValue(GameTag tag, int value) => _values[tag.ID] = new RuntimeTag { Tag = tag, Value = value };
        private bool HasAny(string tag) => _values.TryGetValue(tag, out RuntimeTag info) && info.Value > 0;
        public bool HasAny(GameTag tag) => HasAny(tag.ID);
        public void Clear() => _values.Clear();
    }
}