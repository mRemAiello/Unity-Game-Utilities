using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUtils
{
    [DeclareBoxGroup("graphics", Title = "Graphics")]
    public class ItemAssetBase : UniqueID
    {
        [SerializeField, Group("internal")] private string _internalItemName = null;
        [SerializeField, Group("graphics")] private AssetReferenceSprite _itemIcon = null;
        [SerializeField, Group("graphics")] private LocalizedString _itemName = null;
        [SerializeField, Group("graphics")] private LocalizedString _itemDescription = null;

        //
        public Sprite Icon
        {
            get
            {
                if (_itemIcon == null)
                    return null;

                //
                if (_itemIcon.IsValid() && _itemIcon.OperationHandle.IsValid())
                {
                    if (_itemIcon.OperationHandle.Status == AsyncOperationStatus.None)
                    {
                        return _itemIcon.LoadAssetAsync<Sprite>().WaitForCompletion();
                    }
                    else
                    {
                        return _itemIcon.OperationHandle.Result as Sprite;
                    }
                }

                //
                return _itemIcon.LoadAssetAsync<Sprite>().WaitForCompletion();
            }
        }

        //
        public AssetReferenceSprite AssetSprite => _itemIcon;
        public string InternalName => _internalItemName;
        public string Name => _itemName.GetLocalizedString();
        public string Description => _itemDescription.GetLocalizedString();
    }
}