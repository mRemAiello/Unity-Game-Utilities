using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VInspector;

namespace GameUtils
{
    public abstract class ScriptableObjectCollection : ScriptableObject
    {
        [SerializeField] private List<string> _keys;
        
        [Space]
        [SerializeField, ReadOnly] private List<ScriptableObject> _assets;

        //
        public List<ScriptableObject> Items => _assets;

        public virtual void LoadAssets()
        {
            _assets = new List<ScriptableObject>();

            //
            Addressables.LoadAssetsAsync<ScriptableObject>(_keys, addressable => { _assets.Add(addressable); }, Addressables.MergeMode.Union);
        }
    }
}