using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameUtils
{
    [DeclareBoxGroup("graphics", Title = "Graphics")]
    [DeclareBoxGroup("features", Title = "Features")]
    public abstract class ItemVisualData : ItemLocalizedData
    {
        [SerializeField, Group("graphics")] private AssetReferenceSprite _itemIcon = null;
        [SerializeField, Group("graphics")] private Color _itemColor = Color.white;
        [SerializeReference, Group("features")] private List<ItemFeatureData> _itemFeature = null;

        //
        public AssetReferenceSprite AssetReferenceIcon => _itemIcon;
        public Task<Sprite> Icon => AssetLoader.LoadAssetAsync<Sprite>(_itemIcon);
        public Color ItemColor => _itemColor;

        //
        public bool TryGetFeature<TFeature>(out TFeature feature) where TFeature : ItemFeatureData
        {
            foreach (var itemFeature in _itemFeature)
            {
                if (itemFeature is TFeature typedFeature)
                {
                    feature = typedFeature;
                    return true;
                }
            }

            feature = null;
            return false;
        }
    }
}
