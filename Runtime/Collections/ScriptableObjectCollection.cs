using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameUtils
{
    public abstract class ScriptableObjectCollection : ScriptableObject
    {
        [SerializeField, ReadOnly] private List<string> _keys;

        //
        public List<ScriptableObject> Items { get; private set; }

        public virtual void LoadAssets()
        {
            Items = new List<ScriptableObject>();
            Addressables.LoadAssetsAsync<ScriptableObject>(_keys, addressable => { Items.Add(addressable); }, Addressables.MergeMode.Union);
        }
    }
}