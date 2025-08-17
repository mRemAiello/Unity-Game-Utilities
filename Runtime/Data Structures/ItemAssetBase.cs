using System.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameUtils
{
    [DeclareBoxGroup("graphics", Title = "Graphics")]
    public abstract class ItemAssetBase : ItemLocalizationData
    {
        [SerializeField, Group("graphics")] private AssetReferenceSprite _itemIcon = null;
        [SerializeField, Group("graphics")] private Color _itemColor = Color.white;

        //
        public AssetReferenceSprite AssetReferenceIcon => _itemIcon;
        // Icon sprite loaded asynchronously. Await the returned Task; if loading fails the task faults and the result is null.
        public Task<Sprite> Icon => AssetLoader.LoadAssetAsync<Sprite>(_itemIcon);
        public Color ItemColor => _itemColor;
    }
}
