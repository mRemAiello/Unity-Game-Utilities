using TriInspector;
using UnityEngine;
using UnityEngine.Localization;

namespace GameUtils
{
    [DeclareBoxGroup("localization", Title = "Localization")]
    public abstract class ItemLocalizedData : ItemIdentifierData
    {
        [SerializeField, Group("internal")] private string _internalItemName = null;
        [SerializeField, Group("localization")] private LocalizedString _itemName = null;
        [SerializeField, Group("localization")] private LocalizedString _itemDescription = null;

        //
        public string InternalName => _internalItemName;
        public string Name => _itemName.GetLocalizedString();
        public string Description => _itemDescription.GetLocalizedString();
    }
}