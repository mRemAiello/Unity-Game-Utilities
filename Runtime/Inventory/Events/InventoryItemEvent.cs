using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Inventory/Item Event")]
    public class InventoryItemEvent : GameEventAsset<InventoryItemEventArgs>
    {
    }
}
