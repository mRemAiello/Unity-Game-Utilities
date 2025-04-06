using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameUtils
{
    [DeclareBoxGroup("graphics", Title = "Graphics")]
    public class ItemAssetBase : ItemLocalizationData
    {
        [SerializeField, Group("graphics")] private AssetReferenceSprite _itemIcon = null;
        [SerializeField, Group("graphics")] private Color _itemColor = Color.white;

        //
        public AssetReferenceSprite AssetReferenceIcon => _itemIcon;
        public Sprite Icon => AssetLoader.LoadAssetSync<Sprite>(_itemIcon);
        public Color ItemColor => _itemColor;
    }
}