using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Localization;

namespace GameUtils
{
    [DeclareBoxGroup("Localization")]
    public abstract class ItemLocalizedData : ItemTagsData
    {
        [SerializeField, Group("Internal")] private string _internalItemName = "";
        [SerializeField, Group("Localization")] private LocalizedString _itemName = null;
        [SerializeField, Group("Localization")] private LocalizedString _itemDescription = null;

        //
        private string SafeLocalizedString(LocalizedString localizedString)
        {
            string value;
            try
            {
                value = localizedString.GetLocalizedString();
            }
            catch (Exception)
            {
                return "NO_LOCALIZED_STRING";
            }

            //
            return value;
        }

        //
        public string InternalName => _internalItemName;
        public string Name => SafeLocalizedString(_itemName);
        public string Description => SafeLocalizedString(_itemDescription);
    }
}