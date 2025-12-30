using System;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameUtils
{
    [DeclareBoxGroup("graphics", Title = "Graphics")]
    [DeclareBoxGroup("features", Title = "Features")]
    public abstract class ItemAssetBase : ItemLocalizationData
    {
        [SerializeField, Group("graphics")] private AssetReferenceSprite _itemIcon = null;
        [SerializeField, Group("graphics")] private Color _itemColor = Color.white;
        [SerializeReference, Group("features")] private ItemFeature _itemFeature = null;

        //
        public AssetReferenceSprite AssetReferenceIcon => _itemIcon;
        public Task<Sprite> Icon => AssetLoader.LoadAssetAsync<Sprite>(_itemIcon);
        public Color ItemColor => _itemColor;

        //
        public bool TryGetFeature<TFeature>(out TFeature feature) where TFeature : ItemFeature
        {
            if (_itemFeature is TFeature typedFeature)
            {
                feature = typedFeature;
                return true;
            }

            feature = null;
            return false;
        }

        public TFeature GetFeature<TFeature>() where TFeature : ItemFeature => _itemFeature as TFeature;
    }
}
