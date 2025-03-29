using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        // Save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // Load dictionary from lists
        public void OnAfterDeserialize()
        {
            Clear();

            //
            if (keys.Count != values.Count)
                throw new Exception($"Mismatched keys and values in {typeof(SerializedDictionary<TKey, TValue>)}");

            //
            for (int i = 0; i < keys.Count; i++)
            {
                this[keys[i]] = values[i];
            }
        }
    }
}