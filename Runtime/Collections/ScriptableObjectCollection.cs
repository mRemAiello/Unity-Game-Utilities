using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VInspector;

namespace GameUtils
{
    public abstract class ScriptableObjectCollection<T> : ScriptableObject where T : ScriptableObject
    {
        [SerializeField] private List<string> _keys;
        
        [Space]
        [SerializeField, ReadOnlyField] private List<T> _assets;

        [Button]
        public void LoadAssets()
        {
            _assets = new List<T>();

            //
            Addressables.LoadAssetsAsync<T>(_keys, addressable => { _assets.Add(addressable); }, Addressables.MergeMode.Union);
        }
    }
}