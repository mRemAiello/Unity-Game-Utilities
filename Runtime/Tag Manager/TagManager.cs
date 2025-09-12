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

        public bool HasAll(params GameTag[] tags)
        {
            foreach (var tag in tags)
            {
                if (!HasAny(tag))
                {
                    return false;
                }
            }
            return true;
        }

        public List<GameTag> Intersection(params GameTag[] tags)
        {
            var result = new List<GameTag>();
            foreach (var tag in tags)
            {
                if (HasAny(tag))
                {
                    result.Add(tag);
                }
            }
            return result;
        }

        public List<GameTag> Union(params GameTag[] tags)
        {
            var result = new List<GameTag>();
            foreach (var kvp in _values)
            {
                if (kvp.Value.Value > 0)
                {
                    result.Add(kvp.Value.Tag);
                }
            }

            foreach (var tag in tags)
            {
                if (!result.Contains(tag))
                {
                    result.Add(tag);
                }
            }
            return result;
        }

        //
        public Dictionary<string, RuntimeTag> GetMap() => _values;
        public bool TryGetValue(string tag, out RuntimeTag runtimeTag) => _values.TryGetValue(tag, out runtimeTag);
        public void SetTagValue(GameTag tag, int value) => _values[tag.ID] = new RuntimeTag { Tag = tag, Value = value };
        private bool HasAny(string tag) => _values.TryGetValue(tag, out RuntimeTag info) && info.Value > 0;
        public bool HasAny(GameTag tag) => HasAny(tag.ID);
        public static bool HasAny(ITaggable taggable, params GameTag[] tags)
        {
            foreach (var tag in tags)
            {
                if (taggable.Tags.Contains(tag))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasAll(ITaggable taggable, params GameTag[] tags)
        {
            foreach (var tag in tags)
            {
                if (!taggable.Tags.Contains(tag))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<GameTag> Intersection(ITaggable taggable, params GameTag[] tags)
        {
            var result = new List<GameTag>();
            foreach (var tag in tags)
            {
                if (taggable.Tags.Contains(tag))
                {
                    result.Add(tag);
                }
            }
            return result;
        }

        public static List<GameTag> Union(ITaggable taggable, params GameTag[] tags)
        {
            var result = new List<GameTag>(taggable.Tags);
            foreach (var tag in tags)
            {
                if (!result.Contains(tag))
                {
                    result.Add(tag);
                }
            }
            return result;
        }
        public void Clear() => _values.Clear();
    }
}