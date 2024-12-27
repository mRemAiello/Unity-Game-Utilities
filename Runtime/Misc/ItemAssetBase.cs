using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using VInspector;

namespace GameUtils
{
    public class ItemAssetBase : UniqueID
    {
        [Tab("Graphics")]
        [SerializeField] private AssetReferenceSprite _itemIcon = null;

        [Tab("Localization")]
        [SerializeField] private string _internalItemName = null;
        [SerializeField] private LocalizedString _itemName = null;
        [SerializeField] private LocalizedString _itemDescription = null;

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