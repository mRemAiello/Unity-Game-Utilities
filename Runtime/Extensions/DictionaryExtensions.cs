using System;
using System.Collections.Generic;

namespace GameUtils
{
    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this TKey[] keys, TValue[] values)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (keys.Length != values.Length) throw new ArgumentException("Size keys and size values diffirent!");

            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (var i = 0; i < keys.Length; i++)
            {
                result.Add(keys[i], values[i]);
            }

            return result;
        }

        public static IDictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this IList<TKey> keys, IList<TValue> values)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (keys.Count != values.Count) throw new ArgumentException("Size keys and size values diffirent!");

            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (var i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i], values[i]);
            }

            return result;
        }
    }
}