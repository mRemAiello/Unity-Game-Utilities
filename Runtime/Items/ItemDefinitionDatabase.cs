using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.COLLECTABLES_NAME + "Item Definition Database")]
    public class ItemDefinitionDatabase : GenericDatabase<ItemDefinition>
    {
        private Dictionary<string, ItemDefinition> _itemsByInternalName = new();

        protected override void OnPostRefresh()
        {
            _itemsByInternalName = Items
                .Where(item => item != null && !string.IsNullOrWhiteSpace(item.InternalName))
                .GroupBy(item => item.InternalName)
                .ToDictionary(group => group.Key, group => group.First());
        }

        public bool TrySearchByInternalName(string internalName, out ItemDefinition result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(internalName))
            {
                return false;
            }

            return _itemsByInternalName.TryGetValue(internalName, out result);
        }

        public ItemDefinition SearchByInternalName(string internalName)
        {
            if (TrySearchByInternalName(internalName, out var result))
            {
                return result;
            }

            this.LogError($"ItemDefinition with internal name '{internalName}' not found.");
            return null;
        }
    }
}
