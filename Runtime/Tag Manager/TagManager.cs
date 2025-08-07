using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public struct TagInfo
    {
        public string Name;
        public int Value;
    }

    [Serializable]
    public class TagManager
    {
        [SerializeField, ReadOnly] private SerializedDictionary<string, TagInfo> _values;

        public TagManager()
        {
            _values = new SerializedDictionary<string, TagInfo>();
        }

        public void SetTagValue(GameTag tag, bool value)
        {
            int intValue = Convert.ToInt32(value);
            SetTagValue(tag, intValue);
        }

        private int GetTagValue(string tag)
        {
            if (!_values.TryGetValue(tag, out TagInfo info))
            {
                return 0;
            }
            return info.Value;
        }

        private bool HasTag(string tag)
        {
            return _values.TryGetValue(tag, out TagInfo info) && info.Value > 0;
        }

        public bool HasTag<T>() where T : GameTag
        {
            foreach (var kvp in _values)
            {
                if (kvp.Value.Name == typeof(T).Name && kvp.Value.Value > 0)
                {
                    return true;
                }
            }
            return false;
        }

        //
        public Dictionary<string, TagInfo> GetMap() => _values;
        public bool TryGetValue(string tag, out TagInfo info) => _values.TryGetValue(tag, out info);
        public int GetTagValue(GameTag tag) => GetTagValue(tag.ID);
        public void SetTagValue(GameTag tag, int value) => _values[tag.ID] = new TagInfo { Name = tag.InternalName, Value = value };
        public bool HasTag(GameTag tag) => HasTag(tag.ID);
        public void Clear() => _values.Clear();
    }
}