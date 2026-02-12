using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Localization;

namespace GameUtils
{
    [DeclareBoxGroup("localization", Title = "Localization")]
    public abstract class ItemLocalizedData : ItemTagsData
    {
        [SerializeField, Group("internal")] private string _internalItemName = "";
        [SerializeField, Group("localization")] private LocalizedString _itemName = null;
        [SerializeField, Group("localization")] private LocalizedString _itemDescription = null;

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