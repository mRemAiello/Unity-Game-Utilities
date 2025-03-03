using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VInspector;

namespace GameUtils
{
    public abstract class ScriptableObjectCollection : ScriptableObject
    {
        [SerializeField] private List<string> _keys;

        //
        public List<ScriptableObject> Items { get; private set; }

        public virtual void LoadAssets()
        {
            Items = new List<ScriptableObject>();
            Addressables.LoadAssetsAsync<ScriptableObject>(_keys, addressable => { Items.Add(addressable); }, Addressables.MergeMode.Union);
        }
    }
}