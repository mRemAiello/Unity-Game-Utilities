using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Generic manager for <see cref="ItemAssetBase"/> assets, providing lookups by internal name.
    /// Inherits the automatic asset loading behaviour of <see cref="GenericDataManager{T1, T2}"/>.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public abstract class ItemAssetManager<TManager, TAsset> : GenericDataManager<TManager, TAsset>
        where TManager : ItemAssetManager<TManager, TAsset>
        where TAsset : ItemAssetBase
    {
        private Dictionary<string, TAsset> _itemsByInternalName = new();

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            _itemsByInternalName = Items
                .Where(item => !string.IsNullOrWhiteSpace(item.InternalName))
                .ToDictionary(item => item.InternalName, item => item);
        }

        public bool TrySearchByInternalName(string internalName, out TAsset result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(internalName))
            {
                return false;
            }

            return _itemsByInternalName.TryGetValue(internalName, out result);
        }

        public TAsset SearchByInternalName(string internalName)
        {
            if (TrySearchByInternalName(internalName, out var result))
            {
                return result;
            }

            this.LogError($"Asset with internal name '{internalName}' not found.");
            return null;
        }
    }
}
